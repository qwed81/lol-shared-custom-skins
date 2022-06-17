using Models.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Network.Messages.Server
{

    public class FileReadyMessage
    {

        public FileType FileType { get; set; }

        public FileDescriptor FileDescriptor { get; set; }

        public FileReadyMessage(FileDescriptor fileDescriptor, FileType fileType)
        {
            FileDescriptor = fileDescriptor;
            FileType = fileType;
        }

    }
}
