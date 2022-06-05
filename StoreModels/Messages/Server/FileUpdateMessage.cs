using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreModels.Messages.Server
{

    public class FileUpdateMessage
    {

        public FileType FileType { get; set; }

        public FileDescriptor FileDescriptor { get; set; }

        public FileUpdateMessage(FileDescriptor fileDescriptor, FileType fileType)
        {
            FileDescriptor = fileDescriptor;
            FileType = fileType;
        }

    }
}
