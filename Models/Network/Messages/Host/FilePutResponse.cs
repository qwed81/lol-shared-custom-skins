using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Network.Messages.Server
{
    public class FilePutResponse
    {

        public bool ShouldUpload { get; set; }

        public FilePutResponse(bool shouldUpload)
        {
            ShouldUpload = shouldUpload;
        }

    }
}
