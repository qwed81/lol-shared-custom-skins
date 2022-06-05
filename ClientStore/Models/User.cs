using StoreModels;
using StoreModels.File;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientStore.Models
{
    public class User
    {

        public string? Username { get;}

        public string? Status { get; }

        public IFileProgress? ProfilePictureProgress { get; }

        public string? ProfilePicturePath { get; }

        internal User(string? username, string? status, IFileProgress? profilePictureProgress, string? profilePath)
        {
            Username = username;
            Status = status;
            ProfilePictureProgress = profilePictureProgress;
            ProfilePicturePath = profilePath;
        }

    }
}
