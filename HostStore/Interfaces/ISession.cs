using StoreModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HostStore
{

    public interface ISession
    {

        public SynchronizedList<ServerUser> Users { get; }

        public SynchronizedList<ModInfo> Mods { get; }

        public Guid SessionId { get; }

    }
}
