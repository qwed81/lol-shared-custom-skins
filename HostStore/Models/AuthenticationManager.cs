using StoreModels;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace HostStore
{
    internal class AuthenticationManager : IAuthenticationManager
    {

        private ConcurrentDictionary<string, bool> _passwordSet;
        private Guid _sessionId;


        public AuthenticationManager(Guid sessionId)
        {
            _sessionId = sessionId;

            _passwordSet = new ConcurrentDictionary<string, bool>();
        }

        public InviteInfo CreateInvite(bool admin)
        {
            return InviteInfo.CreateInvite(_sessionId);
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
