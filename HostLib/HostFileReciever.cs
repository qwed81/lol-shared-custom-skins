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
    public class HostFileReciever
    {

        public async Task<IOResult<FilePutRequest>> ReadFilePutRequest(AugmentedInputStream input)
        {
            return await input.ReadObjectAsync<FilePutRequest>();
        }

        public async Task<IOResult> SendFilePutResponse(AugmentedOutputStream output, bool shouldUploadFile)
        {
            var response = new FilePutResponse(shouldUploadFile);
            return await output.WriteObjectsAsync(response);
        }

        public async Task<IOResult> DownloadFile(AugmentedInputStream input, FileStream output,
            long fileLength, IProgress<double> progress)
        {
            return await input.ReadFileAsync(output, fileLength, progress);
        }

    }
}
