﻿using System;
using System.Collections.Generic;
using System.Text;

namespace StoreModels.Messages.Client
{
    public class ConnectMessageChanelRequest : ClientRequest
    {

        public string Password { get; set; }

        public bool Admin { get; set; }

        public UserInfo InitialUserInfo { get; set; }

        public ConnectMessageChanelRequest(Guid sessionRequestId, bool admin, string password, 
            UserInfo initUserInfo) : base(RequestType.CONNECT_MESSAGE_CHANEL, sessionRequestId)
        {
            Admin = admin;
            Password = password;
            InitialUserInfo = initUserInfo;
        }

    }
}