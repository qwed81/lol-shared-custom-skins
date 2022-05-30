using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;

namespace PartyStore.Tests
{
    internal class Client
    {

        TcpClient _client;

        public Client()
        {
            _client = new TcpClient();
            

        }

        public async Task ConnectAsync()
        {
            await _client.ConnectAsync("localhost", 50101);
            Console.WriteLine("Client: connected");

            NetworkStream ns = _client.GetStream();
            StreamReader sr = new StreamReader(ns);

            while(true)
            {
                string? str = await sr.ReadLineAsync();
                Console.WriteLine(str);

            }

        }


    }
}
