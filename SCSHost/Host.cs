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

        private PathSelector _pathSelector;
        private HostState _state;
        private FileGetHandler _fileGetHandler;
        private FilePutHandler _filePutHandler;
        private MessageChanelHandler _messageChanelHandler;
        private ConnectionListener _connectionListener;

        public Host(PathSelector pathSelector)
        {
            _pathSelector = pathSelector;
            _state = new HostState();

            _fileGetHandler = new FileGetHandler(_state, new HostFileSender(), _pathSelector);
            _filePutHandler = new FilePutHandler(_state, new HostFileReciever(), _pathSelector);
            _messageChanelHandler = new MessageChanelHandler(_state, new HostAuthenticator(), new HostMessageChanel());

            _connectionListener = new ConnectionListener();
        }

        public async Task AcceptConnections(IPAddress localAddress, int port = 0)
        {
            TcpListener tcpListener = new TcpListener(localAddress, port);

            while(true)
            {
                var connectionResult = await _connectionListener.ListenForConnection(tcpListener);
                if (connectionResult.Failed)
                    continue;

                Task<IOErrorType> result = Task.Run(async () => 
                {
                    Connection? connection = connectionResult.Value;

                    IConnectionHandler handler = routeConnection(connection.ConnectionType);
                    IOErrorType result = await handler.HandleConnection(connection);
                    
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
