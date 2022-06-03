using System;
using System.Collections.Generic;
using System.Text;

namespace StoreModels.Messages.Server
{
    public class ConnectMessageChanelResponse : ServerResponse
    {

        public Guid SessionId { get; set; }

        public Guid UserId { get; set; }

        public Guid PrivateAccessToken { get; set; }

        public ConnectMessageChanelResponse(Guid sessionId, Guid userId, Guid accessToken)
        {
            Success = true;
            SessionId = sessionId;
            UserId = userId;
            PrivateAccessToken = accessToken;
        }

    }
}
