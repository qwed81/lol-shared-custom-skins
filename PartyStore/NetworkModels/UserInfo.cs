using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyStore.NetworkModels
{
    internal class UserInfo
    {

        public string Name { get; set; }

        public string Status { get; set; }

        public FileDescriptor Image { get; set; }

        public Guid? UserSessionId { get; set; } // can be ignored when sent by client

    }
}
