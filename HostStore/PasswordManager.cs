using StoreModels;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace HostStore
{
    internal class PasswordManager
    {

        private ConcurrentDictionary<string, bool> _passwordSet;
        private IPEndPoint _publicEndpoint;
        private IPEndPoint _localEndpoint;
        private Guid _sessionId;


        public PasswordManager(IPEndPoint localEndPoint, IPEndPoint publicEndPoint, Guid sessionId)
        {
            _localEndpoint = localEndPoint;
            _publicEndpoint = publicEndPoint;
            _sessionId = sessionId;

            _passwordSet = new ConcurrentDictionary<string, bool>();
        }

        private string createInvite(IPEndPoint endpoint, bool admin)
        {
            string inviteStr = InviteUtil.CreateInviteString(endpoint, admin);
            var (_, password) = InviteUtil.ParseInviteString(inviteStr);

            _passwordSet[password] = true;

            return inviteStr;
        }

        public string CreateWanInvite(bool admin)
        {
            return createInvite(_publicEndpoint, admin);
        }

        public string CreateLanInvite(bool admin)
        {
            return createInvite(_localEndpoint, admin);
        }

        public bool InviteExists(string password)
        {
            return _passwordSet[password];
        }

        public void ConsumeInvite(string password)
        {
            _passwordSet[password] = false;
        }

    }
}
