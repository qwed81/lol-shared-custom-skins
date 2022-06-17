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

    public class Client
    {

        private FileIndex _fileIndex;

        public Client(FileIndex fileIndex)
        {
            _fileIndex = fileIndex;
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
