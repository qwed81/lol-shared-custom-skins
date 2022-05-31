using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using StoreModels.Messages.Client;
using System.IO;
using Newtonsoft.Json;

namespace HostStore.Network
{
    internal class ConnectionManager
    {

        private bool _accepting;
        private MessageLoopHandler _messageHandler;

        public ConnectionManager()
        {
            _accepting = true;
        }

        public async Task ListenForConnectionsAsync()
        {

            TcpListener listener = new TcpListener(IPAddress.Any, 0); // assign address and port automatically
   
            while(_accepting)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();


                _ = Task.Run(async () => 
                {
                    await readAndProcessRequestAsync(client);
                    client.Close();
                });

            }
        }

        private async Task readAndProcessRequestAsync(TcpClient client)
        {
            var reader = new StreamReader(client.GetStream());
            string requestJson = await reader.ReadLineAsync();
            var request = JsonConvert.DeserializeObject(requestJson, typeof(ClientRequest));

            if (request == null)
                throw new Exception("Improper request");

            switch(((ClientRequest)request).RequestType)
            {
                case RequestType.MESSAGE_LOOP:
                    var auth = (AuthenticationRequest?)JsonConvert.DeserializeObject(requestJson, typeof(AuthenticationRequest));
                    await processMessageLoopRequestAsync(client, auth);
                    break;
                case RequestType.FILE_RETRIVE:
                    var get = (FileGetRequest?)JsonConvert.DeserializeObject(requestJson, typeof(FileGetRequest));
                    await processFileRetrieveRequestAsync(client, get);
                    break;
                case RequestType.FILE_SEND:
                    var put = (FilePutRequest?)JsonConvert.DeserializeObject(requestJson, typeof(FilePutRequest));
                    await processFileSendRequestAsync(client, put);
                    break;
            }
        }
        
        private async Task processMessageLoopRequestAsync(TcpClient client, AuthenticationRequest? request)
        {
            Guid userId = Guid.NewGuid();
            var messageLoop = new ServerMessageLoop(request.SessionRequestId, );
            
        }

        private async Task processFileRetrieveRequestAsync(TcpClient client, FileGetRequest? request)
        {
            new FileReciever();
        }

        private async Task processFileSendRequestAsync(TcpClient client, FilePutRequest? request)
        {
            new FileSupplier();
        }
    }
}
