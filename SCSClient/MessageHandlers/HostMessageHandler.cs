using Models.Network;
using Models.Network.Messages.Server;
using SCSClient.NetworkClient;
using SCSClient.Types;

namespace SCSClient.MessageTranslator
{
    public class HostMessageHandler
    {

        private LocalModLibrary _modLibrary;
        private RemoteConnectionState _connectionState;

        public HostMessageHandler(LocalModLibrary modLibrary, RemoteConnectionState connectionState)
        {
            _modLibrary = modLibrary;
            _connectionState = connectionState;
        }

        public IEnumerable<DataUpdate> Handle(TypeMessagePair pair)
        {
            if (pair.MessageType == typeof(ModListUpdateMessage))
                return modListUpdate((ModListUpdateMessage)pair.Message);
            else if (pair.MessageType == typeof(UserListUpdateMessage))
                return userListUpdate((UserListUpdateMessage)pair.Message);
            else if (pair.MessageType == typeof(FileReadyMessage))
            {
                fileReady((FileReadyMessage)pair.Message);
                return new DataUpdate[0];
            }
               
            
            throw new NotImplementedException();
        }

        private IEnumerable<DataUpdate> modListUpdate(ModListUpdateMessage message)
        {
            bool shouldUpdate = false;
            foreach(var modInfo in message.ModList)
            {
                if (_modLibrary.ContainsMod(modInfo.FileHash))
                    continue;

                shouldUpdate = true;
                _modLibrary.AddMod(modInfo, true);
            }

            if (shouldUpdate == false)
                return new DataUpdate[0];

            List<ReactMod> mods = new List<ReactMod>();
            foreach(var modActivePair in _modLibrary)
            {
                var modInfo = modActivePair.Item1;
                var mod = new ReactMod(modInfo.Description, modInfo.Name, modInfo.Author,
                    modInfo.ProviderName, "", modInfo.FileHash, 100, modActivePair.Item2);

                mods.Add(mod);
            }

            return new DataUpdate[] { new ModListDataUpdate(mods) };
        }

        private IEnumerable<DataUpdate> userListUpdate(UserListUpdateMessage message)
        {
            if (_connectionState.IsConnected == false)
                return new DataUpdate[0];

            List<UserInfo> copy = message.Users.ToList();
            var user = copy.Where(user => user.UserId == _connectionState.UserId).FirstOrDefault();
            if(user == null)
                return new DataUpdate[0];

            copy.Remove(user);

            var reactUser = new ReactPerson(user.Username, user.Status, user.ImagePath, user.UserId.ToString());
            var userUpdate = new UserDataUpdate(reactUser);

            var reactMembers = new List<ReactPerson>();
            foreach(var member in copy)
            {
                var reactMember = new ReactPerson(user.Username, user.Status, user.ImagePath, user.UserId.ToString());
                reactMembers.Add(reactMember);
            }

            var membersUpdate = new PartyMembersDataUpdate(reactMembers);
            return new DataUpdate[] { userUpdate, membersUpdate };
        }

        private void fileReady(FileReadyMessage message)
        {
            Console.WriteLine("file ready");
        }


    }
}
