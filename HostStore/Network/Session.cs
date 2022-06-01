using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Net.Sockets;
using StoreModels;

namespace HostStore.Network
{

    internal delegate void MessageHandler(Guid userId, Type messageType, object message);

    internal class Session
    {

        private ConcurrentDictionary<string, bool> _passwordSet;
        private ConcurrentDictionary<Guid, ServerMessageLoop> _userMap; // private key to messageLoop
        private ConcurrentDictionary<Guid, Guid> _userIdToPrivateKey;
        private IPEndPoint _publicEndpoint;
        private IPEndPoint _localEndpoint;
        
        public event MessageHandler? OnMessage;

        public Guid SessionId { get; }

        public Session(IPEndPoint localEndPoint, IPEndPoint publicEndPoint, Guid sessionId)
        {
            _passwordSet = new ConcurrentDictionary<string, bool>();
            _userMap = new ConcurrentDictionary<Guid, ServerMessageLoop>();
            _userIdToPrivateKey = new ConcurrentDictionary<Guid, Guid>();

            _localEndpoint = localEndPoint;
            _publicEndpoint = publicEndPoint;
            SessionId = sessionId;
        }

        private string createInvite(IPEndPoint endpoint, bool admin)
        {
            string inviteStr = InviteUtil.CreateInviteString(endpoint, admin);
            var (_, password) = InviteUtil.ParseInviteString(inviteStr);

            _passwordSet[password] = true;

            return inviteStr;
        }

        public string CreateWanInvite(bool admin)
        {
            return createInvite(_publicEndpoint, admin);
        }

        public string CreateLanInvite(bool admin)
        {
            return createInvite(_localEndpoint, admin);
        }

        public bool ConsumeInvite(string password)
        {
            if (_passwordSet[password] == false)
                return false;

            _passwordSet[password] = false;
            return true;
        }

        public bool UserIsAuthenticated(Guid userPrivateKey)
        {
            return _userMap.ContainsKey(userPrivateKey);
        }

        private void onMessageLoopMessage(ServerMessageLoop messageLoop, Type messageType, object message)
        {
            OnMessage?.Invoke(messageLoop.UserId, messageType, message);
        }

        public async Task ProcessUserUntilClosedAsync(Guid userPrivateKey, Guid userId, TcpClientWrapper client)
        {
            ServerMessageLoop messageLoop = new ServerMessageLoop(SessionId, userId, client);
            
            _userMap[userPrivateKey] = messageLoop;
            try
            {
                await messageLoop.ProcessUntilClosedAsync(onMessageLoopMessage);
            }
            finally
            {
                _userMap.TryRemove(userPrivateKey, out _);
            }
            
        }

        public async Task PostMessageAsync(Guid userId, Type messageType, object message)
        {
            Guid privateKey = _userIdToPrivateKey[userId];
            await _userMap[privateKey].PostMessageAsync(messageType, message);
        }

        public async Task PostMessageToAllAsync(Type messageType, object message)
        {
            List<Task> tasks = new List<Task>();
            foreach(var messageLoop in _userMap.Values)
            {
                await messageLoop.PostMessageAsync(messageType, message);
            }
            await Task.WhenAll(tasks);
        }

    }
}
