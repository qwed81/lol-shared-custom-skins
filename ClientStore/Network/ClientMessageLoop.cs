using StoreModels;
using StoreModels.Messages;
using StoreModels.Messages.Client;
using StoreModels.Messages.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClientStore.Network
{

    internal delegate void MessageHandler(ClientMessageLoop sender, Type messageType, object message);

    internal class ClientMessageLoop : IDisposable
    {

        private bool _canceled;
        private TcpClient _tcpClient;
        private StreamWriter _writer;
        private StreamReader _reader;

        public Guid SessionId { get; private set; }

        public Guid UserId { get; private set; }

        public event MessageHandler? OnMessage;

        private ClientMessageLoop(TcpClient client)
        {
            _canceled = false;
            _tcpClient = client;
            _reader = new StreamReader(client.GetStream());
            _writer = new StreamWriter(client.GetStream());
        }

        public static async Task<ClientMessageLoop> ConnectLoopAsync(string host, int port, Guid requestSessionId,
            bool admin, string password, UserInfo initUserInfo)
        {
            TcpClient client = new TcpClient();
            try
            {
                await client.ConnectAsync(host, port);

                var messageLoop = new ClientMessageLoop(client);
                await messageLoop.authenticateAsync(requestSessionId, admin, password, initUserInfo);

                return messageLoop;
            }
            finally // if an error occurs with connection, still close the client
            {
                client.Close();
            }
        }

        public async Task<Exception> ProcessUntilClosedAsync()
        {
            try
            {
                while (_canceled == false)
                {
                    var (type, message) = await readMessageAsync();
                    OnMessage?.Invoke(this, type, message);
                }
            } 
            catch (Exception ex)
            {
                return ex;
            }

            return new Exception("Closed manually");
        }

        private async Task<(Type, object)> readMessageAsync()
        {
            var metadata = await readObjectAsync(typeof(ServerMessageMetadata)) as ServerMessageMetadata;
            if (metadata == null)
                throw new Exception("Invalid message from server");

            Type? messageType = SerializeUtil.StringToType(metadata.MessageType);
            if (messageType == null)
                throw new Exception("Invalid message type");

            object? message = await readObjectAsync(messageType);
            if (message == null)
                throw new Exception("Invalid message from server");

            return (messageType, message);
        }

        public async Task PostMessageAsync(Type messageType, object message)
        {
            string messageTypeStr = SerializeUtil.TypeToString(messageType);
            var metadata = new ClientMessageMetadata(SessionId, UserId, Guid.NewGuid(), messageTypeStr);

            await writeObjectAsync(metadata);
            await writeObjectAsync(message);
        }

        private async Task authenticateAsync(Guid sessionId, bool admin, string password, UserInfo initUserInfo)
        {
            // send auth info
            var authInfo = new AuthenticationRequest(sessionId, admin, password, initUserInfo);
            await writeObjectAsync(authInfo);

            // recive auth response
            var authResponse = (await readObjectAsync(typeof(ServerAuthenticationResponse)) as ServerAuthenticationResponse);
            if (authResponse == null)
                throw new Exception("Server did not send proper authentication response");

            if (authResponse.Success != true)
                throw new Exception($"Authentication failed, reason: {authResponse.FailureReason!}");

            SessionId = authResponse.SessionId;
            UserId = authResponse.UserId;
        }

        private async Task writeObjectAsync(object obj)
        {
            string objJson = SerializeUtil.SerializeObject(obj);
            await _writer.WriteLineAsync(objJson);
            await _writer.FlushAsync();
        }

        private async Task<object?> readObjectAsync(Type type)
        {
            string? json = await _reader.ReadLineAsync();
            return SerializeUtil.DeserializeObject(type, json);
        }

        public void Dispose()
        {
            _canceled = true;
            _tcpClient.Close();
            _reader.Dispose();
            _writer.Dispose();
        }
    }
}
