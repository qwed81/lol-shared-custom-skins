using Models.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Network
{
    public class UserInfo
    {

        public string? Username { get; set; }

        public string? Status { get; set; }

        public FileDescriptor? ProfilePicture { get; set; }

        public Guid UserId { get; set; }

        public UserInfo(string? name, string? status, FileDescriptor? image, Guid userId)
        {
            Username = name;
            Status = status;
            ProfilePicture = image;
            UserId = userId;
        }

    }
}
