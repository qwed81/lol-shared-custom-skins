using StoreModels;
using StoreModels.Messages.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HostStore
{
    internal interface IConnectionEstablishedHandler
    {

        public Task HandleEstablishedConnection(TcpClientWrapper client, ClientRequest request);

    }
}
