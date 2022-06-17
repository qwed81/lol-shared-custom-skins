using StoreModels;
using StoreModels.Messages.Client;
using StoreModels.Messages.Server;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace HostStore
{
    internal class Session : IMessageHandler, ISession, ISessionAuth, IFileCompletionHandler
    {

        private IAuthenticationManager _authManager;
        private MessageChanelGroup _chanelGroup;

        public SynchronizedList<ServerUser> Users { get; }

        public SynchronizedList<ModInfo> Mods { get; }

        public Guid SessionId { get; }

        public bool CanUserReadFiles(Guid userPrivateKey) => _chanelGroup.UserIsAuthenticated(userPrivateKey);

        public bool CanUserWriteFiles(Guid userPrivateKey) => _chanelGroup.UserIsAuthenticated(userPrivateKey);

        public Session(Guid sessionId, IAuthenticationManager authManager)
        {
            SessionId = sessionId;
            _authManager = authManager;
            _chanelGroup = new MessageChanelGroup(SessionId);

            Users = new SynchronizedList<ServerUser>();
            Mods = new SynchronizedList<ModInfo>();
        }

        public async Task HandleChanelConnectionRequestAsync(TcpClientWrapper client, ConnectMessageChanelRequest req)
        {
            await _chanelGroup.HandleChanelConnectionRequestAsync(client, _authManager, this, req);
        }

        public void Close()
        {
            _chanelGroup.Close();
        }

        private void sendUserList()
        {
            List<UserInfo> userInfos = new List<UserInfo>();
            foreach(ServerUser user in Users)
            {
                var userInfo = new UserInfo(user.Username, user.Status, user.ProfilePicture, user.AuthUser.UserId);
                userInfos.Add(userInfo);
            }

            var message = new UserListUpdateMessage(userInfos);
            _chanelGroup.PostMessageToAll(typeof(UserListUpdateMessage), message);
        }

        private void sendModList()
        {
            List<ModInfo> modInfos = Mods.ToList();

            var message = new ModListUpdateMessage(modInfos);
            _chanelGroup.PostMessageToAll(typeof(ModListUpdateMessage), message);
        }

        public void HandleMessage(IAuthenticatedUser user, Type messageType, object message)
        {
            if(messageType == typeof(ModAddMessage))
            {
                handleModAddMessage((ModAddMessage)message);
            }
            else if (messageType == typeof(UserUpdateMessage))
            {
                handleUserUpdateMessage(user.UserId, (UserUpdateMessage)message);
            }
        }

        private void handleModAddMessage(ModAddMessage message)
        {
            Mods.AddElement(message.ModInfo);
            sendModList();
        }

        private void handleUserUpdateMessage(Guid userId, UserUpdateMessage message)
        {
            ServerUser? userUpdated = Users.Where((serverUser) => serverUser.AuthUser.UserId == userId).FirstOrDefault();
            if(userUpdated != null)
            {
                var userInfo = message.User;
                if (userInfo.Status != null)
                    userUpdated.Status = userInfo.Status;
                if (userInfo.Username != null)
                    userUpdated.Username = userInfo.Username;
                if (userInfo.ProfilePicture != null)
                    userUpdated.ProfilePicture = userInfo.ProfilePicture;


            }


            

        }

        public void HandleUserAdded(IAuthenticatedUser user)
        {

        }

        public void HandleUserRemoved(IAuthenticatedUser user)
        {

        }

        public void HandleFileCompleted(FileDescriptor fd)
        {
            throw new NotImplementedException();
        }
    }
}
