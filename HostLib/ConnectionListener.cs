using HostLib.Models;
using Models.Network.Messages.Client;
using SharedLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HostLib
{
    public class ConnectionListener
    {

        public async Task<IOResult<Connection>> ListenForConnection(TcpListener listener)
        {
            TcpClient client;
            try
            {
                client = await listener.AcceptTcpClientAsync();
            }
            catch (IOException)
            {
                return IOResult.CreateFailure<Connection>(IOErrorType.IOError);
            }

            var input = new AugmentedInputStream(client.GetStream());
            var connectionHeaderResult = await input.ReadObjectAsync<ConnectionHeader>();
            if (connectionHeaderResult.Failed)
                return IOResult.CreateFailure<Connection>(connectionHeaderResult.ErrorType);

            var connectionType = connectionHeaderResult.Value.ConnectionType;
            var output = new AugmentedOutputStream(client.GetStream());

            var connection = new Connection(connectionType, client, input, output);

            return IOResult.CreateSuccess(connection);
        } 

    }
}
