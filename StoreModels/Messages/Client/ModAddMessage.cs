using System;
using System.Collections.Generic;
using System.Text;

namespace StoreModels.Messages.Client
{
    public class ModAddMessage
    {

        public ModInfo Mod { get; set; }

        public ModAddMessage(ModInfo mod)
        {
            Mod = mod;
        }

    }
}
