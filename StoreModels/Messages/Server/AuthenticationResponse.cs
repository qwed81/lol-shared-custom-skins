using System;
using System.Collections.Generic;
using System.Text;

namespace StoreModels.Messages.Server
{
    public class AuthenticationResponse
    {

        public bool Success { get; set; }

        public Guid SessionId { get; set; }

        public Guid UserId { get; set; }

        public Guid PrivateAccessToken { get; set; }

        public string? FailureReason { get; set; }

        public static AuthenticationResponse CreateFailure(string reason)
        {
            return new AuthenticationResponse
            {
                Success = false,
                FailureReason = reason
            };

        }

        public static AuthenticationResponse CreateSuccess(Guid sessionId, Guid userId, Guid accessToken)
        {
            return new AuthenticationResponse
            {
                Success = true,
                SessionId = sessionId,
                UserId = userId,
                PrivateAccessToken = accessToken
            };
        }

    }
}
