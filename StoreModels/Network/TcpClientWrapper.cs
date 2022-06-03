using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace StoreModels
{
    public class TcpClientWrapper : IDisposable
    {

        private StreamWriter _writer;
        private StreamReader _reader;

        public TcpClient Client { get; }

        public TcpClientWrapper(TcpClient client)
        {
            Client = client;
            _reader = new StreamReader(client.GetStream());
            _writer = new StreamWriter(client.GetStream());
        }

        public async Task WriteObjectAsync(object obj)
        {
            string objJson = SerializeUtil.SerializeObject(obj);
            await _writer.WriteLineAsync(objJson);
            await _writer.FlushAsync();
        }

        public async Task WriteObjectsAsync(params object[] objs)
        {
            foreach(object obj in objs)
            {
                string objJson = SerializeUtil.SerializeObject(obj);
                await _writer.WriteLineAsync(objJson);
            }

            await _writer.FlushAsync();
        }

        public async Task<object?> ReadObjectAsync(Type deserializeType)
        {
            string? json = await _reader.ReadLineAsync();
            return SerializeUtil.DeserializeObject(deserializeType, json);
        }

        public async Task<T> ExpectObjectAsync<T>(string errorMesage, Type? deserializeType = null)
        {
            string? json = await _reader.ReadLineAsync();
            if (json == null)
                throw new Exception(errorMesage);

            object? obj = SerializeUtil.DeserializeObject(deserializeType ?? typeof(T), json);
            if (obj == null)
                throw new Exception(errorMesage);

            return (T)obj;
        }

        public async Task WriteFileAsync(Stream inputFs, long fileSize, IProgress<double> progress)
        {
            var networkStream = Client.GetStream();

            byte[] buffer = new byte[1024];
            int amtRead = 1;
            long totalAmtRead = 0;
            while (amtRead != 0)
            {
                amtRead = await inputFs.ReadAsync(buffer, 0, buffer.Length);
                if (amtRead == 0)
                    break;

                totalAmtRead += amtRead;

                await networkStream.WriteAsync(buffer, 0, amtRead);
                await networkStream.FlushAsync();
                progress.Report(amtRead / (double)totalAmtRead);
            }

            progress.Report(1);
        }

        public async Task DownloadFileAsync(Stream outputFs, long fileSize, IProgress<double> progress)
        {
            var networkStream = Client.GetStream();

            byte[] buffer = new byte[1024];

            long totalAmtRead = 0;
            while (totalAmtRead < fileSize)
            {
                int amtRead = await networkStream.ReadAsync(buffer, 0, buffer.Length);
                totalAmtRead = amtRead + totalAmtRead;

                await outputFs.WriteAsync(buffer, 0, amtRead);
                await outputFs.FlushAsync();

                progress.Report(totalAmtRead / (double)fileSize);
            }

            progress.Report(1);
        } 

        // does not dispose the underlying tcp client
        public void Dispose()
        {
            _writer.Dispose();
            _reader.Dispose();
        }


    }
}
