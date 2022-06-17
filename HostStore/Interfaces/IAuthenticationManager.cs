using System;
using System.Collections.Generic;
using System.Text;

namespace HostStore
{
    internal interface IAuthenticationManager
    {

        public bool InviteExists(string password);

        public void ConsumeInvite(string password);

    }
}
