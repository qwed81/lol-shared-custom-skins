using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Network.Messages.Client
{
    public class ConnectionHeader
    {

        public Guid SessionId { get; set; }

        public ConnectionType ConnectionType { get; set; }

        public ConnectionHeader(Guid sessionId, ConnectionType connectionType)
        {
            SessionId = sessionId;
            ConnectionType = connectionType;
        }

    }
}
