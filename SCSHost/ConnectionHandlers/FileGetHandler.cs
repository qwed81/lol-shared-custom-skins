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
        
        private HostFileSender _fileSender;
        private PathSelector _pathSelector;
        private HostState _state;

        public FileGetHandler(HostState hostState, HostFileSender fileSender, PathSelector pathSelector)
        {
            _state = hostState;
            _fileSender = fileSender;
            _pathSelector = pathSelector;
        }

        public async Task<IOErrorType> HandleConnection(Connection connection)
        {
            var fileRequestResult = await _fileSender.ReadGetFileRequest(connection.Input);
            if (fileRequestResult.Failed)
                return fileRequestResult.ErrorType;

            FileGetRequest fileRequest = fileRequestResult.Value;
            string fileHash = fileRequest.FileHash;

            if (_state.GetExistingMessageChanel(fileRequest.PrivateAccessToken) == null)
                return IOErrorType.NotAuthenticated;

            if (_state.ModExists(fileHash) == false)
                return IOErrorType.FileError;

            if (_state.CompletedFiles[fileHash] == false)
                return IOErrorType.FileError;

            if (_pathSelector.FileExists(fileHash) == false) // should never happen, data is corrupted
                return IOErrorType.FileError;

            
            using (FileStream fs = _pathSelector.GetFile(fileHash))
            {
                var responseResult = await _fileSender.SendGetFileResponse(connection.Output, fs.Length);
                if(responseResult.Failed)
                    return responseResult.ErrorType;

                var fileSendResult = await _fileSender.SendFile(connection.Output, fs, fs.Length, new Progress<double>());
                return fileSendResult.ErrorType;
            }
            
        }

    }
}
