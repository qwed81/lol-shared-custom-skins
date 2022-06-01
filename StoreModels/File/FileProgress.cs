using System;
using System.Collections.Generic;
using System.Text;

namespace StoreModels.File
{
    public class FileProgress : IProgress<double>
    {

        public FileDescriptor File { get; }

        public double Progress { get; private set; }

        public bool IsCompleted => Progress == 1;

        public event Action<FileProgress>? ProgressChanged;

        public FileProgress(FileDescriptor file)
        {
            Progress = 0;
            File = file;
        }

        public void Report(double percentage)
        {
            Progress = percentage;
            ProgressChanged?.Invoke(this);
        }

        public void SetCompleted()
        {
            Progress = 1;
        }
    }
}
