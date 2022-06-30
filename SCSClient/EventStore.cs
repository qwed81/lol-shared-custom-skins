using Models.Network;
using SCSClient.Types;
using System.Collections.Concurrent;

namespace SCSClient
{
    public enum EventUser 
    {
        ActionThread, DataResponseThread, HostMessageThread, ClientMessageThread
    }


    public class EventStore
    {

        private BlockingCollection<TypeMessagePair> _outboundMessages;

        private BlockingCollection<DataUpdate> _dataUpdates;

        public EventStore()
        {
            _outboundMessages = new BlockingCollection<TypeMessagePair>();
            _dataUpdates = new BlockingCollection<DataUpdate>();
        }

        public void QueueDataUpdate(EventUser eventUser, DataUpdate update)
        {
            _dataUpdates.Add(update);
        }

        public void QueueOutboundMessage(EventUser eventUser, TypeMessagePair message)
        {
            _outboundMessages.Add(message);
        }

        public DataUpdate PollDataUpdate(EventUser eventUser)
        {
            return _dataUpdates.Take();
        }

        public TypeMessagePair PollOutboundMessage(EventUser eventUser)
        {
            return _outboundMessages.Take();
        }

    }
}
