using HostLib;
using HostLib.Models;
using Models.Network;
using Models.Network.Messages.Client;
using Models.Network.Messages.Server;
using SharedLib;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSHost.ConnectionHandlers
{
    public class MessageChanelHandler : IConnectionHandler
    {

        private HostState _state;
        private HostAuthenticator _authenticator;
        private HostMessageChanel _messageChanel;

        public MessageChanelHandler(HostState state, HostAuthenticator authenticator, HostMessageChanel messageChanel)
        {
            _state = state;
            _authenticator = authenticator;
            _messageChanel = messageChanel;
        }

        public async Task<IOErrorType> HandleConnection(Connection connection)
        {
            var authResult = await authenticate(connection);
            if (authResult.Failed)
                return authResult.ErrorType;

            addUser(authResult.Value);
            while (true) // handle messages
            {
                var messageResult = await _messageChanel.RecieveMessage(connection.Input);
                if (messageResult.Failed) // closed client side
                {
                    removeUser(authResult.Value);
                    return messageResult.ErrorType;
                }

                var messagePair = messageResult.Value;
                handleMessage(authResult.Value, messagePair);
            }

        }

        private async Task<IOResult<AuthenticatedConnection>> authenticate(Connection connection)
        {
            var authResult = await _authenticator.RecieveAuthenticationRequest(connection.Input);
            if (authResult.Failed)
                return IOResult.CreateFailure<AuthenticatedConnection>(authResult.ErrorType);

            string password = authResult.Value.Password;
            if (_state.PasswordBag.PasswordExists(password) == false)
                return IOResult.CreateFailure<AuthenticatedConnection>(authResult.ErrorType);

            _state.PasswordBag.RemovePassword(password);

            Guid userId = Guid.NewGuid();
            Guid privateAccessToken = Guid.NewGuid();

            var authSendResult = await _authenticator.SendAuthenticationResponse(connection.Output, userId, privateAccessToken);
            if (authSendResult.Failed)
                return IOResult.CreateFailure<AuthenticatedConnection>(authResult.ErrorType);

            var authConnection = new AuthenticatedConnection(userId, privateAccessToken, connection);

            return IOResult.CreateSuccess(authConnection);
        }

        private void handleMessage(AuthenticatedConnection connection, TypeMessagePair pair)
        {
            if (pair.MessageType == typeof(ModActivationMessage))
                handleModAddMessage(connection, (ModActivationMessage)pair.Message);
            else if (pair.MessageType == typeof(UserUpdateMessage))
                handleUserUpdateMessage(connection, (UserUpdateMessage)pair.Message);
        }

        private void postModListUpdateMessage()
        {
            var message = new ModListUpdateMessage(_state.Mods.Copy());
            var messageTypePair = new TypeMessagePair(typeof(ModListUpdateMessage), message);
            _state.OutboundMessages.Add(messageTypePair);
        }

        private void postUserListUpdateMessage()
        {
            var message = new UserListUpdateMessage(_state.Users.Copy());
            var messageTypePair = new TypeMessagePair(typeof(UserListUpdateMessage), message);
            _state.OutboundMessages.Add(messageTypePair);
        }

        private void handleModAddMessage(AuthenticatedConnection connection, ModActivationMessage message)
        {
            _state.Mods.Add(message.ModInfo);
            postModListUpdateMessage();
        }

        private void handleUserUpdateMessage(AuthenticatedConnection connection, UserUpdateMessage message)
        {
            _state.Users.ForEach((user, index) =>
            {
                if (user.UserId != connection.UserId)
                    return;

                user.Status = message.User.Status;
                user.Username = message.User.Username;
                user.ImagePath = message.User.ImagePath;
            });

            postUserListUpdateMessage();
        }

        private void addUser(AuthenticatedConnection connection)
        {
            _state.Connections.Add(connection);

            UserInfo defaultUser = new UserInfo(null, null, null, connection.UserId);
            _state.Users.Add(defaultUser);
            postUserListUpdateMessage();
        }

        private void removeUser(AuthenticatedConnection removeConnection)
        {
            _state.Connections.ForEach((connection, index) =>// Ok to iterate forwards because only 1 connection removed
            {
                if (removeConnection.UserId == connection.UserId)
                    _state.Connections.RemoveAt(index);
            });

            _state.Users.ForEach((user, index) =>
            {
                if (removeConnection.UserId == user.UserId)
                    _state.Users.RemoveAt(index);
            });

            postUserListUpdateMessage();
        }


    }
}
