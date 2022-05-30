using System;
using System.Collections.Generic;
using System.Text;

namespace StoreModels.Messages.Server
{
    public class ServerAuthenticationResponse
    {

        public bool Success { get; set; }

        public Guid SessionId { get; set; }

        public Guid UserId { get; set; }

        public string? FailureReason { get; set; }

        public static ServerAuthenticationResponse CreateFailure(string reason)
        {
            return new ServerAuthenticationResponse
            {
                Success = false,
                FailureReason = reason
            };

        }

        public static ServerAuthenticationResponse CreateSuccess(Guid sessionId, Guid userId)
        {
            return new ServerAuthenticationResponse
            {
                Success = true,
                SessionId = sessionId,
                UserId = userId
            };
        }

    }
}
