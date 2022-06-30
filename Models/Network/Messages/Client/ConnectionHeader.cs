using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Network.Messages.Client
{
    public class ConnectionHeader
    {

        public ConnectionType ConnectionType { get; set; }

        public ConnectionHeader(ConnectionType connectionType)
        {
            ConnectionType = connectionType;
        }

    }
}
