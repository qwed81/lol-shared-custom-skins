using System;
using System.Collections.Generic;
using System.Text;

namespace StoreModels.Messages.Server
{
    public class FileGetResponse : ServerResponse
    {
        public long FileSize { get; set; }

        public FileGetResponse(long fileSize)
        {
            FileSize = fileSize;
        }


    }
}
