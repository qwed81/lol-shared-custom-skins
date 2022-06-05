using ClientStore.ModelLogic;
using ClientStore.Network;
using StoreModels;
using StoreModels.File;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ClientStore
{
    public class ClientInitOptions
    {

        public string RootPath { get; set; } = "c://scs";

        public string ModPath { get; set; } = "/mods";

        public bool DeleteModsAfterClose { get; set; } = false;

        public string ProfilePicturePath { get; set; } = "/pfps";

        public bool DeleteProfilePicturesAfterClose { get; set; } = true;

    }

    public class Client
    {

        private FileIndex _fileIndex;

        public static Client Init(Action<ClientInitOptions> options)
        {
            ClientInitOptions config = new ClientInitOptions();
            options(config);

            var pathNames = new Dictionary<FileType, string>()
            {
                [FileType.MOD_ZIP] = config.ModPath,
                [FileType.PROFILE_PICTURE] = config.ProfilePicturePath
            };

            var sessionSpecific = new Dictionary<FileType, bool>()
            {
                [FileType.MOD_ZIP] = config.DeleteModsAfterClose,
                [FileType.PROFILE_PICTURE] = config.DeleteProfilePicturesAfterClose
            };

            PathSelector pathSelector = new PathSelector(config.RootPath, pathNames, sessionSpecific);
            return new Client(pathSelector);
        }

        private Client(PathSelector pathSelector)
        {
            _fileIndex = new FileIndex(pathSelector);
        }

        public async Task ProcessConnectionUntilClosedAsync(ClientConnectionInfo info, Action<IDataStore> onConnection)
        {
            RemoteStore remoteStore = new RemoteStore(_fileIndex);
            await remoteStore.ConnectAsync(info);
            onConnection(remoteStore);

            await remoteStore.ProcessUntilClosedAsync();
        } 


    }
}
