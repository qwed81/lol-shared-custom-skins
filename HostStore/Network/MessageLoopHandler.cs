using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;

namespace HostStore.Network
{

    internal delegate void SourcedMessageHandler(MessageLoopHandler handler, Guid userId, Type messageType,
        object message);

    internal class MessageLoopHandler
    {

        private ConcurrentDictionary<Guid, ServerMessageLoop> _messageLoops;

        public MessageLoopHandler()
        {
            _messageLoops = new ConcurrentDictionary<Guid, ServerMessageLoop>();
        }

        public async Task ProcessClientUntilClosedAsync(ServerMessageLoop messageLoop)
        {
            _messageLoops[messageLoop.UserId] = messageLoop;

            await messageLoop.ProcessUntilClosedAsync();

            ServerMessageLoop cleared;
            _messageLoops.TryRemove(messageLoop.UserId, out cleared);
        }

        public async Task PostMessageAsync(Guid userId, Type messageType, object message)
        {
            await _messageLoops[userId].PostMessageAsync(messageType, message);
        }

        public async Task PostMessageToAllAsync(Type messageType, object message)
        {
            foreach(var messageLoop in _messageLoops.Values)
            {
                await messageLoop.PostMessageAsync(messageType, message);
            }
        }

        public void Close()
        {
            foreach (var messageLoop in _messageLoops.Values)
            {
                messageLoop.Dispose();
            }

            _messageLoops.Clear();
        }

    }
}
