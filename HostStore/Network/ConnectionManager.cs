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

namespace HostStore.Network
{

    internal class ConnectionManager
    {

        private bool _accepting;
        private ConcurrentDictionary<Guid, Session> _sessions;
        private IPEndPoint _localEndpoint;
        private IPEndPoint _publicEndpoint;

        public ConnectionManager(IPAddress lanAddress, IPAddress wanAddress, int port)
        {
            _localEndpoint = new IPEndPoint(lanAddress, port);
            _publicEndpoint = new IPEndPoint(wanAddress, port);
            _sessions = new ConcurrentDictionary<Guid, Session>();
            _accepting = true;
        }

        public Session OpenSession()
        {
            Guid sessionId = Guid.NewGuid();
            Session session = new Session(_localEndpoint, _publicEndpoint, sessionId);

            _sessions[sessionId] = session;
            return session;
        }

        public async Task ListenForConnectionsAsync()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 0); // assign address and port automatically
   
            while(_accepting)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();

                _ = Task.Run(async () => 
                {
                    try
                    {
                        using (var clientWrapper = new TcpClientWrapper(client))
                        {
                            await readAndProcessRequestAsync(clientWrapper);
                        }
                    }
                    catch (Exception)
                    {

                    }
                    finally
                    {
                        client.Close();
                    }
                    
                });

            }
        }

        private async Task readAndProcessRequestAsync(TcpClientWrapper client)
        {
            var reader = new StreamReader(client.Client.GetStream());
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
                    await processFileGetRequestAsync(client, get);
                    break;
                case RequestType.FILE_SEND:
                    var put = (FilePutRequest?)JsonConvert.DeserializeObject(requestJson, typeof(FilePutRequest));
                    await processFilePutRequestAsync(client, put);
                    break;
            }
        }

        private async Task processMessageLoopRequestAsync(TcpClientWrapper client, AuthenticationRequest? request)
        {
            if (request == null)
                throw new Exception("Improper request");

            AuthenticationResponse? res = null;

            if (_sessions.ContainsKey(request.SessionRequestId) == false)
                res = AuthenticationResponse.CreateFailure("Session does not exist");

            Session session = _sessions[request.SessionRequestId];
            bool passwordAccepted = session.ConsumeInvite(request.Password);
            if (passwordAccepted == false)
                res = AuthenticationResponse.CreateFailure("Password is not correct");

            Guid userId = Guid.NewGuid();
            Guid userPrivateKey = Guid.NewGuid();

            if (res == null)
                res = AuthenticationResponse.CreateSuccess(request.SessionRequestId, userId, userPrivateKey);

            await client.WriteObjectAsync(res);

            await session.ProcessUserUntilClosedAsync(userPrivateKey, userId, client);
        }

        private async Task processFileGetRequestAsync(TcpClientWrapper client, FileGetRequest? request)
        {
            if (request == null)
                throw new Exception("Improper request");

            FileGetResponse? res = null;

            if (_sessions.ContainsKey(request.SessionId) == false)
                res = FileGetResponse.CreateFailure("Session does not exist");



        }

        private async Task processFilePutRequestAsync(TcpClientWrapper client, FilePutRequest? request)
        {
            new FileSupplier();
        }
    }
}
