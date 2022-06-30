using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SharedLib
{
    public class PathSelector
    {

        private string _root;
        
        public PathSelector(string root)
        {
            _root = root;
        }

        public bool FileExists(string fileHash)
        {
            throw new NotImplementedException();
        }

        public FileStream GetFile(string fileHash)
        {
            throw new NotImplementedException();
        }

        public FileStream CreateFile(string fileHash)
        {
            throw new NotImplementedException();
        }

        public void RemoveFile(string fileHash)
        {
            throw new NotImplementedException();
        }

    }
}
