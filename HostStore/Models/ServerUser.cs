using StoreModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace HostStore
{
    public class ServerUser
    {

        public IAuthenticatedUser AuthUser { get; set; }

        public string Username { get; set; }

        public string Status { get; set; }

        public FileDescriptor ProfilePicture { get; set; } 

        public ServerUser(IAuthenticatedUser user, string username, string status, FileDescriptor profilePicture)
        {
            AuthUser = user;
            Username = username;
            Status = status;
            ProfilePicture = profilePicture;
        }

    }
}
