using Models.File;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace SharedLib
{
    public class FileIndex
    {

        private ConcurrentDictionary<FileDescriptor, bool> _fileCompletedMap;

        public FileIndex()
        {
            _fileCompletedMap = new ConcurrentDictionary<FileDescriptor, bool>();
        }

        public bool FileExists(FileDescriptor fd, FileType fileType)
        {
            return _fileCompletedMap.ContainsKey(fd);
        }

        public bool FileCompleted(FileDescriptor fd, FileType fileType)
        {
            
            if (_fileCompletedMap.ContainsKey(fd) == false)
                return false;
            return _fileCompletedMap[fd];
        }

        public void AddFile(FileDescriptor fd, FileType fileType)
        {
            _fileCompletedMap[fd] = false;
        }

        public void SetFileCompleted(FileDescriptor fd, FileType fileType)
        {
            _fileCompletedMap[fd] = true;   
        }

    }
}
