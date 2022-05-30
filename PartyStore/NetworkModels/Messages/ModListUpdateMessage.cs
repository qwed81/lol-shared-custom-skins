using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyStore.NetworkModels.Messages
{
    internal class ModListUpdateMessage
    {

        public static string MessageType = "MODS";

        public List<ModInfo> ModList { get; set; }

        public ModListUpdateMessage(List<ModInfo> modList)
        {
            ModList = modList;
        }

    }
}
