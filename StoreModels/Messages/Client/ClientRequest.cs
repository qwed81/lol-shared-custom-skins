using System;
using System.Collections.Generic;
using System.Text;

namespace StoreModels.Messages.Client
{
    public enum RequestType
    {
        CONNECT_MESSAGE_CHANEL, FILE_GET, FILE_PUT
    }

    public class ClientRequest
    {

        public RequestType RequestType { get; set; }

        public Guid SessionId { get; set; }

        public ClientRequest(RequestType requestType, Guid sessionId)
        {
            RequestType = requestType;
            SessionId = sessionId;
        }
    
    }
}