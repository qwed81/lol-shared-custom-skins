using Models.File;
using Models.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Network
{
    public struct MessageTypePair
    {

        public Type MessageType { get; }

        public object Message { get; }

        public MessageTypePair(Type messageType, object message)
        {
            MessageType = messageType;
            Message = message;
        }

    }
}
