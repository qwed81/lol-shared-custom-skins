using Newtonsoft.Json;
using SCSClient.Types;
using System.Net.WebSockets;
using System.Text;

namespace SCSClient.UI
{
    public class DataResponseThread
    {

        private WebSocket _websocket;
        private EventStore _eventStore;

        public DataResponseThread(EventStore eventStore)
        {
            _websocket = null!;
            _eventStore = eventStore;

            enterLoop();
        }

        public void Restart(WebSocket newSocket)
        {
            _websocket.Dispose();
            _websocket = newSocket;
        }

        private void enterLoop()
        {
            _ = Task.Run(async () =>
            {
                while(true)
                {
                    DataUpdate response = _eventStore.DataUpdates.Take();        
                    await sendDataResponse(response);
                }
            });
        }

        private async Task sendDataResponse(DataUpdate response)
        {
            string message = JsonConvert.SerializeObject(response);

            var bytes = Encoding.Default.GetBytes(message);
            var arraySegment = new ArraySegment<byte>(bytes);
            await _websocket.SendAsync(arraySegment, WebSocketMessageType.Text,
                true, CancellationToken.None);
        }

    }
}
