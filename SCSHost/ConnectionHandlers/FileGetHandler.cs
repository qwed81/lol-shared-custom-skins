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
    public class FileGetHandler : IConnectionHandler
    {
        
        private ConcurrentDictionary<Guid, Session> _sessions;
        private HostFileSender _fileSender;
        private PathSelector _pathSelector;
        private FileIndex _fileIndex;

        public FileGetHandler(ConcurrentDictionary<Guid, Session> sessions, HostFileSender fileSender, 
            PathSelector pathSelector, FileIndex fileIndex)
        {
            _sessions = sessions;
            _fileSender = fileSender;
            _pathSelector = pathSelector;
            _fileIndex = fileIndex;
        }

        public async Task<IOErrorType> HandleConnection(Guid sessionId, Connection connection)
        {
            var fileRequestResult = await _fileSender.ReadGetFileRequest(connection.Input);
            if (fileRequestResult.Failed)
                return fileRequestResult.ErrorType;

            FileGetRequest fileRequest = fileRequestResult.Value;

            lock(_sessions[sessionId].SessionLock) // makes sure user is authenticated to get file
            {
                var t = _sessions[sessionId].Connections.Where(c => c.PrivateAccessToken == fileRequest.PrivateAccessToken);
                if (t.Count() == 0)
                    return IOErrorType.NotAuthenticated;
            }

            if (_fileIndex.FileCompleted(fileRequest.File, fileRequest.FileType) == false)
                return IOErrorType.FileError;

            string path = _pathSelector.GetPath(sessionId, fileRequest.File, fileRequest.FileType);
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var responseResult = await _fileSender.SendGetFileResponse(connection.Output, fs.Length);
                    if (responseResult.Failed)
                        return responseResult.ErrorType;

                    var fileSendResult = await _fileSender.SendFile(connection.Output, fs, fs.Length, new Progress<double>());
                    return fileSendResult.ErrorType;
                }
            }
            catch (IOException)
            {
                return IOErrorType.FileError;
            }
        }

    }
}
