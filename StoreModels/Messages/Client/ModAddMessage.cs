﻿using System;
using System.Collections.Generic;
using System.Text;

namespace StoreModels.Messages.Client
{
    public class ModAddMessage
    {

        public ModInfo ModInfo { get; set; }

        public ModAddMessage(ModInfo modInfo)
        {
            ModInfo = modInfo;
        }

    }
}
