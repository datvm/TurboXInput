using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace TurboXInput.Core
{
    public class TurboInputException : Exception
    {
        public TurboInputExceptionCode ErrorCode { get; private set; }

        public TurboInputException(TurboInputExceptionCode errorCode)
        {
            this.ErrorCode = errorCode;
        }

        public TurboInputException(TurboInputExceptionCode errorCode, string message) : base(message)
        {
            this.ErrorCode = errorCode;
        }

        public TurboInputException(TurboInputExceptionCode errorCode, string message, Exception innerException) : base(message, innerException)
        {
            this.ErrorCode = errorCode;
        }

        public override string ToString()
        {
            return $"{(int) this.ErrorCode} {this.ErrorCode.ToString()}";
        }
    }

    public enum TurboInputExceptionCode
    {
        DeviceNotConnected = 1,
        UnexpectedError = 999,
    }
}
