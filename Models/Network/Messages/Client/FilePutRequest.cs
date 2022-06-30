using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Network.Messages.Client
{
    public class FilePutRequest
    {

        public Guid PrivateAccessToken { get; set; }

        public string FileHash { get; set; }

        public long FileLength { get; set; }

        public FilePutRequest(Guid privateAccessToken, string fileHash, long fileLength)
        {
            PrivateAccessToken = privateAccessToken;
            FileHash = fileHash;
            FileLength = fileLength;
        }

    }
}
