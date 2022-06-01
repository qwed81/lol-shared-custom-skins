using System;
using System.Collections.Generic;
using System.Text;

namespace StoreModels.Messages.Server
{
    public class FileGetResponse
    {

        public bool Success { get; set; }

        public long FileSize { get; set; }

        public string? FailureReason { get; set; }

        public static FileGetResponse CreateFailure(string reason)
        {
            return new FileGetResponse
            {
                Success = false,
                FailureReason = reason
            };
        }

        public static FileGetResponse CreateSuccess(long fileSize)
        {
            return new FileGetResponse 
            {
                Success = true,
                FileSize = fileSize
            };
        }


    }
}
