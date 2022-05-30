using System;
using System.Collections.Generic;
using System.Text;

namespace StoreModels.Messages.Client
{
    public class FileMetadata
    {

        public Guid SessionId { get; set; }

        public FileDescriptor FileDescriptor { get; set; }

        public FileType FileType { get; set; }

        public ulong FileLength { get; set; }

        public FileMetadata(Guid sessionId, FileDescriptor fd, FileType fileType, ulong fileLength)
        {
            SessionId = sessionId;
            FileDescriptor = fd;
            FileType = fileType;
            FileLength = fileLength;
        }

    }
}
