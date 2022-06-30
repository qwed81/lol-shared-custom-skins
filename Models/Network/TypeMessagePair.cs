using Models.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Network
{
    public struct TypeMessagePair
    {

        public Type MessageType { get; }

        public object Message { get; }

        public TypeMessagePair(Type messageType, object message)
        {
            MessageType = messageType;
            Message = message;
        }

    }
}
