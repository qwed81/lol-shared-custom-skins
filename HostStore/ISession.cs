using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HostStore
{

    internal delegate void MessageHandler(Session session, IAuthenticatedUser user, Type messageType, object message);

    internal interface ISession : IEnumerable<IAuthenticatedUser>
    {

        public event MessageHandler? OnMessage;

        public Task PostMessageToAllAsync(Type messageType, object message);

    }
}
