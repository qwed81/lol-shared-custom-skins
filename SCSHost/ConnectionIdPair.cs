using SharedLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSHost
{
    public struct ConnectionIdPair
    {

        public Connection Connection { get; }

        public Guid UserId { get; }

        public Guid PrivateAccessToken { get; }

        public ConnectionIdPair(Connection connection, Guid userId, Guid privateAccessToken)
        {
            Connection = connection;
            UserId = userId;
            PrivateAccessToken = privateAccessToken;
        }

    }
}
