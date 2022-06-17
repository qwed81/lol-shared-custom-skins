using SharedLib;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSHost.ConnectionHandlers
{
    public interface IConnectionHandler
    {

        public Task<IOErrorType> HandleConnection(Guid sessionId, Connection connection); 

    }
}
