using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Network.Messages.Server
{

    public class FileReadyMessage
    {

        public string FileHash { get; set; }

        public FileReadyMessage(string fileHash)
        {
            FileHash = fileHash;
        }

    }
}
