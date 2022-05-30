using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreModels.Messages.Server
{
    public class ModListUpdateMessage
    {

        public List<ModInfo> ModList { get; set; }

        public ModListUpdateMessage(List<ModInfo> modList)
        {
            ModList = modList;
        }

    }
}
