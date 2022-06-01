using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using StoreModels;
using StoreModels.File;
using StoreModels.Messages;
using StoreModels.Messages.Client;

namespace ClientStore.Network
{
    internal class ClientFileSharer
    {

        private string _host;
        private int _port;
        private FileIndex _fileIndex;

        public ClientFileSharer(string host, int port, FileIndex fileIndex)
        {
            _host = host;
            _port = port;
            _fileIndex = fileIndex;
        }

        public Task<bool> RetrieveFileIfAbsentAsync(Guid sessionId, Guid privateAccessToken,
            FileDescriptor fd, FileType fileType)
        {
            if (_fileIndex.FileExists(fd))
                return Task.FromResult(true);


            Action<double> setCompletion;
            using (FileStream fileStream = _fileIndex.StreamCreateFile(sessionId, fd, fileType, out setCompletion))
            using (TcpClient client = new TcpClient())
            {
                try
                {
                    await client.ConnectAsync(_host, _port);

                    // sends request
                    using (var clientWrapper = new TcpClientWrapper(client))
                    {
                        var request = new FileGetRequest(sessionId, privateAccessToken, fd);
                        await clientWrapper.WriteObjectAsync(request);
                    }                        

                    await retrieveFileBytesAsync(client.GetStream(), fileStream, setCompletion);

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        private async Task downloadFile(TcpClientWrapper client, FileStream fileStream, 
            Guid sessionId, Guid privateAccessToken, FileDescriptor fd, FileType fileType)
        {

        }

    }
}
