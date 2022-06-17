using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreModels
{
    public class UserInfo
    {

        public string? Username { get; }

        public string? Status { get; }

        public FileDescriptor? ProfilePicture { get; }

        public Guid UserId { get; }

        public UserInfo(string? name, string? status, FileDescriptor? image, Guid userId)
        {
            Username = name;
            Status = status;
            ProfilePicture = image;
            UserId = userId;
        }

    }
}
