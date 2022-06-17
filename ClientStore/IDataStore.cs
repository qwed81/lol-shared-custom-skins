using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ClientStore.Models;
using StoreModels;

namespace ClientStore
{
    public interface IDataStore
    {

        public SynchronizedList<User> UserList { get; }

        public SynchronizedList<Mod> ModList { get; }

        public Task UpdateUserAsync(string username, string status, string pfpFilePath);

        public Task UploadModAsync(string modName, string modDescription, string modPath, IProgress<double>? progress = null);

        public void RemoveMod(Mod mod);

    }
}
