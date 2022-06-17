using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Network.Messages.Server
{
    public class ServerMessageMetadata
    {

        public string MessageType { get; set; }

        public ServerMessageMetadata(string messageType)
        {
            MessageType = messageType;
        }

    }
}
