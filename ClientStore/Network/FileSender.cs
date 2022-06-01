﻿using System;
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

        public async Task<bool> SendFileAsync(Guid sessionId, Guid privateAccessToken, 
            FileDescriptor fd, FileType fileType)
        {
            using (FileStream fileStream = _fileIndex.StreamCompletedFile(sessionId, fd, fileType))
            using (TcpClient client = new TcpClient())
            {
                try
                {
                    await client.ConnectAsync(_host, _port);

                    // sends metadata
                    ulong fileSize = (ulong)fileStream.Length;
                    using (var clientWrapper = new TcpClientWrapper(client))
                    {
                        var request = new FilePutRequest(sessionId, privateAccessToken, fd, fileType, fileSize);
                        await clientWrapper.WriteObjectAsync(request);
                    }

                    await sendBytesFileAsync(client.GetStream(), fileStream);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

    }
}
