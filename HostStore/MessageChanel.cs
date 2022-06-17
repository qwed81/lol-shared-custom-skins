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

namespace HostStore
{

    internal class MessageChanel : IAuthenticatedUser
    {

        private bool _canceled;
        private TcpClientWrapper _client;
        private Guid _sessionId;

        public Guid UserId { get; }

        public Guid PrivateAccessKey { get; }

        public bool Admin { get; private set; }

        public MessageChanel(TcpClientWrapper client, Guid sessionId)
        {
            _sessionId = sessionId;

            UserId = Guid.NewGuid();
            PrivateAccessKey = Guid.NewGuid();

            _canceled = false;
            _client = client;
        }

        public async Task HandleAuthenticationAsync(ConnectMessageChanelRequest req, IAuthenticationManager authManager)
        {
            ServerResponse res = new ConnectMessageChanelResponse(_sessionId, UserId, PrivateAccessKey);
            if (authManager.InviteExists(req.Password) == false)
            {
                res = ServerResponse.CreateFailureResponse(RequestType.CONNECT_MESSAGE_CHANEL, 
                    "Password does not exist");               
            }
            else
            {
                authManager.ConsumeInvite(req.Password);
                res.Success = true;
            }

            await _client.WriteObjectAsync(res);
        }

        public async Task ProcessUntilClosedAsync(IMessageHandler messageHandler)
        {
            while (_canceled == false)
            {
                var metadata = await _client.ExpectObjectAsync<ClientMessageMetadata>("Message not valid");

                Type? messageType = SerializeUtil.StringToType(metadata.MessageType);
                if (messageType == null)
                    throw new Exception("Message not valid");

                var message = await _client.ExpectObjectAsync<object>("Message not valid", messageType);
                
                messageHandler.HandleMessage(this, messageType, message);
            }
        }

        public async Task PostMessageAsync(Type messageType, object message)
        {
            string messageTypeStr = SerializeUtil.TypeToString(messageType);
            var metadata = new ServerMessageMetadata(Guid.NewGuid(), messageTypeStr);

            await _client.WriteObjectsAsync(metadata, message);
        }

        public void Cancel()
        {
            _canceled = true;
        }
    }
}
