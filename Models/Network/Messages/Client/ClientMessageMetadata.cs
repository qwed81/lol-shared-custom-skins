using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Network.Messages.Client
{
    public class ClientMessageMetadata
    {

        public string MessageType { get; set; }

        public ClientMessageMetadata(string messageType)
        {
            MessageType = messageType;
        }

    }
}
