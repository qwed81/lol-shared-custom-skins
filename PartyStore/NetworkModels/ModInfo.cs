using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyStore.NetworkModels
{
    internal class ModInfo
    {

        public string Name { get; set; }

        public string Description { get; set; }

        public FileDescriptor Image { get; set; }

        public FileDescriptor ModFile { get; set; }

    }
}
