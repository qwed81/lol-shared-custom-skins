using System;
using System.Collections.Generic;
using System.Text;

namespace StoreModels.Messages.Client
{
    public class AuthenticationRequest : ClientRequest
    {

        public Guid SessionRequestId { get; set; }

        public string Password { get; set; }

        public bool Admin { get; set; }

        public UserInfo InitialUserInfo { get; set; }

        public AuthenticationRequest(Guid sessionRequestId, bool admin, string password, 
            UserInfo initUserInfo) : base(RequestType.MESSAGE_LOOP)
        {
            SessionRequestId = sessionRequestId;
            Admin = admin;
            Password = password;
            InitialUserInfo = initUserInfo;
        }

    }
}
