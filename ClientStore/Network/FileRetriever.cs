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
    internal class FileRetriever
    {

        private string _host;
        private int _port;
        private FileIndex _fileIndex;

        public FileRetriever(string host, int port, FileIndex fileIndex)
        {
            _host = host;
            _port = port;
            _fileIndex = fileIndex;
        }

        public async Task<bool> RetrieveFileIfAbsentAsync(Guid sessionId, Guid userId,
            FileDescriptor fd, FileType fileType)
        {
            if (_fileIndex.FileExists(fd))
                return true;

            Action<double> setCompletion;
            using (FileStream fileStream = _fileIndex.StreamCreateFile(sessionId, fd, fileType, out setCompletion))
            using (TcpClient client = new TcpClient())
            {
                try
                {
                    await client.ConnectAsync(_host, _port);

                    NetworkStream networkStream = client.GetStream();
                    StreamWriter sw = new StreamWriter(networkStream);

                    await sendFileRequestAsync(sessionId, fd, sw);
                    await retrieveFileBytesAsync(networkStream, fileStream, setCompletion);

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        private async Task retrieveFileBytesAsync(NetworkStream networkStream, FileStream fileStream,
            Action<double> setCompletion)
        {
            byte[] buffer = new byte[1024];
            await networkStream.ReadAsync(buffer, 0, 8); // reads a unsigned long as the file size
            ulong size = SerializeUtil.ULongFromBytes(buffer, 0);

            ulong totalAmtRead = 0;
            while (totalAmtRead < size)
            {
                int amtRead = await networkStream.ReadAsync(buffer, 0, buffer.Length);
                totalAmtRead = (ulong)amtRead + totalAmtRead;
                await fileStream.WriteAsync(buffer, 0, amtRead);
                await fileStream.FlushAsync();

                double progress = totalAmtRead / (double)size;
                setCompletion(progress);
            }

            setCompletion(1);
        }

        private async Task sendFileRequestAsync(Guid sessionId, FileDescriptor fd, StreamWriter writer)
        {
            var request = new FileGetRequest(sessionId, Guid.NewGuid(), fd);
            string requestJson = SerializeUtil.SerializeObject(request);
            await writer.WriteLineAsync(requestJson);
            await writer.FlushAsync();
        }

    }
}
