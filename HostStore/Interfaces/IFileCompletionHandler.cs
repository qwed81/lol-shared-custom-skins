using StoreModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace HostStore
{
    internal interface IFileCompletionHandler
    {

        public void HandleFileCompleted(FileDescriptor fd);

    }
}
