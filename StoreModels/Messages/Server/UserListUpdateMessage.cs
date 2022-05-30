using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreModels.Messages.Server
{
    public class UserListUpdateMessage
    {

        public List<UserInfo> Users { get; set; }

        public UserListUpdateMessage(List<UserInfo> users)
        {
            Users = users;
        }

    }

}
