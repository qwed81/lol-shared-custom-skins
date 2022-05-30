using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PartyStore.Tests
{
    internal class Server
    {

        private TcpListener _listener;

        public Server()
        {
            const int port = 50101;

            IPAddress ipAddress = Dns.GetHostAddresses("localhost")[0];
            _listener = new TcpListener(ipAddress, port);

            _listener.Start();
        }

        public async Task AcceptClients()
        {
            while (true)
            {
                TcpClient client = await _listener.AcceptTcpClientAsync();
                Console.WriteLine("Server: client accepted");

                NetworkStream ns = client.GetStream();
                StreamWriter sw = new StreamWriter(ns);
                
                
                sw.WriteLine("Message: message 1");
                sw.WriteLine("Message: message 2");
                sw.WriteLine("Message: message 3");
                sw.Flush();
                
            }
        }

        


    }
}
