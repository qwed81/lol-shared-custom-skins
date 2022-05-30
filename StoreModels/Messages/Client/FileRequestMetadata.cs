using System;
using System.Collections.Generic;
using System.Text;

namespace StoreModels.Messages.Client
{
    public class FileRequestMetadata
    {

        public Guid SessionId { get; set; }

        public Guid MessageId { get; set; }

        public FileDescriptor File { get; set; }

        public FileRequestMetadata(Guid sessionId, Guid messageId, FileDescriptor fileDescriptor)
        {
            SessionId = sessionId;
            MessageId = messageId;
            File = fileDescriptor;
        }

    }
}
