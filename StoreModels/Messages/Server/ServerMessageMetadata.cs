using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreModels.Messages.Server
{
    public class ServerMessageMetadata
    {

        public Guid MessageId { get; set; }

        public string MessageType { get; set; }

        public ServerMessageMetadata(Guid messageId, string messageType)
        {
            MessageId = messageId;
            MessageType = messageType;
        }

    }
}
