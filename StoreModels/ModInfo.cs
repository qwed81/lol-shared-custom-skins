using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreModels
{ 
    public class ModInfo
    {

        public string Name { get; }

        public string Description { get; }

        public FileDescriptor ModFile { get; }

        public ModInfo(string name, string description, FileDescriptor modFile)
        {
            Name = name;
            Description = description;
            ModFile = modFile;
        }
    }
}
