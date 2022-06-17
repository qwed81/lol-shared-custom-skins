using ClientStore.Models;
using StoreModels;
using StoreModels.File;
using System;
using System.Collections.Generic;
using System.Text;

using ClientStore.Network;
using System.Threading.Tasks;
using StoreModels.Messages.Client;
using StoreModels.Messages.Server;
using StoreModels.Messages.Shared;

namespace ClientStore.ModelLogic
{
    internal class RemoteStore : IClientMessageHandler, IDataStore
    {

        private FileIndex _fileIndex;
        private FileTransferer? _fileSender;
        private ClientMessageChanel? _messageChanel;
        private HashSet<FileDescriptor> _uploadedFiles;

        public SynchronizedList<Mod> ModList { get; }

        public SynchronizedList<User> UserList { get; }

        public RemoteStore(FileIndex fileIndex)
        {
            _fileIndex = fileIndex;
            _uploadedFiles = new HashSet<FileDescriptor>();
            ModList = new SynchronizedList<Mod>();
            UserList = new SynchronizedList<User>();
        }

        #region Connection

        public async Task ConnectAsync(ClientConnectionInfo connectionInfo)
        {
            _messageChanel = await ClientMessageChanel.ConnectChanelAsync(connectionInfo, this);
            _fileSender = new FileTransferer(connectionInfo.Host, connectionInfo.Port, _fileIndex);
        }

        public async Task ProcessUntilClosedAsync()
        {
            await _messageChanel!.ProcessUntilClosedAsync();
        }

        #endregion Connection

        #region Sender

        private void sendFileIfNew(FileDescriptor fd, FileType fileType, IProgress<double> progress)
        {
            if (_uploadedFiles.Contains(fd))
                return;

            _fileSender!.RequestSendFile(_messageChanel!.SessionId, _messageChanel!.PrivateAccessToken, fd, fileType, progress);
            _uploadedFiles.Add(fd);
        }

        public void RemoveMod(Mod mod)
        {
            throw new NotImplementedException();    
        }

        private async Task sendUserUpdateMessageAsync(FileDescriptor? fd, string? username, string? status)
        {
            var userInfo = new UserInfo(username, status, fd, _messageChanel!.UserId);
            var message = new UserUpdateMessage(userInfo);

            await _messageChanel!.PostMessageAsync(typeof(UserUpdateMessage), message);
            
            if(fd != null)
                sendFileIfNew(fd, FileType.PROFILE_PICTURE, new Progress<double>());
        }

        public async Task UpdateUserAsync(string? username, string? status, string? pfpFilePath)
        {
            if(pfpFilePath == null)
            {
                await sendUserUpdateMessageAsync(null, username, status);
                return;
            }

            FileDescriptor? fd = _fileIndex.GetFileDescriptorIfExists(pfpFilePath);
            if (fd == null)
            {
                var newFd = await _fileIndex.AddExternalFileToIndexAsync(pfpFilePath, _messageChanel!.SessionId,
                    FileType.PROFILE_PICTURE, (fp) => { });
                await sendUserUpdateMessageAsync(newFd, username, status);
            }
            else
            {
                await sendUserUpdateMessageAsync(fd, username, status);
            } 
        }

        private async Task sendModAddMessageAsync(FileDescriptor fd, string modName, string modDescription,
            IProgress<double>? progress)
        {
            var modInfo = new ModInfo(modName, modDescription, fd, Guid.NewGuid());
            var message = new ModAddMessage(modInfo);

            await _messageChanel!.PostMessageAsync(typeof(ModAddMessage), message);
            sendFileIfNew(fd, FileType.MOD_ZIP, progress ?? new Progress<double>());
        }

        public async Task UploadModAsync(string modName, string modDescription, string modPath, IProgress<double>? progress = null)
        {
            FileDescriptor? fd = _fileIndex.GetFileDescriptorIfExists(modPath);
            if (fd == null)
            {
                var newFd = await _fileIndex.AddExternalFileToIndexAsync(modPath, _messageChanel!.SessionId,
                    FileType.MOD_ZIP, (fp) => { });

                await sendUserUpdateMessageAsync(newFd, modName, modDescription);
            }
            else
            {
                await sendModAddMessageAsync(fd,modName, modDescription, progress);
            }
        }

        #endregion Sender

        #region Handler

        public void HandleMessage(Type messageType, object message)
        {
            if(messageType == typeof(ModListUpdateMessage))
            {
                handleModListUpdate((ModListUpdateMessage)message);
            }
            else if (messageType == typeof(UserListUpdateMessage))
            {
                handleUserListUpdate((UserListUpdateMessage)message);
            }
            else if (messageType == typeof(FileUpdateMessage))
            {
                handleFileUpdate((FileUpdateMessage)message);
            }
            else
            {
                throw new Exception("Unhandled message type");
            }

        }

        private void handleModListUpdate(ModListUpdateMessage message)
        {
            List<Mod> mods = new List<Mod>();
            foreach(var modInfo in message.ModList) // converts from mod info to mod
            {
                var modProgress = _fileIndex.GetFileDownloadProgress(modInfo.ModFile);
                string modPath = _fileIndex.PathSelector.GetPath(_messageChanel!.SessionId, modInfo.ModFile, FileType.MOD_ZIP);
                var mod = new Mod(modInfo.Name, modInfo.Description, modProgress, modPath, modInfo.ModId);

                mods.Add(mod);
            }

            ModList.TakeOwnershipOfCollection(mods);
        }

        private void handleUserListUpdate(UserListUpdateMessage message)
        {
            List<User> users = new List<User>();
            foreach(var userInfo in message.Users)
            {
                IFileProgress? profilePicProgress = null;
                string? filePath = null;
                if(userInfo.ProfilePicture != null)
                {
                    profilePicProgress = _fileIndex.GetFileDownloadProgress(userInfo.ProfilePicture);
                    filePath = _fileIndex.PathSelector.GetPath(_messageChanel!.SessionId, userInfo.ProfilePicture, FileType.PROFILE_PICTURE);
                }
                    
                var user = new User(userInfo.Username, userInfo.Status, profilePicProgress, filePath);
                users.Add(user);
            }

            UserList.TakeOwnershipOfCollection(users);
        }

        private void handleFileUpdate(FileUpdateMessage message)
        {
            _fileSender!.RequestRetrieveFile(_messageChanel!.SessionId, _messageChanel!.PrivateAccessToken, 
                message.FileDescriptor, message.FileType);
        }

        #endregion Handler
    }
}
