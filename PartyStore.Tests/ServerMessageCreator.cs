using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyStore.Tests
{
    internal class ServerMessageCreator
    {

        private Guid _userId;
        private Guid _sessionId;

        public ServerMessageCreator(Guid sessionId, Guid userId)
        {
            _userId = userId;
            _sessionId = sessionId;
        }






    }
}
