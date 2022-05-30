using StoreModels.Messages;
using StoreModels.Messages.Client;
using StoreModels.Messages.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HostStore.Network
{
    internal delegate void MessageHandler(MessageLoop sender, Type messageType, object message);

    internal class MessageLoop
    {

        private bool _active;
        private List<TcpClient> _activeClients;
        private TextWriter _logger;

        public Guid SessionId { get; private set; }

        public event MessageHandler? OnMessage;

        public MessageLoop(TextWriter logger)
        {

            SessionId = Guid.NewGuid();

            _active = true;

        }

        public async Task Run(int port)
        {
            IPAddress ipAddress = Dns.GetHostEntry("localhost").AddressList[0]; // ip address of localhost
            TcpListener listener = new TcpListener(ipAddress, port);

            while(_active)
            {
                using (TcpClient client = await listener.AcceptTcpClientAsync())
                {
                    _activeClients.Add(client);

                    try
                    {
                        await Task.Run(messageLoop);
                    }
                    catch (Exception e)
                    {
                        _logger.WriteLine("Client disconnected, reason: " + e);
                    }

                    _activeClients.Remove(client);
                }

            }

        }

        public async Task PostMessageAsync()
        {

        }

        public void Close()
        {

        }

        private async Task messageLoop(StreamWriter writer, StreamReader reader)
        {
            var authInfo = await reciveAuthenticationInfo();
        }

        private async Task<ClientAuthenticationInfo> reciveAuthenticationInfo(StreamReader reader)
        {
            var response = await readObjectAsync(reader, typeof(ClientAuthenticationInfo));
            if (response == null)
                throw new Exception("Client did not send proper authentication information");

            return (ClientAuthenticationInfo)response;
        }

        private async Task sendAuthenticationResponse(StreamWriter writer, bool success, Guid? userId,
            string? failureReason)
        {
            ServerAuthenticationResponse response;
            if(success)
            {
                response = new ServerAuthenticationResponse(true, SessionId, userId, failureReason);
            }
            else
            {
                response = new ServerAuthenticationResponse(false, null, null, failureReason);
            }

            await writeObjectAsync(writer, response);
            
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


    }
}
