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

namespace HostStore.Network
{

    internal class ConnectionRouter
    {

        private bool _accepting;
        private ConcurrentDictionary<Guid, Session> _sessions;
        private IPEndPoint _localEndpoint;
        private IPEndPoint _publicEndpoint;
        private FileReciever _fileReciever;

        public ConnectionRouter(IPAddress lanAddress, IPAddress wanAddress, int port, FileIndex fileIndex)
        {
            _localEndpoint = new IPEndPoint(lanAddress, port);
            _publicEndpoint = new IPEndPoint(wanAddress, port);
            _sessions = new ConcurrentDictionary<Guid, Session>();
            _accepting = true;
            _fileReciever = new FileReciever(fileIndex);
        }

        public ISession OpenSession()
        {
            Guid sessionId = Guid.NewGuid();
            PasswordManager pwm = new PasswordManager(_localEndpoint, _publicEndpoint, sessionId);
            Session session = new Session(sessionId, pwm);

            _sessions[sessionId] = session;
            return session;
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
                    await routeRequestAsync(clientWrapper, req);
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



        private async Task routeRequestAsync(TcpClientWrapper client, ClientRequest clientRequest)
        {

            if (_sessions.ContainsKey(clientRequest.SessionId) == false) // if the session does not exist
            {
                var res = ServerResponse.CreateFailureResponse(clientRequest.RequestType, "Session doesn't exist");
                await client.WriteObjectAsync(res);
                return;
            }

            var session = _sessions[clientRequest.SessionId];

            switch (clientRequest.RequestType)
            {
                case RequestType.CONNECT_MESSAGE_CHANEL:
                    var ml = new MessageChanel(client, session);
                    await ml.HandleChanelConnectionRequestAsync(clientRequest as ConnectMessageChanelRequest);
                    break;
                case RequestType.FILE_GET:
                    await _fileReciever.HandleFileGetRequestAsync(client, clientRequest as FileGetRequest, session);
                    break;
                case RequestType.FILE_PUT:
                    await _fileReciever.HandleFilePutRequestAsync(client, clientRequest as FilePutRequest, session);
                    break;
                default:
                    throw new Exception("Invalid request");
            };
            
        }

    }
}
