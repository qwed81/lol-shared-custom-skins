using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Network.Messages.Server
{
    public class AuthenticateResponse
    {

        public Guid UserId { get; set; }

        public Guid PrivateAccessToken { get; set; }

        public AuthenticateResponse(Guid userId, Guid accessToken)
        {
            UserId = userId;
            PrivateAccessToken = accessToken;
        }

    }
}
