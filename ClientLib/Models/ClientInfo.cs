using System;
using System.Collections.Generic;
using System.Text;

namespace ClientLib.Models
{
    public readonly struct ClientInfo
    {

        public Guid UserId { get; }

        public Guid PrivateAccessToken { get; }

        public ClientInfo(Guid userId, Guid privateAccessToken)
        {
            UserId = userId;
            PrivateAccessToken = privateAccessToken;
        }

    }
}
