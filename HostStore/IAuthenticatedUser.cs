using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HostStore
{
    internal interface IAuthenticatedUser
    {
        public Guid PrivateAccessKey { get; }

        public Guid UserId { get; }

        public bool Admin { get; }

        public Task PostMessageAsync(Type messageType, object message);

    }
}
