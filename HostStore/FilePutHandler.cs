using StoreModels;
using StoreModels.File;
using StoreModels.Messages.Client;
using StoreModels.Messages.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace HostStore
{

    internal class FilePutHandler
    {

        private FileIndex _fileIndex;

        public FilePutHandler(FileIndex fileIndex)
        {
            _fileIndex = fileIndex;
        }

        public async Task HandleFilePutRequestAsync(TcpClientWrapper client, FilePutRequest req, ISessionAuth auth,
            IFileCompletionHandler fileCompletedHandler)
        {
            if (auth.CanUserWriteFiles(req.PrivateAccessToken) == false)
            {
                var res = ServerResponse.CreateFailureResponse(RequestType.FILE_PUT, "User is not authenticated");
                await client.WriteObjectAsync(res);
            }

            FileProgress fp;
            FileStream file = _fileIndex.StreamCreateFile(auth.SessionId, req.FileDescriptor, req.FileType, out fp);
            
            fp.ProgressChanged += (fp) => // notifies handler on file completion
            {
                if (fp.IsCompleted == false)
                    return;

                fileCompletedHandler.HandleFileCompleted(fp.File);
            };

            await client.WriteObjectAsync(new FilePutResponse());
            await client.DownloadFileAsync(file, req.FileLength, fp);

        }
    }
}
