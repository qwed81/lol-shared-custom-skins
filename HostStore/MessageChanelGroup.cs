using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;
using StoreModels;
using StoreModels.Messages.Server;
using StoreModels.Messages.Client;
using System.Collections;

namespace HostStore
{

    internal class MessageChanelGroup
    {

        private ConcurrentDictionary<Guid, MessageChanel> _userMap;
        private Guid _sessionId;
        private TaskQueue<(Type, object)> _messageQueue;

        public IEnumerable<IAuthenticatedUser> Users => _userMap.Values;

        public MessageChanelGroup(Guid sessionId)
        {
            _userMap = new ConcurrentDictionary<Guid, MessageChanel>();
            _sessionId = sessionId;
            _messageQueue = new TaskQueue<(Type, object)>(postMessageToAllAsync);
        }

        private async Task postMessageToAllAsync((Type, object) messageTuple)
        {
            List<Task> tasks = new List<Task>();
            foreach (var chanel in _userMap.Values)
            {
                Task t = chanel.PostMessageAsync(messageTuple.Item1, messageTuple.Item2);
                tasks.Add(t);
            }

            await Task.WhenAll(tasks);
        }

        public void PostMessageToAll(Type messageType, object message)
        {
            var tuple = (messageType, message);
            _messageQueue.Enqueue(tuple);
        }

        public async Task HandleChanelConnectionRequestAsync(TcpClientWrapper client, 
            IAuthenticationManager authManager, IMessageHandler messageHandler, ConnectMessageChanelRequest req)
        {
            var chanel = new MessageChanel(client, _sessionId);
            await chanel.HandleAuthenticationAsync(req, authManager);
            _userMap[chanel.PrivateAccessKey] = chanel;

            try
            {
                await chanel.ProcessUntilClosedAsync(messageHandler);
            }
            finally
            {
                _userMap.TryRemove(chanel.PrivateAccessKey, out _);
            }
        }

        public bool UserIsAuthenticated(Guid userPrivateKey)
        {
            return _userMap.ContainsKey(userPrivateKey);
        }

        public void Close()
        {
            foreach(var mc in _userMap.Values)
            {
                mc.Cancel();
            }
        }


    }
}
