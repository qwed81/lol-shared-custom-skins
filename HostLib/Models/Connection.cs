using Models.Network;
using SharedLib;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace HostLib.Models
{
    public class Connection : IDisposable
    {

        public ConnectionType ConnectionType { get; set; }

        public TcpClient Client { get; set; }

        public AugmentedInputStream Input { get; set; }

        public AugmentedOutputStream Output { get; set; }

        public Connection(ConnectionType connectionType, TcpClient client, AugmentedInputStream input,
            AugmentedOutputStream output)
        {
            ConnectionType = connectionType;
            Client = client;
            Input = input;
            Output = output;
        }

        public void Dispose()
        {
            Client.Close();
        }
    }
}
