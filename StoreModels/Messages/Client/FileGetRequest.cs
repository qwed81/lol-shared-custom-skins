using System;
using System.Collections.Generic;
using System.Text;

namespace StoreModels.Messages.Client
{
    public class FileGetRequest : ClientRequest
    {

        public Guid SessionId { get; set; }

        public Guid PrivateAccessToken { get; set; }

        public FileDescriptor File { get; set; }

        public FileGetRequest(Guid sessionId, Guid privateAccessToken,
            FileDescriptor fileDescriptor) : base(RequestType.FILE_RETRIVE)
        {
            SessionId = sessionId;
            PrivateAccessToken = privateAccessToken;
            File = fileDescriptor;
        }

    }
}
