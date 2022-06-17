using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Network.Messages.Server
{
    public class FileGetResponse
    {
        public long FileSize { get; set; }

        public FileGetResponse(long fileSize)
        {
            FileSize = fileSize;
        }


    }
}
