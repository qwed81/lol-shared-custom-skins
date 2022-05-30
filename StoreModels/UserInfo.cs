using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreModels
{
    public class UserInfo
    {

        public string? Name { get; }

        public string? Status { get; }

        public FileDescriptor? Image { get; }

        public Guid UserId { get; }

        public UserInfo(string? name, string? status, FileDescriptor? image, Guid userId)
        {
            Name = name;
            Status = status;
            Image = image;
            UserId = userId;
        }

    }
}
