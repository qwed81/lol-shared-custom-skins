using ClientLib;
using ClientLib.Models;
using Models.Network;
using SharedLib;

namespace SCSClient.NetworkClient
{
    public class Client
    {

        private EventStore _eventStore;
        private Connector _connector;
        private Authenticator _authenticator;
        private MessageChanel _messageChanel;
        private HostMessageThread _hostMessageThread;
        private RemoteConnectionState _connectionState;

        public Client(EventStore eventStore, Connector connector, RemoteConnectionState connectionState,
            Authenticator authenticator, MessageChanel chanel, HostMessageThread hostHandler)
        {
            _eventStore = eventStore;
            _connector = connector;
            _authenticator = authenticator;
            _messageChanel = chanel;
            _hostMessageThread = hostHandler;
            _connectionState = connectionState;
        }

        public void Connect(InviteInfo invite)
        {
            _connectionState.Disconnect();

            Task.Run(async () =>
            {
                var connectionResult = await _connector.ConnectMessageChanel(invite.PublicIP, invite.Port);
                if (connectionResult.Failed)
                    return;

                var authResult = await authenticate(connectionResult.Value, invite);
                if (authResult.Failed)
                    return;

                var auth = authResult.Value;
                _connectionState.ConnectComplete(connectionResult.Value, auth.UserId, auth.PrivateAccessToken);

                _hostMessageThread.Start(); // runs connection until closed

                while(true)
                {
                    var message = _eventStore.PollOutboundMessage(EventUser.ClientMessageThread);
                    var msgResult = await _messageChanel.SendMessage(_connectionState.Connection.Output, 
                        message.MessageType, message.Message);

                    if (msgResult.Failed)
                        return;
                }

            });
        }

        private async Task<IOResult<ClientInfo>> authenticate(Connection connection, InviteInfo invite)
        {
            var result = await _authenticator.SendAuthenticationRequest(connection.Output, invite.Password, invite.Admin);
            if (result.Failed)
                return IOResult.CreateFailure<ClientInfo>(result.ErrorType);

            return await _authenticator.RecieveAuthenticationInfo(connection.Input);
        }

        public void Disconnect()
        {
            _connectionState.Disconnect();
        }

    }
}
