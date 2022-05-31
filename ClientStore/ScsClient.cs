using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;

using ClientStore.Network;
using StoreModels;
using StoreModels.File;
using StoreModels.Messages.Client;
using StoreModels.Messages.Server;
using StoreModels.Messages.Shared;
using System.Threading.Tasks;

namespace ClientStore
{

    public delegate void ClientDisconnectedHandler(ScsClient sender, string reason);

    internal class NetworkGroup
    {
        public ClientMessageLoop MessageLoop { get; }
        public FileRetriever FileRetriever { get; }
        public FileSender FileSender { get; }

        public NetworkGroup(ClientMessageLoop messageLoop, FileRetriever fileRetriever, FileSender fileSender)
        {
            MessageLoop = messageLoop;
            FileRetriever = fileRetriever;
            FileSender = fileSender;
        }
    
    }

    public class ScsClient
    {

        private FileIndex _fileIndex;
        private NetworkGroup? _networkGroup;
        private List<UserInfo> _currentUsers;
        private List<ModInfo> _currentMods;

        public UserInfo CurrentUser { get; private set; }
        public UserInfo SyncedUser { get; private set; }
        public IReadOnlyList<UserInfo> OtherUsers => _currentUsers;
        public IReadOnlyList<ModInfo> Mods => _currentMods;

        public event Action<ScsClient>? UsersChanged;
        public event Action<ScsClient>? ModsChanged;
        public event Action<ScsClient>? SyncedUserChanged;
        public event ClientDisconnectedHandler? PartyDisconnected;

        public bool Connected => _networkGroup != null;

        public ScsClient(FileIndex fileIndex)
        {
            _currentUsers = new List<UserInfo>();
            _currentMods = new List<ModInfo>();
            _fileIndex = fileIndex;

            CurrentUser = new UserInfo(null, null, null, default);
            SyncedUser = new UserInfo(null, null, null, default);
        }

        public async Task<bool> ConnectToPartyAsync(string host, int port, Guid sessionRequestId, bool admin, 
            string password, UserInfo initUserInfo)
        {
            if (Connected)
                throw new Exception("Already connected to a party");

            _currentUsers = new List<UserInfo>();
            _currentMods = new List<ModInfo>();

            CurrentUser = initUserInfo;
            SyncedUser = new UserInfo(null, null, null, default);

            var fileRetriever = new FileRetriever(host, port, _fileIndex);
            var fileSender = new FileSender(host, port, _fileIndex);

            try
            {
                var messageLoop = await ClientMessageLoop.ConnectLoopAsync(host, port, sessionRequestId,
                    admin, password, initUserInfo);
                messageLoop.OnMessage += handleMessage;

                _networkGroup = new NetworkGroup(messageLoop, fileRetriever, fileSender);

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }



        #region Sender

        public async Task AddModAsync(ModInfo mod)
        {
            if (_networkGroup == null)
                throw new Exception("Can not add mod while not connected");

            var message = new ModAddMessage(mod);
            await _networkGroup.MessageLoop.PostMessageAsync(typeof(ModAddMessage), message);
            sendFile(mod.ModFile, FileType.MOD_ZIP);
        }

        public async Task SetUserInfoAsync(UserInfo info)
        {
            if (_networkGroup == null)
                throw new Exception("Can not add user while not connected");

            if (CurrentUser.Image != info.Image && info.Image != null)
            {
                sendFile(info.Image, FileType.PROFILE_PICTURE);
            }

            CurrentUser = info;
            var message = new UserUpdateMessage(info);
            await _networkGroup.MessageLoop.PostMessageAsync(typeof(UserUpdateMessage), message);

        }

        public async Task SetStatusAsync(string status)
        {
            UserInfo newInfo = new UserInfo(CurrentUser.Name, status, CurrentUser.Image, CurrentUser.UserId);
            await SetUserInfoAsync(newInfo);
        }

        private void sendFile(FileDescriptor fd, FileType fileType)
        {
            if (_networkGroup == null) // dont send a file if already disconnected
                return; 

            // copy on same thread so not disconnected after null check, before execution
            var fsCopy = _networkGroup.FileSender;
            Guid sessionId = _networkGroup.MessageLoop.SessionId;
            Guid userId = _networkGroup.MessageLoop.UserId;

            _ = Task.Run(async () =>
            {
                await fsCopy.SendFileAsync(sessionId, userId, fd, fileType);
            });
        }

        #endregion Sender

        #region Handler

        private void handleMessage(ClientMessageLoop loop, Type messageType, object message)
        {
            if (messageType == typeof(ModListUpdateMessage))
            {
                updateMods(((ModListUpdateMessage)message).ModList);
            }
            else if (messageType == typeof(UserListUpdateMessage))
            {
                updateUsers(((UserListUpdateMessage)message).Users);
            }
            else if (messageType == typeof(FileUpdateMessage))
            {
                var msg = (FileUpdateMessage)message;
                updateFile(msg.FileDescriptor, msg.FileType);
            }
            else
            {
                throw new Exception("Unhandled message type");
            }

        }

        private void updateMods(List<ModInfo> mods)
        {
            _currentMods = mods;
            ModsChanged?.Invoke(this);
        }

        private void updateUsers(List<UserInfo> partyMembers)
        {
            UserInfo? newUser = null;
            for(int i = partyMembers.Count - 1; i > -1; i--)
            {
                if(partyMembers[i].UserId == _networkGroup!.MessageLoop.UserId)
                {
                    newUser = partyMembers[i];
                    partyMembers.RemoveAt(i);
                }
            }

            _currentUsers = partyMembers;

            if(newUser != null)
            {
                SyncedUser = newUser;
                if(newUser.Status != SyncedUser.Status || newUser.Name != SyncedUser.Name ||
                    newUser.Image != SyncedUser.Image)
                {
                    SyncedUserChanged?.Invoke(this);
                }
            }

            UsersChanged?.Invoke(this);
        }

        private void updateFile(FileDescriptor fd, FileType fileType)
        {
            if (_networkGroup == null) // dont send a file if already disconnected
                return;

            // copy on same thread so not disconnected after null check, before execution
            var frCopy = _networkGroup.FileRetriever;
            Guid sessionId = _networkGroup.MessageLoop.SessionId;
            Guid userId = _networkGroup.MessageLoop.UserId;

            _ = Task.Run(async () =>
            {
                await frCopy.RetrieveFileIfAbsentAsync(sessionId, userId, fd, fileType);
            });
        }

        #endregion Handler
    }


}
