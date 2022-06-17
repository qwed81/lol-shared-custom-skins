using System;
using System.Collections.Generic;
using System.Text;

namespace StoreModels.File
{
    public class FileIndexInitOptions
    {

        public string RootPath { get; set; } = "c://scs";

        public string ModPath { get; set; } = "/mods";

        public bool DeleteModsAfterClose { get; set; } = false;

        public string ProfilePicturePath { get; set; } = "/pfps";

        public bool DeleteProfilePicturesAfterClose { get; set; } = true;

    }
}
