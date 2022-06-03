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

    internal delegate void MessageHandler(ClientMessageChanel sender, Type messageType, object message);

    internal class ClientMessageChanel : IDisposable
    {

        private bool _canceled;
        private TcpClientWrapper _client;

        public Guid SessionId { get; private set; }

        public Guid UserId { get; private set; }

        public event MessageHandler? OnMessage;

        private ClientMessageChanel(TcpClient client)
        {
            _canceled = false;
            _client = new TcpClientWrapper(client);
        }

        public static async Task<ClientMessageChanel> ConnectChanelAsync(string host, int port, Guid requestSessionId,
            bool admin, string password, UserInfo initUserInfo)
        {
            TcpClient client = new TcpClient();
            try
            {
                await client.ConnectAsync(host, port);

                var messageLoop = new ClientMessageChanel(client);
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
                    var metadata = await _client.ExpectObjectAsync<ServerMessageMetadata>("Server message not valid");

                    Type? messageType = SerializeUtil.StringToType(metadata.MessageType);
                    if (messageType == null)
                        throw new Exception("Server message not valid");

                    var message = await _client.ExpectObjectAsync<object>("Server message not valid", messageType);

                    OnMessage?.Invoke(this, messageType, message);
                }
            } 
            catch (Exception ex)
            {
                return ex;
            }

            return new Exception("Closed manually");
        }

        public async Task PostMessageAsync(Type messageType, object message)
        {
            string messageTypeStr = SerializeUtil.TypeToString(messageType);
            var metadata = new ClientMessageMetadata(SessionId, UserId, Guid.NewGuid(), messageTypeStr);

            await _client.WriteObjectsAsync(metadata, message);
        }

        private async Task authenticateAsync(Guid sessionId, bool admin, string password, UserInfo initUserInfo)
        {
            // send auth info
            var authInfo = new ConnectMessageChanelRequest(sessionId, admin, password, initUserInfo);
            await _client.WriteObjectAsync(authInfo);

            // recive auth response
            var authResponse = await _client.ExpectObjectAsync<ConnectMessageChanelResponse>("Auth response not valid");

            if (authResponse.Success != true)
                throw new Exception($"Authentication failed, reason: {authResponse.FailureReason!}");

            SessionId = authResponse.SessionId;
            UserId = authResponse.UserId;
        }

        public void Dispose()
        {
            _canceled = true;
            _client.Client.Dispose();
            _client.Dispose();
        }
    }
}
