/*
using Newtonsoft.Json;
using StoreModels.Messages;
using StoreModels.Messages.Client;
using StoreModels.Messages.Server;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClientStore.Network
{

    internal class MessageRecievedEventArgs 
    {

        public Guid SessionId { get; set; }

        public Guid UserId { get; set; }

        public Type MessageType { get; set; }

        public object Message { get; set; }

        public MessageRecievedEventArgs(Guid sessionid, Guid userId, Type messageType, object message)
        {
            SessionId = sessionid;
            UserId = userId;
            MessageType = messageType;
            Message = message;
        }

    }

    internal class ClientMessageLoop : IDisposable
    {

        private readonly string _host;
        private readonly int _port;
        private ConcurrentQueue<object> _outboundMessageQueue;
        private StreamWriter? _streamWriter;
        private bool _isRunning;

        public Guid SessionId { get; private set; }

        public Guid UserId { get; private set; }

        public bool IsConnected { get; private set; }

        public bool HeadersSet { get; private set; }

        public event EventHandler<MessageRecievedEventArgs>? MessageRecived;

        public ClientMessageLoop(string host, int port)
        {
            _host = host;
            _port = port;
            _outboundMessageQueue = new ConcurrentQueue<object>();
            
        }

        public void Start()
        {
            _isRunning = true;
            Task.Run(startOnThread); // runs the message handler thread
        }

        private async Task startOnThread()
        {
            while(_isRunning)
            {
                try
                {
                    await connectAsync();
                    IsConnected = false;
                }
                catch (Exception)
                {
                    IsConnected = false;
                    await Task.Delay(1000);
                }
                
            }

        }

        public void PostMessage(object message) 
        {
            if (message == null)
                throw new ArgumentException("message is null");

            _outboundMessageQueue.Enqueue(message);

            if (IsConnected)
                Task.Run(sendAllPostedMessages); // sends messages on a new thread
        }

        public void Stop()
        {
            _isRunning = false;
        }

        private async Task sendAllPostedMessages()
        {
            while(_outboundMessageQueue.Count > 0 && IsConnected)
            {
                object message;
                if(_outboundMessageQueue.TryDequeue(out message))
                {
                    string messageTypeStr = SerializeUtil.TypeToString(message.GetType());
                    var metadata = new ClientMessageMetadata(SessionId, UserId, Guid.NewGuid(), messageTypeStr);

                    await writeObjectAsync(_streamWriter!, metadata);
                    await writeObjectAsync(_streamWriter!, message);
                }    
            }
        }

        private async Task connectAsync()
        {
            if (_streamWriter != null)
                _streamWriter.Dispose();

            using (TcpClient client = new TcpClient())
            {
                await client.ConnectAsync(_host, _port);
                IsConnected = true;

                var streamReader = new StreamReader(client.GetStream());
                _streamWriter = new StreamWriter(client.GetStream());

                await sendAllPostedMessages();

                await enterMessageLoopAsync(streamReader);
            }
        }

        private async Task enterMessageLoopAsync(StreamReader streamReader)
        {




            while(_isRunning)
            {
                string? metadataJson = await streamReader.ReadLineAsync();
                string? messageJson = await streamReader.ReadLineAsync();

                Type messageType = getTypeFromMetadata(metadataJson);
                object message = parseMessage(messageType, messageJson);

                var eventArgs = new MessageRecievedEventArgs(SessionId, UserId, messageType, message);
                MessageRecived?.Invoke(this, eventArgs);
            }
        }

        private async Task writeObjectAsync(StreamWriter writer, object obj)
        {
            string objJson = SerializeUtil.SerializeObject(obj);
            await writer.WriteLineAsync(objJson);
            await writer.FlushAsync();
        }

        private async Task<object?> readObjectAsync(StreamReader reader, Type type)
        {
            string? json = await reader.ReadLineAsync();
            return SerializeUtil.DeserializeObject(type, json);
        }

        public void Dispose()
        {
            Stop();
            if(_streamWriter != null)
                _streamWriter.Dispose();
        }
    }
}
*/