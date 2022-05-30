using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyStore.NetworkModels
{
    public class ClientMessageMetadata
    {

        public Guid UserId { get; set; }

        public Guid SessionId { get; set; }

        public Guid MessageId { get; set; }

        public string MessageType { get; set; }

        public ClientMessageMetadata(Guid sessionId, Guid userId, Guid messageId, string messageType)
        {
            UserId = userId;
            SessionId = sessionId;
            MessageId = messageId;
            MessageType = messageType;
        }

    }
}
