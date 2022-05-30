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
    internal class FileSender
    {

        private string _host;
        private int _port;
        private FileIndex _fileIndex;

        public FileSender(string host, int port, FileIndex fileIndex)
        {
            _host = host;
            _port = port;
            _fileIndex = fileIndex;
        }

        public async Task SendFileAsync(Guid sessionId, Guid userId, FileDescriptor fd, FileType fileType)
        {
            using (FileStream fileStream = _fileIndex.StreamCompletedFile(sessionId, fd, fileType))
            using (TcpClient client = new TcpClient())
            {
                await client.ConnectAsync(_host, _port);

                NetworkStream networkStream = client.GetStream();
                StreamWriter sw = new StreamWriter(networkStream);

                ulong fileSize = (ulong)fileStream.Length;

                await sendFileMetadata(sessionId, fd, fileType, fileSize, sw);

                byte[] buffer = new byte[1024];
                int amtRead = 1;
                while(amtRead != 0)
                {
                    amtRead = await fileStream.ReadAsync(buffer, 0, buffer.Length);
                    if (amtRead == 0)
                        break;

                    await networkStream.WriteAsync(buffer, 0, amtRead);
                }
            }
        }

        private async Task sendFileMetadata(Guid sessionId, FileDescriptor fd, FileType fileType, ulong fileLength, StreamWriter writer)
        {
            var metadata = new FileMetadata(sessionId, fd, fileType, fileLength);
            string metadataJson = SerializeUtil.SerializeObject(metadata);
            await writer.WriteLineAsync(metadataJson);
            await writer.FlushAsync();

        }

    }
}
