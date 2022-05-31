using System;
using System.Collections.Generic;
using System.Text;

namespace StoreModels.Messages.Client
{
    public class FilePutRequest : ClientRequest
    {

        public Guid SessionId { get; set; }

        public FileDescriptor FileDescriptor { get; set; }

        public FileType FileType { get; set; }

        public ulong FileLength { get; set; }

        public FilePutRequest(Guid sessionId, FileDescriptor fd, FileType fileType, 
            ulong fileLength) : base(RequestType.FILE_SEND)
        {
            SessionId = sessionId;
            FileDescriptor = fd;
            FileType = fileType;
            FileLength = fileLength;
        }

    }
}
