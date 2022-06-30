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

        public string Author { get; set; }

        public string ProviderName { get; set; }

        public string FileHash { get; set; }

        public ModInfo(string name, string description, string author, string providerName, string fileHash)
        {
            Name = name;
            Description = description;
            Author = author;
            ProviderName = providerName;
            FileHash = fileHash;
        }
    }
}
