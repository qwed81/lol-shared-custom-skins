using System;
using System.Collections.Generic;
using System.Text;

namespace StoreModels.Messages.Client
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
