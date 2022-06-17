using HostLib;
using Models.Network;
using SharedLib;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace SCSHost
{
    public class Session
    {

        public List<ConnectionIdPair> Connections { get; }

        public PasswordBag PasswordBag { get; }

        public List<ModInfo> Mods { get; }

        public List<UserInfo> Users { get; }

        public object SessionLock { get; } // so changes from different threads don't collide

        public Session(List<ConnectionIdPair> connections, PasswordBag passwordBag, 
            List<ModInfo> mods, List<UserInfo> users)
        {
            Connections = connections;
            PasswordBag = passwordBag;
            Mods = mods;
            Users = users;
            SessionLock = new object();
        }


    }

}
