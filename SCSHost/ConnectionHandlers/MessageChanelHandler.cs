using HostLib;
using HostLib.Models;
using Models.Network;
using Models.Network.Messages.Client;
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

        private ConcurrentDictionary<Guid, Session> _sessions;
        private HostAuthenticator _authenticator;
        private HostMessageChanel _messageChanel;

        public MessageChanelHandler(ConcurrentDictionary<Guid, Session> sessions, 
            HostAuthenticator authenticator, HostMessageChanel messageChanel)
        {
            _sessions = sessions;
            _authenticator = authenticator;
            _messageChanel = messageChanel;
        }

        public async Task<IOErrorType> HandleConnection(Guid sessionId, Connection connection)
        {
            // authenticate
            var authResult = await _authenticator.RecieveAuthenticationRequest(connection.Input);
            if (authResult.Failed)
                return authResult.ErrorType;

            string password = authResult.Value.Password;

            if (_sessions[sessionId].PasswordBag.PasswordExists(password) == false)
                return IOErrorType.NotAuthenticated;

            _sessions[sessionId].PasswordBag.RemovePassword(password);

            Guid userId = Guid.NewGuid();
            Guid privateAccessToken = Guid.NewGuid();
            ConnectionIdPair connectionPair = new ConnectionIdPair(connection, userId, privateAccessToken);

            var authSendResult = await _authenticator.SendAuthenticationResponse(connection.Output, sessionId, userId, privateAccessToken);
            if (authSendResult.Failed)
                return authSendResult.ErrorType;

            addToSession(sessionId, connectionPair);

            while (true) // handle messages
            {
                var messageResult = await _messageChanel.RecieveMessage(connection.Input);
                if (messageResult.Failed) // closed client side
                {
                    removeFromSession(sessionId, connectionPair);
                    return messageResult.ErrorType;
                }

                var messagePair = messageResult.Value;
                handleMessage(sessionId, connectionPair, messagePair.MessageType, messagePair.Message);
            }

        }

        private IEnumerable<AugmentedOutputStream> getOutputs(Guid sessionId)
        {
            List<AugmentedOutputStream> streams = new List<AugmentedOutputStream>();
            foreach (var connection in _sessions[sessionId].Connections)
                streams.Add(connection.Connection.Output);
            return streams;
        }

        private void handleMessage(Guid sessionId, ConnectionIdPair connection, Type messageType, object message)
        {
            if (messageType == typeof(ModAddMessage))
                handleModAddMessage(sessionId, (ModAddMessage)message);
            else if (messageType == typeof(UserUpdateMessage))
                handleUserUpdateMessage(sessionId, connection, (UserUpdateMessage)message);
        }

        private void handleModAddMessage(Guid sessionId, ModAddMessage message)
        {

            lock (_sessions[sessionId].SessionLock)
            {
                _sessions[sessionId].Mods.Add(message.ModInfo);

                var outputs = getOutputs(sessionId);
                var modList = _sessions[sessionId].Mods.ToList(); // in lock in case of change

                _messageChanel.SendModListUpdateToAll(outputs, modList).Wait();
            }
        }

        private void handleUserUpdateMessage(Guid sessionId, ConnectionIdPair connection, UserUpdateMessage message)
        {
            lock(_sessions[sessionId].SessionLock)
            {
                UserInfo? user = _sessions[sessionId].Users.Where(user => user.UserId == connection.UserId).FirstOrDefault();

                if (user == null) // user no longer in the list
                    return;

                user.Status = message.User.Status;
                user.Username = message.User.Username;
                user.ProfilePicture = message.User.ProfilePicture;

                _messageChanel.SendUserListUpdateToAll(getOutputs(sessionId), _sessions[sessionId].Users);
            }
        }

        private void addToSession(Guid sessionId, ConnectionIdPair connection)
        {
            lock(_sessions[sessionId].SessionLock)
            {
                _sessions[sessionId].Connections.Add(connection);

                UserInfo defaultUser = new UserInfo(null, null, null, connection.UserId);
                _sessions[sessionId].Users.Add(defaultUser);

                _messageChanel.SendUserListUpdateToAll(getOutputs(sessionId), _sessions[sessionId].Users);
            }
        }

        private void removeFromSession(Guid sessionId, ConnectionIdPair connection)
        {
            lock (_sessions[sessionId].SessionLock)
            {
                _sessions[sessionId].Connections.Remove(connection);

                UserInfo user = _sessions[sessionId].Users.Where(user => user.UserId == connection.UserId).First();
                _sessions[sessionId].Users.Remove(user);

                _messageChanel.SendUserListUpdateToAll(getOutputs(sessionId), _sessions[sessionId].Users);
            }
        }


    }
}
