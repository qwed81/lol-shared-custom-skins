using Models.File;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SharedLib
{
    public class PathSelector
    {

        private string _root;
        private Dictionary<FileType, string> _pathNames;
        private Dictionary<FileType, bool> _sessionSpecific;

        public PathSelector(string root, Dictionary<FileType, string> pathNames,
            Dictionary<FileType, bool> sessionSpecific)
        {
            _root = root;
            _pathNames = pathNames;
            _sessionSpecific = sessionSpecific;
        }

        public string GetPathPrefix(Guid sessionId, FileType fileType)
        {
            if (_sessionSpecific[fileType])
                return Path.Combine(_root, "sessions", sessionId.ToString(), _pathNames[fileType]);
            else
                return Path.Combine(_root, _pathNames[fileType]);
        }

        public string GetPath(Guid sessionId, FileDescriptor fd, FileType fileType)
        {
            return Path.Combine(GetPathPrefix(sessionId, fileType), fd.ToString());
        }

        public string[] GetSessionPaths(Guid sessionId, FileType fileType)
        {
            if (!_sessionSpecific[fileType])
                throw new ArgumentException("Can not get session paths of not session specific files");
            return Directory.GetFiles(Path.Combine(_root, "sessions",
                sessionId.ToString(), _pathNames[fileType]));
        }

    }
}
