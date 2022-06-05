using System;
using System.Collections.Generic;
using System.Text;

namespace ClientStore.Network
{
    internal interface IClientMessageHandler
    {

        public void HandleMessage(Type messageType, object message);


    }
}
