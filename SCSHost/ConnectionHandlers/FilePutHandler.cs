using HostLib;
using HostLib.Models;
using Models.Network.Messages.Client;
using SharedLib;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSHost.ConnectionHandlers
{
    public class FilePutHandler : IConnectionHandler
    {

        private ConcurrentDictionary<Guid, Session> _sessions;
        private HostFileReciever _fileReciever;
        private PathSelector _pathSelector;
        private FileIndex _fileIndex;

        public FilePutHandler(ConcurrentDictionary<Guid, Session> sessions, HostFileReciever fileReciever,
            PathSelector pathSelector, FileIndex fileIndex)
        {
            _sessions = sessions;
            _fileReciever = fileReciever;
            _pathSelector = pathSelector;
            _fileIndex = fileIndex;
        }

        public async Task<IOErrorType> HandleConnection(Guid sessionId, Connection connection)
        {
            var fileRequestResult = await _fileReciever.ReadFilePutRequest(connection.Input);
            if (fileRequestResult.Failed)
                return fileRequestResult.ErrorType;

            FilePutRequest fileRequest = fileRequestResult.Value;

            lock (_sessions[sessionId].SessionLock) // makes sure user is authenticated to get file
            {
                var t = _sessions[sessionId].Connections.Where(c => c.PrivateAccessToken == fileRequest.PrivateAccessToken);
                if (t.Count() == 0)
                    return IOErrorType.NotAuthenticated;
            }

            if(_fileIndex.FileExists(fileRequest.FileDescriptor, fileRequest.FileType) == true)
            {
                var responseResult = await _fileReciever.SendFilePutResponse(connection.Output, false);
                return responseResult.ErrorType;
            }
            _fileIndex.AddFile(fileRequest.FileDescriptor, fileRequest.FileType);

            string path = _pathSelector.GetPath(sessionId, fileRequest.FileDescriptor, fileRequest.FileType);
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    var responseResult = await _fileReciever.SendFilePutResponse(connection.Output, true);
                    if (responseResult.Failed)
                        return responseResult.ErrorType;

                    var fileRecieveResult = await _fileReciever.DownloadFile(connection.Input, fs, fs.Length, new Progress<double>());

                    if(fileRecieveResult.Failed == false)
                    {
                        _fileIndex.SetFileCompleted(fileRequest.FileDescriptor, fileRequest.FileType);
                    }

                    return fileRecieveResult.ErrorType;
                }
            }
            catch (IOException)
            {
                return IOErrorType.FileError;
            }

        }
    }
}
