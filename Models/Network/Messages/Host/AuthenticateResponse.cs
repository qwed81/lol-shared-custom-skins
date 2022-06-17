using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Network.Messages.Server
{
    public class AuthenticateResponse
    {

        public Guid SessionId { get; set; }

        public Guid UserId { get; set; }

        public Guid PrivateAccessToken { get; set; }

        public AuthenticateResponse(Guid sessionId, Guid userId, Guid accessToken)
        {
            SessionId = sessionId;
            UserId = userId;
            PrivateAccessToken = accessToken;
        }

    }
}
