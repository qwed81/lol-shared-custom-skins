using Models.Network.Messages.Client;
using Models.Network.Messages.Server;
using SharedLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace HostLib
{
    public class HostFileSender
    {

        public async Task<IOResult<FileGetRequest>> ReadGetFileRequest(AugmentedInputStream input)
        {
            return await input.ReadObjectAsync<FileGetRequest>();
        }

        public async Task<IOResult> SendGetFileResponse(AugmentedOutputStream output, long fileLength)
        {
            var response = new FileGetResponse(fileLength);
            return await output.WriteObjectsAsync(response);
        }

        public async Task<IOResult> SendFile(AugmentedOutputStream output, FileStream input,
            long fileLength, IProgress<double> progress)
        {
            return await output.WriteFileAsync(input, fileLength, progress);
        }

    }
}
