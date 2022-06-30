using System.Net.WebSockets;
using System.Text;
using Models.Network;
using Newtonsoft.Json;
using SCSClient.MessageTranslator;
using SCSClient.NetworkClient;
using SCSClient.Types;

namespace SCSClient.UI
{
    public class ActionThread
    {

        private WebSocket _websocket;
        private EventStore _eventStore;
        private ActionHandler _handler;

        public ActionThread(EventStore eventStore, ActionHandler handler)
        {
            _websocket = null!;
            _eventStore = eventStore;
            _handler = handler;
        }

        public void Start(WebSocket newSocket)
        {
            _websocket.Dispose();
            _websocket = newSocket;

            _ = Task.Run(async () =>
            {
                try
                {
                    while (true)
                    {
                        var arraySegment = new ArraySegment<byte>(new byte[4096]);
                        var result = await _websocket.ReceiveAsync(arraySegment, CancellationToken.None);
                        if (result.MessageType == WebSocketMessageType.Text)
                        {
                            string json = Encoding.Default.GetString(arraySegment).TrimEnd('\0');
                            object? obj = JsonConvert.DeserializeObject(json);
                            UIAction? action = (obj as UIAction);
                            if (action == null)
                                continue;

                            TypeMessagePair? pair = _handler.HandleAction(action);
                            if (pair != null)
                                _eventStore.QueueOutboundMessage(EventUser.ActionThread, pair.Value);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            });
        }

        

    }
}
