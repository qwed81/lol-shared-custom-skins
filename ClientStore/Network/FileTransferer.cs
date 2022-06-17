using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using StoreModels;
using StoreModels.File;
using StoreModels.Messages;
using StoreModels.Messages.Client;
using StoreModels.Messages.Server;

namespace ClientStore.Network
{

    internal class ExtendedFilePutRequest : FileGetRequest
    {
        public IProgress<double> SendProgress { get; set; }

        public ExtendedFilePutRequest(Guid sessionId, Guid privateAccessToken, IProgress<double> sp,
            FileDescriptor fd, FileType fileType) : base(sessionId, privateAccessToken, fd, fileType)
        {
            SendProgress = sp;
            FileType = fileType;
        }
    }

    internal class FileTransferer
    {

        private string _host;
        private int _port;
        private FileIndex _fileIndex;
        private TaskQueue<ExtendedFilePutRequest> _putQueue;
        private TaskQueue<FileGetRequest> _getQueue;

        public FileTransferer(string host, int port, FileIndex fileIndex)
        {
            _host = host;
            _port = port;
            _fileIndex = fileIndex;
            _putQueue = new TaskQueue<ExtendedFilePutRequest>(startSendFile);
            _getQueue = new TaskQueue<FileGetRequest>(startDownloadFile);
        }

        public void RequestSendFile(Guid sessionId, Guid privateAccessToken,
            FileDescriptor fd, FileType fileType, IProgress<double> sendProgress)
        {
            var request = new ExtendedFilePutRequest(sessionId, privateAccessToken, sendProgress, fd, fileType);
            _putQueue.Enqueue(request);
        }

        public void RequestRetrieveFile(Guid sessionId, Guid privateAccessToken,
            FileDescriptor fd, FileType fileType)
        {
            if (_fileIndex.FileExists(fd))
                return;

            var request = new FileGetRequest(sessionId, privateAccessToken, fd, fileType);
            _getQueue.Enqueue(request);
        }

        private async Task startSendFile(ExtendedFilePutRequest req)
        {
            using (FileStream? fs = _fileIndex.StreamCompletedFile(req.SessionId, req.File, req.FileType))
            using (TcpClient tcpClient = new TcpClient())
            {
                if (fs == null)
                    throw new ArgumentException("File is not completed or does not exist");

                await tcpClient.ConnectAsync(_host, _port);

                using (TcpClientWrapper client = new TcpClientWrapper(tcpClient))
                {
                    await client.WriteObjectAsync(req);
                    var res = await client.ExpectObjectAsync<FilePutResponse>("Incorrect file response");

                    if (!res.Success)
                        return;

                    long fileSize = fs.Length;
                    await client.WriteFileAsync(fs, fileSize, req.SendProgress);

                }
            }
        }

        private async Task startDownloadFile(FileGetRequest req)
        {
            if (_fileIndex.FileExists(req.File))
                return;

            FileProgress fp;
            using (FileStream fs = _fileIndex.StreamCreateFile(req.SessionId, req.File, req.FileType, out fp))
            using (TcpClient tcpClient = new TcpClient())
            {
                await tcpClient.ConnectAsync(_host, _port);

                using (TcpClientWrapper client = new TcpClientWrapper(tcpClient))
                {
                    await client.WriteObjectAsync(req);
                    var res = await client.ExpectObjectAsync<FileGetResponse>("Incorrect file response");

                    if (!res.Success)
                        return;

                    await client.DownloadFileAsync(fs, res.FileSize, fp);

                }
            }
        }

    }
}
