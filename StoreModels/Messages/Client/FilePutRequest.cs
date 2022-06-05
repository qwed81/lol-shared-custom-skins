using System;
using System.Collections.Generic;
using System.Text;

namespace StoreModels.Messages.Client
{
    public class FilePutRequest : ClientRequest
    {

        public Guid PrivateAccessToken { get; set; }

        public FileDescriptor FileDescriptor { get; set; }

        public FileType FileType { get; set; }

        public long FileLength { get; set; }

        public FilePutRequest(Guid sessionId, Guid privateAccessToken, FileDescriptor fd, FileType fileType, 
            long fileLength) : base(RequestType.FILE_PUT, sessionId)
        {
            PrivateAccessToken = privateAccessToken;
            FileDescriptor = fd;
            FileType = fileType;
            FileLength = fileLength;
        }

    }
}
