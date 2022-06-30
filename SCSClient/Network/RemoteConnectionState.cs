using ClientLib.Models;
using SharedLib;
using System.Net.Sockets;

namespace SCSClient.NetworkClient
{

    public class RemoteConnectionState
    {
        public Guid UserId { get; private set; }

        public Guid PrivateAccessToken { get; private set; }

        public bool IsConnected { get; private set; }

        public Connection Connection { get; private set; }

        public RemoteConnectionState()
        {
            UserId = Guid.Empty;
            PrivateAccessToken = Guid.Empty;
            IsConnected = false;
            Connection = null!;
        }

        public void ConnectComplete(Connection connection, Guid userId, Guid privateAccessToken)
        {
            IsConnected = true;
            Connection = connection;
            PrivateAccessToken = privateAccessToken;
        }

        public void Disconnect()
        {
            Connection.Dispose();

            UserId = Guid.Empty;
            PrivateAccessToken = Guid.Empty;
            IsConnected = false;
            Connection = null!;
        }

        

    }

}
