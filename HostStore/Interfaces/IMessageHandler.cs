using StoreModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HostStore
{
    internal interface IMessageHandler
    {

        public void HandleMessage(IAuthenticatedUser user, Type messageType, object message);

        public void HandleUserAdded(IAuthenticatedUser user);

        public void HandleUserRemoved(IAuthenticatedUser user);

    }
}
