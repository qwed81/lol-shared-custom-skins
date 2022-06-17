using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using StoreModels.Messages.Client;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using StoreModels;
using StoreModels.Messages.Server;
using StoreModels.File;

namespace HostStore
{

    public class ConnectionListener
    {

        private bool _accepting;
        private IConnectionEstablishedHandler _connectionHandler;

        internal ConnectionListener(IConnectionEstablishedHandler connectionHandler)
        {
            _accepting = true;
            _connectionHandler = connectionHandler;
        }

        public async Task ListenForConnectionsAsync()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 0); // assign address and port automatically
   
            while(_accepting)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();

                _ = Task.Run(async () => await onConnectionEstablished(client));
            }
        }

        private async Task onConnectionEstablished(TcpClient client)
        {
            try
            {
                using (var clientWrapper = new TcpClientWrapper(client))
                {
                    ClientRequest req = await readRequestAsync(clientWrapper);
                    await _connectionHandler.HandleEstablishedConnection(clientWrapper, req);
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                client.Close();
            }
        }

        private async Task<ClientRequest> readRequestAsync(TcpClientWrapper client)
        {
            var reader = new StreamReader(client.Client.GetStream());
            string requestJson = await reader.ReadLineAsync();
            var request = JsonConvert.DeserializeObject(requestJson, typeof(ClientRequest));

            if (request == null)
                throw new Exception("Improper request");

            return (ClientRequest)request;
        }

    }
}
