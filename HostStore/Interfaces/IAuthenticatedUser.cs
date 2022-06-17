using StoreModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HostStore
{
    public interface IAuthenticatedUser
    {
        public Guid UserId { get; }

        public Guid PrivateAccessKey { get; }

        public bool Admin { get; }

    }
}
