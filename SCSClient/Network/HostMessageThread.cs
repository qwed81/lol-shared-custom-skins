using ClientLib;
using SCSClient.MessageTranslator;
using SCSClient.Types;

namespace SCSClient.NetworkClient
{
    public class HostMessageThread
    {

        private EventStore _store;
        private MessageChanel _chanel;
        private HostMessageHandler _handler;
        private RemoteConnectionState _connectionState;

        public HostMessageThread(EventStore eventStore, MessageChanel chanel,
            HostMessageHandler handler, RemoteConnectionState connectionState)
        {
            _store = eventStore;
            _chanel = chanel;
            _handler = handler;
            _connectionState = connectionState;
        }

        public void Start()
        {
            _store.QueueDataUpdate(EventUser.HostMessageThread, new PartyConnectedDataUpdate());
            _ = Task.Run(async () =>
            {
                while(true)
                {
                    var messageResult = await _chanel.RecieveMessage(_connectionState.Connection.Input);
                    if (messageResult.Failed)
                        break;

                    IEnumerable<DataUpdate> updates = _handler.Handle(messageResult.Value);
                    foreach(var update in updates)
                        _store.QueueDataUpdate(EventUser.HostMessageThread, update);
                }

                _store.QueueDataUpdate(EventUser.HostMessageThread, new PartyDisconnectedDataUpdate());
            });
        }

    }
}
