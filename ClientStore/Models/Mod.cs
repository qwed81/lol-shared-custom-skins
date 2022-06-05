using StoreModels;
using StoreModels.File;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientStore.Models
{
    public class Mod
    {

        public string Name { get; }

        public string Description { get; }

        public IFileProgress FileProgress { get; }

        public string Path { get; }

        internal Guid Id { get; }

        internal Mod(string name, string description, IFileProgress fileProgress, string path, Guid id)
        {
            Name = name;
            Description = description;
            FileProgress = fileProgress;
            Path = path;
            Id = id;
        }

    }
}
