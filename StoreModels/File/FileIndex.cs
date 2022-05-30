using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace StoreModels.File
{
    public class FileIndex
    {

        private ConcurrentDictionary<FileDescriptor, double> _filePercentages;
        private PathSelector _pathSelector;

        public FileIndex(PathSelector pathSelector)
        {
            _filePercentages = new ConcurrentDictionary<FileDescriptor, double>();
            _pathSelector = pathSelector;
        }

        public bool FileExists(FileDescriptor fd)
        {
            return _filePercentages.ContainsKey(fd);
        }

        public bool IsFileCompleted(FileDescriptor fd)
        {
            return _filePercentages[fd] == 1;
        }

        public double GetFileCompletion(FileDescriptor fd)
        {
            return _filePercentages[fd];
        }

        public FileStream StreamCreateFile(Guid sessionId, FileDescriptor fd, FileType fileType, out Action<double> setCompletion)
        {
            if (_filePercentages.ContainsKey(fd))
                throw new ArgumentException("File already exists");

            setCompletion = (prog) => _filePercentages[fd] = prog;
            return new FileStream(_pathSelector.GetPath(sessionId, fd, fileType), FileMode.CreateNew, FileAccess.Write, FileShare.None);
        }

        public FileStream StreamCompletedFile(Guid sessionId, FileDescriptor fd, FileType fileType)
        {
            if (!IsFileCompleted(fd))
                throw new ArgumentException("File download is not completed");

            return new FileStream(_pathSelector.GetPath(sessionId, fd, fileType), FileMode.Open, FileAccess.Read, FileShare.None);
        }


    }
}
