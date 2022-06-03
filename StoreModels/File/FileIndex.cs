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

        public FileProgress CreateFileProgress(FileDescriptor fd)
        {
            if (_fileProgresses.ContainsKey(fd))
                throw new ArgumentException("File already exists");

            var progress = new FileProgress(fd);
            _fileProgresses[fd] = progress;

            return progress;
        }

        public FileStream StreamCreateFile(Guid sessionId, FileDescriptor fd, FileType fileType,
            out FileProgress setCompletion)
        {
            if (_fileProgresses.ContainsKey(fd) == false)
                CreateFileProgress(fd);

            setCompletion = GetFileDownloadCompletion(fd);
            return new FileStream(_pathSelector.GetPath(sessionId, fd, fileType),
                FileMode.CreateNew, FileAccess.Write, FileShare.None);
        }

        public FileStream? StreamCompletedFile(Guid sessionId, FileDescriptor fd, FileType fileType)
        {
            if (GetFileDownloadCompletion(fd).IsCompleted == false)
                return null;

            try
            {
                return new FileStream(_pathSelector.GetPath(sessionId, fd, fileType),
                    FileMode.Open, FileAccess.Read, FileShare.Read);
            } 
            catch (IOException)
            {
                return null;
            }
        }


    }
}
