using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyStore.NetworkModels
{
    internal class FileDescriptor
    {

        public string Name { get; set; }

        public string Hash { get; set; }

        public string Descriptor => Name + Hash;

        public bool ClearAfterSession { get; set; }

        public Guid SessionId { get; set; }

    }
}
