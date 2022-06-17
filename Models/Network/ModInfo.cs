using Models.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Network
{ 
    public class ModInfo
    {

        public string Name { get; set; }

        public string Description { get; set; }

        public FileDescriptor ModFile { get; set; }

        public Guid ModId { get; set; }

        public ModInfo(string name, string description, FileDescriptor modFile, Guid modId)
        {
            Name = name;
            Description = description;
            ModFile = modFile;
            ModId = modId;
        }
    }
}
