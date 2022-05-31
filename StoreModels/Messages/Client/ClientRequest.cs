using System;
using System.Collections.Generic;
using System.Text;

namespace StoreModels.Messages.Client
{
    public enum RequestType
    {
        MESSAGE_LOOP, FILE_RETRIVE, FILE_SEND
    }

    public class ClientRequest
    {

        public RequestType RequestType { get; set; }

        public ClientRequest(RequestType requestType)
        {
            RequestType = requestType;
        }
    
    }
}