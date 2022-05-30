using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyStore.NetworkModels
{
    internal class UserListUpdateMessage
    {
        public static string MessageType = "USERS";

        public List<UserInfo> Users { get; set; }

        public UserListUpdateMessage(List<UserInfo> users)
        {
            Users = users;
        }

    }

}
