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
    public class FileReciever
    {

        public async Task<IOResult> RequestGetFile(AugmentedOutputStream output, ClientInfo clientInfo,
            FileDescriptor fd, FileType fileType)
        {
            var request = new FileGetRequest(clientInfo.PrivateAccessToken, fd, fileType);
            return await output.WriteObjectsAsync(request);
        }

        public async Task<IOResult<FileGetResponse>> ReadGetFileResponse(AugmentedInputStream input)
        {
            return await input.ReadObjectAsync<FileGetResponse>();
        }

        public async Task<IOResult> DownloadFile(AugmentedInputStream input, FileStream output, 
            long fileLength, IProgress<double> progress)
        {
            return await input.ReadFileAsync(output, fileLength, progress);
        }

    }
}
