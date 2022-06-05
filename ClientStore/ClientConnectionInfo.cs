using StoreModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientStore
{
    public class ClientConnectionInfo
    {

        public string Host { get; set; }

        public int Port { get; set; }

        public Guid SessionRequestId { get; set; }

        public bool Admin { get; set; }

        public string Password { get; set; }

        public string? InitUsername { get; set; }

        public string? InitStatus { get; set; }

        public FileDescriptor? InitProfilePicture { get; set; }

        public ClientConnectionInfo(string host, int port, Guid sessionRequestId, bool admin,
            string password, string? initUsername, string? initStatus, FileDescriptor? initProfilePicture)
        {
            Host = host;
            Port = port;
            SessionRequestId = sessionRequestId;
            Admin = admin;
            Password = password;
            InitUsername = initUsername;
            InitStatus = initStatus;
            InitProfilePicture = initProfilePicture;
        }

    }
}
