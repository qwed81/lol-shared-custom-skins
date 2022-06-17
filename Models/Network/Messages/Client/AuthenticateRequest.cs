using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Network.Messages.Client
{
    public class AuthenticateRequest
    {

        public string Password { get; set; }

        public bool Admin { get; set; }

        public AuthenticateRequest(bool admin, string password)
        {
            Admin = admin;
            Password = password;
        }

    }
}
