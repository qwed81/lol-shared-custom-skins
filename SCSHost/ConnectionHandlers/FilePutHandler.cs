using HostLib;
using HostLib.Models;
using Models.Network;
using Models.Network.Messages.Client;
using Models.Network.Messages.Server;
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

        private HostFileReciever _fileReciever;
        private PathSelector _pathSelector;
        private HostState _state;

        public FilePutHandler(HostState state, HostFileReciever fileReciever, PathSelector pathSelector)
        {
            _state = state;
            _fileReciever = fileReciever;
            _pathSelector = pathSelector;
        }

        public async Task<IOErrorType> HandleConnection(Connection connection)
        {
            var fileRequestResult = await _fileReciever.ReadFilePutRequest(connection.Input);
            if (fileRequestResult.Failed)
                return fileRequestResult.ErrorType;

            FilePutRequest fileRequest = fileRequestResult.Value;
            string fileHash = fileRequest.FileHash;

            if (_state.GetExistingMessageChanel(fileRequest.PrivateAccessToken) == null)
                return IOErrorType.NotAuthenticated;

            if (_state.ModExists(fileHash)) // don't put mod, already has
                return IOErrorType.None;

            if (_pathSelector.FileExists(fileHash)) // file already there, currently being uploaded
                return IOErrorType.FileError;

            var responseResult = await _fileReciever.SendFilePutResponse(connection.Output, true);
            if (responseResult.Failed)
                return responseResult.ErrorType;

            using (FileStream fs = _pathSelector.CreateFile(fileHash))
            {
                IOResult downloadResult = await _fileReciever.DownloadFile(connection.Input, fs, fileRequest.FileLength, new Progress<double>());

                if (downloadResult.Failed)
                {
                    _pathSelector.RemoveFile(fileHash);
                    return downloadResult.ErrorType;
                }
            }

            // let the state know this file has been downloaded successfully
            _state.CompletedFiles[fileHash] = true;

            var message = new FileReadyMessage(fileHash);
            var pair = new TypeMessagePair(typeof(FileReadyMessage), message);
            _state.OutboundMessages.Add(pair);

            return IOErrorType.None;
        }
    }
}
