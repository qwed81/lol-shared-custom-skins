using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreModels
{
    public class FileDescriptor
    {

        public string Name { get; }

        public string Hash { get; }

        public FileDescriptor(string name, string hash)
        {
            Name = name;
            Hash = hash;
        }

        public static FileDescriptor FromSimplifiedFileName(string fileName)
        {
            string[] strSplit = fileName.Split('_');
            string name = strSplit[0];
            string hash = strSplit[1];

            return new FileDescriptor(name, hash);
        }

        public override string ToString()
        {
            return Hash + '_' + Name;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return ToString().Equals(obj.ToString());
        }

    }
}
