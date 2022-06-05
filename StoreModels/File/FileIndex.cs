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
        
        public PathSelector PathSelector { get; }

        public FileIndex(PathSelector pathSelector)
        {
            _fileProgresses = new ConcurrentDictionary<FileDescriptor, FileProgress>();
            PathSelector = pathSelector;
        }

        public bool FileExists(FileDescriptor fd)
        {
            return _fileProgresses.ContainsKey(fd);
        }

        public FileProgress GetFileDownloadProgress(FileDescriptor fd)
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

            setCompletion = GetFileDownloadProgress(fd);
            return new FileStream(PathSelector.GetPath(sessionId, fd, fileType),
                FileMode.CreateNew, FileAccess.Write, FileShare.None);
        }

        public FileStream? StreamCompletedFile(Guid sessionId, FileDescriptor fd, FileType fileType)
        {
            if (GetFileDownloadProgress(fd).IsCompleted == false)
                return null;

            try
            {
                return new FileStream(PathSelector.GetPath(sessionId, fd, fileType),
                    FileMode.Open, FileAccess.Read, FileShare.Read);
            } 
            catch (IOException)
            {
                return null;
            }
        }


    }
}
