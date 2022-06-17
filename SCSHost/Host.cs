using HostLib;
using SharedLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Models.Network;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using SCSHost.ConnectionHandlers;
using HostLib.Models;

namespace SCSHost
{
    public class Host
    {

        private readonly ConcurrentDictionary<Guid, Session> _sessions;

        private FileGetHandler _fileGetHandler;
        private FilePutHandler _filePutHandler;
        private MessageChanelHandler _messageChanelHandler;
        private ConnectionListener _connectionListener;

        public Host()
        {
            _sessions = new ConcurrentDictionary<Guid, Session>(); // sets up all dependencies

            _fileGetHandler = new FileGetHandler(_sessions, new HostFileSender());
            _filePutHandler = new FilePutHandler(_sessions, new HostFileReciever());
            _messageChanelHandler = new MessageChanelHandler(_sessions, new HostAuthenticator(), new HostMessageChanel());

            _connectionListener = new ConnectionListener();
        }

        public async Task AcceptConnections(IPAddress localAddress)
        {
            TcpListener tcpListener = new TcpListener(localAddress, 0);

            while(true)
            {
                var connectionResult = await _connectionListener.ListenForConnection(tcpListener);
                if (connectionResult.Failed)
                    continue;

                Task<IOErrorType> result = Task.Run(async () => 
                {
                    var sessionId = connectionResult.Value.SessionId;
                    var connection = connectionResult.Value.Connection;

                    if (_sessions.ContainsKey(sessionId) == false)
                        return IOErrorType.NoSession;

                    IConnectionHandler handler = routeConnection(connection.ConnectionType);
                    IOErrorType result = await handler.HandleConnection(sessionId, connection);
                    
                    connection.Dispose();

                    return result;
                });
            }
        }

        private IConnectionHandler routeConnection(ConnectionType connectionType)
        {
            return connectionType switch
            {
                ConnectionType.FilePut => _fileGetHandler,
                ConnectionType.FileGet => _filePutHandler,
                ConnectionType.MessageChanel => _messageChanelHandler,
                _ => throw new ArgumentException("Connection not valid")
            };
        }

    }
}
