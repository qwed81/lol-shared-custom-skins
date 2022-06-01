using System;
using System.Collections.Generic;
using System.Text;

namespace StoreModels.Messages.Server
{
    internal class FilePutResponse
    {

        public bool Success { get; set; }

        public string? FailureReason { get; set; }

        public static FilePutResponse CreateFailure(string reason)
        {
            return new FilePutResponse
            {
                Success = false,
                FailureReason = reason
            };
        }

        public static FilePutResponse CreateSuccess()
        {
            return new FilePutResponse
            {
                Success = true
            };
        }


    }
}
