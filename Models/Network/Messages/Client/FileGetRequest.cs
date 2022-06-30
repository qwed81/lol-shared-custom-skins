using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Network.Messages.Client
{
    public class FileGetRequest
    {

        public Guid PrivateAccessToken { get; set; }

        public string FileHash { get; set; }

        public FileGetRequest(Guid privateAccessToken, string fileHash)
        {
            PrivateAccessToken = privateAccessToken;
            FileHash = fileHash;
        }

    }
}
