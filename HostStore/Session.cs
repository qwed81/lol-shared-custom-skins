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

    internal class Session : ISession
    {

        private ConcurrentDictionary<Guid, IAuthenticatedUser> _userMap;
        
        public event MessageHandler? OnMessage;

        public Guid SessionId { get; }

        public PasswordManager PasswordManager { get; }

        public Session(Guid sessionId, PasswordManager passwordManager)
        {
            _userMap = new ConcurrentDictionary<Guid, IAuthenticatedUser>();
            SessionId = sessionId;
            PasswordManager = passwordManager;
        }

        public bool UserIsAuthenticated(Guid userPrivateKey)
        {
            return _userMap.ContainsKey(userPrivateKey);
        }

        public void AddUser(IAuthenticatedUser user)
        {
            _userMap[user.PrivateAccessKey] = user;
        }

        public void RemoveUser(IAuthenticatedUser user)
        {
            _userMap.TryRemove(user.PrivateAccessKey, out _);
        }

        public void InvokeOnMessage(IAuthenticatedUser sender, Type messageType, object message)
        {
            OnMessage?.Invoke(this, sender, messageType, message);
        }

        public async Task PostMessageToAllAsync(Type messageType, object message)
        {
            List<Task> tasks = new List<Task>();
            foreach(var user in _userMap.Values)
            {
                await user.PostMessageAsync(messageType, message);
            }
            await Task.WhenAll(tasks);
        }

        public IEnumerator<IAuthenticatedUser> GetEnumerator()
        {
            return _userMap.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _userMap.Values.GetEnumerator();
        }
    }
}
