using System;
using System.Collections.Generic;
using System.Text;

namespace HostLib.Models
{
    public struct AuthRequestData
    {

        public string Password { get; set; }

        public bool Admin { get; set; }

        public AuthRequestData(string password, bool admin)
        {
            Password = password;
            Admin = admin;
        }

    }
}
