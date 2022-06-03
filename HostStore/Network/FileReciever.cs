﻿using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Threading.Tasks;

using StoreModels;
using StoreModels.File;
using StoreModels.Messages;
using StoreModels.Messages.Client;
using StoreModels.Messages.Server;

namespace HostStore.Network
{
    internal class FileReciever
    {

        private FileIndex _fileIndex;

        public FileReciever(FileIndex fileIndex)
        {
            _fileIndex = fileIndex;
        }


        public async Task HandleFileGetRequestAsync(TcpClientWrapper client, FileGetRequest? req, Session session)
        {
            if (req == null)
                throw new Exception("Improper request");

            if (session.UserIsAuthenticated(req.PrivateAccessToken) == false)
            {
                var res = ServerResponse.CreateFailureResponse(RequestType.FILE_GET, "User is not authenticated");
                await client.WriteObjectAsync(res);
            }

            FileStream? file = _fileIndex.StreamCompletedFile(req.SessionId, req.File, req.FileType);
            if(file == null)
            {
                var res = ServerResponse.CreateFailureResponse(RequestType.FILE_GET, "File does not exist or is not completed");
                await client.WriteObjectAsync(res);
                return;
            }

            await client.WriteObjectAsync(new FileGetResponse(file.Length));
            await client.WriteFileAsync();

        }

        public async Task HandleFilePutRequestAsync(TcpClientWrapper client, FilePutRequest? req, Session session)
        {
            if (req == null)
                throw new Exception("Improper request");

            if (session.UserIsAuthenticated(req.PrivateAccessToken) == false)
            {
                var res = ServerResponse.CreateFailureResponse(RequestType.FILE_PUT, "User is not authenticated");
                await client.WriteObjectAsync(res);
            }



        }

    }
}
