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

        private StreamWrapper _streamWrapper;

        public TcpClient Client { get; }

        public TcpClientWrapper(TcpClient client)
        {
            Client = client;
            _streamWrapper = new StreamWrapper(client.GetStream());
        }

        public async Task WriteObjectAsync(object obj)
        {
            await _streamWrapper.WriteObjectAsync(obj);
        }

        public async Task WriteObjectsAsync(params object[] objs)
        {
            await _streamWrapper.WriteObjectAsync(objs);
        }

        public async Task<object?> ReadObjectAsync(Type deserializeType)
        {
            return await _streamWrapper.ReadObjectAsync(deserializeType);
        }

        public async Task<T> ExpectObjectAsync<T>(string errorMesage, Type? deserializeType = null)
        {
            return await _streamWrapper.ExpectObjectAsync<T>(errorMesage, deserializeType);
        }

        public async Task WriteFileAsync(Stream outputFs, long fileSize, IProgress<double> progress)
        {
            await _streamWrapper.WriteFileAsync(outputFs, fileSize, progress);
        }

        public async Task DownloadFileAsync(Stream inputFs, long fileSize, IProgress<double> progress)
        {
            await _streamWrapper.ReadFileAsync(inputFs, fileSize, progress);
        }

        // does not dispose the underlying tcp client
        public void Dispose()
        {
            _streamWrapper.Dispose();
        }

    }
}
