using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Network.Messages.Client
{
    public class ModActivationMessage
    {

        public ModInfo ModInfo { get; set; }

        public bool Activate { get; set; }

        public ModActivationMessage(ModInfo modInfo, bool activate)
        {
            ModInfo = modInfo;
            Activate = activate;
        }

    }
}
