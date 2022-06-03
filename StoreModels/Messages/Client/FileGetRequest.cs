using System;
using System.Collections.Generic;
using System.Text;

namespace StoreModels.Messages.Client
{
    public class FileGetRequest : ClientRequest
    {

        public Guid PrivateAccessToken { get; set; }

        public FileDescriptor File { get; set; }

        public FileType FileType { get; set; }

        public FileGetRequest(Guid sessionId, Guid privateAccessToken, FileDescriptor fileDescriptor, 
            FileType fileType) : base(RequestType.FILE_GET, sessionId)
        {
            PrivateAccessToken = privateAccessToken;
            File = fileDescriptor;
            FileType = fileType;
        }

    }
}
