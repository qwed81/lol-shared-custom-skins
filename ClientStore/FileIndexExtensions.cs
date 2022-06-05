using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using StoreModels;
using StoreModels.File;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace ClientStore
{
    internal static class FileIndexExtensions
    {

        public static FileDescriptor? GetFileDescriptorIfExists(this FileIndex fileIndex, string filePath)
        {
            FileDescriptor fd = FileDescriptor.FromSimplifiedFileName(filePath);
            if (fileIndex.FileExists(fd))
                return fd;

            return null;
        }

        public static Task<FileDescriptor> AddExternalFileToIndexAsync(this FileIndex fileIndex, string filePath,
            Guid sessionId, FileType fileType, Action<IFileProgress> outProgress)
        {
            return Task.Run(async () =>
            {
                string pathPrefix = fileIndex.PathSelector.GetPathPrefix(sessionId, fileType);
                string name = Path.GetFileName(filePath);

                string hash;
                using (var md5 = MD5.Create())
                using (var fileStream = File.OpenRead(filePath))
                {
                    byte[] hashBytes = md5.ComputeHash(fileStream);
                    hash = BitConverter.ToString(hashBytes).Replace('-', ' ');
                }

                FileDescriptor fd = new FileDescriptor(name, hash);
                string newPath = Path.Combine(pathPrefix, fd.ToString());

                FileProgress fileProgress;
                using (var inputStream = File.OpenRead(filePath))
                using (var outputStream = fileIndex.StreamCreateFile(sessionId, fd, fileType, out fileProgress))
                using (var streamWrapper = new StreamWrapper(inputStream))
                {
                    outProgress(fileProgress);
                    await streamWrapper.WriteFileAsync(outputStream, inputStream.Length, fileProgress);
                }

                return fd;

            });

        }

    }
}
