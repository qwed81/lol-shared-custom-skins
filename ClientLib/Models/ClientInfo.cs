using System;
using System.Collections.Generic;
using System.Text;

namespace ClientLib.Models
{
    public readonly struct ClientInfo
    {

        public Guid SessionId { get; }

        public Guid UserId { get; }

        public Guid PrivateAccessToken { get; }

        public ClientInfo(Guid sessionId, Guid userId, Guid privateAccessToken)
        {
            SessionId = sessionId;
            UserId = userId;
            PrivateAccessToken = privateAccessToken;
        }

    }
}
