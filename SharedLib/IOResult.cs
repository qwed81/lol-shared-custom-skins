using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLib
{
    public enum IOErrorType
    {
        None, ImproperFormat, IOError, NotAuthenticated, NoSession, FileError
    }

    public readonly struct IOResult<T>
    {

        public T Value { get; }
        public IOErrorType ErrorType { get; }
        public bool Failed { get; }

        internal IOResult(T value, IOErrorType errorType, bool failed)
        {
            Value = value;
            ErrorType = errorType;
            Failed = failed;
        }

    }

    public partial struct IOResult
    {
        public IOErrorType ErrorType { get; }
        public bool Failed { get; }

        internal IOResult(IOErrorType errorType, bool failed)
        {
            ErrorType = errorType;
            Failed = failed;
        }

    }

    public partial struct IOResult
    {

        public static IOResult<T> CreateFailure<T>(IOErrorType errorType)
        {
            return new IOResult<T>(default(T)!, errorType, true);
        }

        public static IOResult<T> CreateSuccess<T>(T value)
        {
            return new IOResult<T>(value, IOErrorType.None, false);
        }

        public static IOResult CreateFailure(IOErrorType errorType)
        {
            return new IOResult(errorType, true);
        }

        public static IOResult CreateSuccess()
        {
            return new IOResult(IOErrorType.None, false);
        }

    }


}
