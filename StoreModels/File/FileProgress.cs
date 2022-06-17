using System;
using System.Collections.Generic;
using System.Text;

namespace StoreModels.File
{
    public class FileProgress : IProgress<double>, IFileProgress
    {

        public FileDescriptor File { get; }

        public double Progress { get; private set; }

        public bool IsCompleted { get; private set; }

        public event Action<FileProgress>? ProgressChanged;

        public FileProgress(FileDescriptor file)
        {
            Progress = 0;
            File = file;
        }

        public void Report(double percentage)
        {
            if (IsCompleted)
                throw new InvalidOperationException("Can not report of a completed file");

            Progress = percentage;
            ProgressChanged?.Invoke(this);

            if (Progress >= 1)
            {
                SetCompleted();
            }
        }

        public void SetCompleted()
        {
            Progress = 1;
            ProgressChanged = null;
            IsCompleted = true;
        }
    }
}
