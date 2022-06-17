using SharedLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace HostLib.Models
{
    public struct SessionConnectionPair
    {
        public Guid SessionId { get; set; }

        public Connection Connection { get; set; }

        public SessionConnectionPair(Guid sessionId, Connection connection)
        {
            SessionId = sessionId;
            Connection = connection;
        }

    }
}
