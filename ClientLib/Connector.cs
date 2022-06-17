using Models.Network;
using Models.Network.Messages.Client;
using SharedLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientLib
{
    public class Connector
    {

        private async Task<IOResult<Connection>> connect(string host, int port, Guid sessionId, ConnectionType type)
        {
            TcpClient client = new TcpClient(host, port);
            try
            {
                await client.ConnectAsync(host, port);
            }
            catch (IOException)
            {
                return IOResult.CreateFailure<Connection>(IOErrorType.IOError);
            }

            var header = new ConnectionHeader(sessionId, type);
            var output = new AugmentedOutputStream(client.GetStream());
            var result = await output.WriteObjectsAsync(header);
            if (result.Failed)
                return IOResult.CreateFailure<Connection>(result.ErrorType);

            var input = new AugmentedInputStream(client.GetStream());
            Connection connection = new Connection(type, client, input, output);
            return IOResult.CreateSuccess(connection);
        }

        public Task<IOResult<Connection>> ConnectMessageChanel(string host, int port, Guid sessionId)
        {
            return connect(host, port, sessionId, ConnectionType.MessageChanel);
        }

        public Task<IOResult<Connection>> ConnectFilePut(string host, int port, Guid sessionId)
        {
            return connect(host, port, sessionId, ConnectionType.FilePut);
        }

        public Task<IOResult<Connection>> ConnectFileGet(string host, int port, Guid sessionId)
        {
            return connect(host, port, sessionId, ConnectionType.FileGet);
        }

    }
}
