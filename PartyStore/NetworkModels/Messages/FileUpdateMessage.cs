using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyStore.NetworkModels
{
    internal class FileUpdateMessage
    {
        public static string MessageType = "FILE";

        public FileDescriptor FileDescriptor { get; set; }

        public FileUpdateMessage(FileDescriptor fileDescriptor)
        {
            FileDescriptor = fileDescriptor;
        }

    }
}
