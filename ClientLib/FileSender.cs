using ClientLib.Models;
using Models.File;
using Models.Network.Messages.Client;
using Models.Network.Messages.Server;
using SharedLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ClientLib
{
    public class FileSender
    {

        public async Task<IOResult> RequestPutFile(AugmentedOutputStream output, ClientInfo clientInfo,
            FileDescriptor fd, FileType fileType, long fileLength)
        {
            var request = new FilePutRequest(clientInfo.PrivateAccessToken, fd, fileType, fileLength);
            return await output.WriteObjectsAsync(request);
        }

        public async Task<IOResult<FilePutResponse>> ReadPutFileResponse(AugmentedInputStream input)
        {
            return await input.ReadObjectAsync<FilePutResponse>();
        }

        public async Task<IOResult> UploadFile(AugmentedOutputStream output, FileStream input, long fileLength,
            IProgress<double> progress)
        {
            return await output.WriteFileAsync(input, fileLength, progress);
        }

    }
}
