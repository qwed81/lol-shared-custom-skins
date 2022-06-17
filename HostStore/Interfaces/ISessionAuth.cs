using System;
using System.Collections.Generic;
using System.Text;

namespace HostStore
{
    internal interface ISessionAuth
    {

        public Guid SessionId { get; }

        public bool CanUserReadFiles(Guid userPrivateKey);

        public bool CanUserWriteFiles(Guid userPrivateKey);

    }
}
