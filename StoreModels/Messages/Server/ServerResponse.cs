using StoreModels.Messages.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace StoreModels.Messages.Server
{
    public class ServerResponse
    {

        public bool Success { get; set; }

        public string? FailureReason { get; set; }

        public static ServerResponse CreateFailureResponse(RequestType type, string failureReason)
        {
            ServerResponse res = type switch
            {
                RequestType.CONNECT_MESSAGE_CHANEL => new ConnectMessageChanelResponse(Guid.Empty, Guid.Empty, Guid.Empty),
                RequestType.FILE_GET => new FileGetResponse(-1),
                RequestType.FILE_PUT => new FilePutResponse(),
                _ => throw new Exception("Type not valid")
            };
            res.FailureReason = failureReason;
            res.Success = false;
            return res;
        }

    }
}
