using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Network.Messages.Client
{
    public class UserUpdateMessage
    {

        public UserInfo User { get; set; }

        public UserUpdateMessage(UserInfo user)
        {
            User = user;
        }

    }
}
