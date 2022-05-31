using System;
using System.Collections.Generic;
using System.Text;

namespace StoreModels.Messages.Client
{
    public class FileGetRequest : ClientRequest
    {

        public Guid SessionId { get; set; }

        public Guid MessageId { get; set; }

        public FileDescriptor File { get; set; }

        public FileGetRequest(Guid sessionId, Guid messageId, FileDescriptor fileDescriptor) : base(RequestType.FILE_RETRIVE)
        {
            SessionId = sessionId;
            MessageId = messageId;
            File = fileDescriptor;
        }

    }
}
