using Models.File;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Network.Messages.Client
{
    public class FilePutRequest
    {

        public Guid PrivateAccessToken { get; set; }

        public FileDescriptor FileDescriptor { get; set; }

        public FileType FileType { get; set; }

        public long FileLength { get; set; }

        public FilePutRequest(Guid privateAccessToken, FileDescriptor fd, FileType fileType, long fileLength)
        {
            PrivateAccessToken = privateAccessToken;
            FileDescriptor = fd;
            FileType = fileType;
            FileLength = fileLength;
        }

    }
}
