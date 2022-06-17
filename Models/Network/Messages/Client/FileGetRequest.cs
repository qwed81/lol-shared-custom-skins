using Models.File;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Network.Messages.Client
{
    public class FileGetRequest
    {

        public Guid PrivateAccessToken { get; set; }

        public FileDescriptor File { get; set; }

        public FileType FileType { get; set; }

        public FileGetRequest(Guid privateAccessToken, FileDescriptor fileDescriptor, FileType fileType)
        {
            PrivateAccessToken = privateAccessToken;
            File = fileDescriptor;
            FileType = fileType;
        }

    }
}
