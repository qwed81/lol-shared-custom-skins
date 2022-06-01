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

        private ConcurrentDictionary<FileDescriptor, FileProgress> _fileProgresses;
        private PathSelector _pathSelector;

        public FileIndex(PathSelector pathSelector)
        {
            _fileProgresses = new ConcurrentDictionary<FileDescriptor, FileProgress>();
            _pathSelector = pathSelector;
        }

        public bool FileExists(FileDescriptor fd)
        {
            return _fileProgresses.ContainsKey(fd);
        }

        public FileProgress GetFileDownloadCompletion(FileDescriptor fd)
        {
            return _fileProgresses[fd];
        }

        public FileStream StreamCreateFile(Guid sessionId, FileDescriptor fd, FileType fileType,
            out IProgress<double> setCompletion)
        {
            if (_fileProgresses.ContainsKey(fd))
                throw new ArgumentException("File already exists");

            setCompletion = GetFileDownloadCompletion(fd);
            return new FileStream(_pathSelector.GetPath(sessionId, fd, fileType),
                FileMode.CreateNew, FileAccess.Write, FileShare.None);
        }

        public FileStream StreamCompletedFile(Guid sessionId, FileDescriptor fd, FileType fileType)
        {
            if (GetFileDownloadCompletion(fd).IsCompleted == false)
                throw new ArgumentException("File download is not completed");

            return new FileStream(_pathSelector.GetPath(sessionId, fd, fileType),
                FileMode.Open, FileAccess.Read, FileShare.None);
        }


    }
}
