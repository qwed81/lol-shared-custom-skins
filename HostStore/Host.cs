using StoreModels;
using StoreModels.File;
using StoreModels.Messages.Client;
using StoreModels.Messages.Server;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HostStore
{
    public class Host : IConnectionEstablishedHandler
    {

        private FileGetHandler _fileGetHandler;
        private FilePutHandler _filePutHandler;

        public SessionStore SessionStore { get; set; }

        public ConnectionListener ConnectionListener { get; set; }

        public Host(FileIndex fileIndex)
        {
            SessionStore = new SessionStore();
            ConnectionListener = new ConnectionListener(this);
            _fileGetHandler = new FileGetHandler(fileIndex);
            _filePutHandler = new FilePutHandler(fileIndex);
        }

        public async Task HandleEstablishedConnection(TcpClientWrapper client, ClientRequest req)
        {
            if (SessionStore.SessionExists(req.SessionId) == false) // if the session does not exist
            {
                var res = ServerResponse.CreateFailureResponse(req.RequestType, "Session doesn't exist");
                await client.WriteObjectAsync(res);
                return;
            }

            var session = SessionStore.GetSession(req.SessionId);

            switch (req.RequestType)
            {
                case RequestType.CONNECT_MESSAGE_CHANEL:
                    await session.HandleChanelConnectionRequestAsync(client, (ConnectMessageChanelRequest)req);
                    break;
                case RequestType.FILE_GET:
                    await _fileGetHandler.HandleFileGetRequestAsync(client, (FileGetRequest)req, session);
                    break;
                case RequestType.FILE_PUT:
                    await _filePutHandler.HandleFilePutRequestAsync(client, (FilePutRequest)req, session, session);
                    break;
                default:
                    throw new Exception("Invalid request");
            };
        }
    }
}
