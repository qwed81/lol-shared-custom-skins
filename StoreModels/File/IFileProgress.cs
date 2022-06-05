using System;
using System.Collections.Generic;
using System.Text;

namespace StoreModels.File
{
    public interface IFileProgress
    {

        public double Progress { get; }

        public bool IsCompleted { get; }

        public event Action<FileProgress>? ProgressChanged;

    }
}
