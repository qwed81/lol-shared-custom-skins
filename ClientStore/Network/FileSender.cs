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
        public FileType FileType { get; set; }

        public IProgress<double> SendProgress { get; set; }

        public ExtendedFilePutRequest(Guid sessionId, Guid privateAccessToken, IProgress<double> sp,
            FileDescriptor fd, FileType fileType) : base(sessionId, privateAccessToken, fd)
        {
            SendProgress = sp;
            FileType = fileType;
        }
    }

    internal class ExtendedFileGetRequest : FileGetRequest
    {
        public FileType FileType { get; set; }

        public ExtendedFileGetRequest(Guid sessionId, Guid privateAccessToken,
            FileDescriptor fd, FileType fileType) : base(sessionId, privateAccessToken, fd)
        {
            FileType = fileType;
        }
    }

    internal class FileSender
    {

        private string _host;
        private int _port;
        private FileIndex _fileIndex;
        private NetworkQueue<ExtendedFilePutRequest> _putQueue;
        private NetworkQueue<ExtendedFileGetRequest> _getQueue;

        public FileSender(string host, int port, FileIndex fileIndex)
        {
            _host = host;
            _port = port;
            _fileIndex = fileIndex;
            _putQueue = new NetworkQueue<ExtendedFilePutRequest>(startSendFile);
            _getQueue = new NetworkQueue<ExtendedFileGetRequest>(startDownloadFile);
        }

        public void RequestSendFile(Guid sessionId, Guid privateAccessToken,
            FileDescriptor fd, FileType fileType, IProgress<double> sendProgress)
        {
            var request = new ExtendedFilePutRequest(sessionId, privateAccessToken, sendProgress, fd, fileType);
            _putQueue.Enqueue(request);
        }

        public FileProgress RequestRetrieveFile(Guid sessionId, Guid privateAccessToken,
            FileDescriptor fd, FileType fileType)
        {
            if (_fileIndex.FileExists(fd))
                return _fileIndex.GetFileDownloadCompletion(fd);

            FileProgress fp = _fileIndex.CreateFileProgress(fd);
            var request = new ExtendedFileGetRequest(sessionId, privateAccessToken, fd, fileType);
            _getQueue.Enqueue(request);

            return fp;
        }

        private async Task startSendFile(ExtendedFilePutRequest req)
        {
            using (FileStream fs = _fileIndex.StreamCompletedFile(req.SessionId, req.File, req.FileType))
            using (TcpClient tcpClient = new TcpClient())
            {
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

        private async Task startDownloadFile(ExtendedFileGetRequest req)
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
