using HostLib;
using HostLib.Models;
using Models.Network;
using SharedLib;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace SCSHost
{
    public class HostState
    {

        public LockedList<AuthenticatedConnection> Connections { get; }

        public PasswordBag PasswordBag { get; }

        public LockedList<ModInfo> Mods { get; }

        public LockedList<UserInfo> Users { get; }

        public ConcurrentDictionary<string, bool> CompletedFiles { get; }

        public BlockingCollection<TypeMessagePair> OutboundMessages { get; }

        public HostState()
        {
            Connections = new LockedList<AuthenticatedConnection>();
            PasswordBag = new PasswordBag();
            Mods = new LockedList<ModInfo>();
            Users = new LockedList<UserInfo>();
            CompletedFiles = new ConcurrentDictionary<string, bool>();

            OutboundMessages = new BlockingCollection<TypeMessagePair>();
        }


        public AuthenticatedConnection? GetExistingMessageChanel(Guid privateAccessToken)
        {
            AuthenticatedConnection? messageChanelConnection = null;
            Connections.ForEach((connection, index) =>
            {
                if (connection.PrivateAccessToken == privateAccessToken)
                    messageChanelConnection = connection;
            });

            return messageChanelConnection;
        }

        public bool ModExists(string fileHash)
        {
            bool exists = false;
            Mods.ForEach((mod, index) =>
            {
                if (mod.FileHash == fileHash)
                    exists = true;
            });

            return exists;
        }

    }

}
