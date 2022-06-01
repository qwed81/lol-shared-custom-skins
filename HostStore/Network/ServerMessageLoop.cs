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

    internal class ServerMessageLoop
    {

        private bool _canceled;
        private TcpClientWrapper _client;

        public Guid SessionId { get; }

        public Guid UserId { get; }


        public ServerMessageLoop(Guid sessionId, Guid userId, TcpClientWrapper client)
        {
            SessionId = sessionId;
            UserId = userId;

            _canceled = false;
            _client = client;
        }

        public async Task<Exception> ProcessUntilClosedAsync(Action<ServerMessageLoop, Type, object> onMessage)
        {
            try
            {
                while (_canceled == false)
                {
                    var metadata = await _client.ExpectObjectAsync<ClientMessageMetadata>("Message not valid");

                    Type? messageType = SerializeUtil.StringToType(metadata.MessageType);
                    if (messageType == null)
                        throw new Exception("Message not valid");

                    var message = await _client.ExpectObjectAsync<object>("Message not valid", messageType);
                    onMessage(this, messageType, message);
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

        public void Cancel()
        {
            _canceled = true;
        }
    }
}
