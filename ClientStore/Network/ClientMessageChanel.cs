using ClientStore.ModelLogic;
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

    internal class ClientMessageChanel : IDisposable
    {

        private bool _canceled;
        private TcpClientWrapper _client;
        private IClientMessageHandler _handler;

        public Guid SessionId { get; private set; }

        public Guid UserId { get; private set; }

        public Guid PrivateAccessToken { get; set; }

        private ClientMessageChanel(TcpClient client, IClientMessageHandler messageHandler)
        {
            _canceled = false;
            _client = new TcpClientWrapper(client);
            _handler = messageHandler;
        }

        public static async Task<ClientMessageChanel> ConnectChanelAsync(ClientConnectionInfo info, 
            IClientMessageHandler messageHandler)
        {
            TcpClient client = new TcpClient();
            try
            {
                await client.ConnectAsync(info.Host, info.Port);

                var chanel = new ClientMessageChanel(client, messageHandler);

                var initUserInfo = new UserInfo(info.InitUsername, info.InitStatus, info.InitProfilePicture, Guid.Empty);
                var authInfo = new ConnectMessageChanelRequest(info.SessionRequestId, info.Admin, info.Password, initUserInfo);
                await chanel.authenticateAsync(authInfo);

                return chanel;
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

                    _handler.HandleMessage(messageType, message);
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

        private async Task authenticateAsync(ConnectMessageChanelRequest req)
        {
            // send auth info
            await _client.WriteObjectAsync(req);

            // recive auth response
            var authResponse = await _client.ExpectObjectAsync<ConnectMessageChanelResponse>("Auth response not valid");

            if (authResponse.Success != true)
                throw new Exception($"Authentication failed, reason: {authResponse.FailureReason!}");

            SessionId = authResponse.SessionId;
            UserId = authResponse.UserId;
            PrivateAccessToken = authResponse.PrivateAccessToken;
        }

        public void Dispose()
        {
            _canceled = true;
            _client.Client.Dispose();
            _client.Dispose();
        }
    }
}
