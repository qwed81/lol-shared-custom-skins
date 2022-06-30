using HostLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSHost.ConnectionHandlers
{
    public class MessageOutputThread
    {

        private Thread _thread;
        private HostState _state;
        private HostMessageChanel _chanel;

        public MessageOutputThread(HostState hostState, HostMessageChanel chanel)
        {
            _thread = new Thread(threadStart);
            _state = hostState;
            _chanel = chanel;
        }

        public void Start()
        {
            _thread.Start();
        }

        private void threadStart()
        {
            while(true)
            {
                var message = _state.OutboundMessages.Take();

                List<Task> sendTasks = new List<Task>();
                _state.Connections.ForEach((connection, _) =>
                {
                    Task sendTask = _chanel.SendMessage(connection.Connection.Output, message.MessageType, message.Message);
                    sendTasks.Add(sendTask);
                });

                Task.WaitAll(sendTasks.ToArray());
            }
        }

    }
}
