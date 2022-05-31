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

namespace HostStore.Network
{

    internal delegate void MessageHandler(ServerMessageLoop sender, Type messageType, object message);

    internal class ServerMessageLoop : IDisposable
    {

        private bool _canceled;
        private TcpClient _tcpClient;
        private StreamWriter _writer;
        private StreamReader _reader;

        public Guid SessionId { get; }

        public Guid UserId { get; }

        public event MessageHandler? OnMessage;

        public ServerMessageLoop(Guid sessionId, Guid userId, TcpClient client)
        {
            SessionId = sessionId;
            UserId = userId;

            _canceled = false;
            _tcpClient = client;
            _reader = new StreamReader(client.GetStream());
            _writer = new StreamWriter(client.GetStream());
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
            if (_writer == null)
                throw new Exception("Can not post message before connection");

            string messageTypeStr = SerializeUtil.TypeToString(messageType);
            var metadata = new ClientMessageMetadata(SessionId, UserId, Guid.NewGuid(), messageTypeStr);

            await writeObjectAsync(metadata);
            await writeObjectAsync(message);
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
