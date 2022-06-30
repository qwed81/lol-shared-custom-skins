using Models.Network;
using SharedLib;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace HostLib.Models
{
    public class AuthenticatedConnection
    {

        public Connection Connection { get; set; }

        public Guid UserId { get; set; }

        public Guid PrivateAccessToken { get; set; }

        public AuthenticatedConnection(Guid userId, Guid privateAccessToken, Connection connection)
        {
            Connection = connection;
            UserId = userId;
            PrivateAccessToken = privateAccessToken;
        }
    }
}
