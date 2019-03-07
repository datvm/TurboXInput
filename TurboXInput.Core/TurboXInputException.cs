using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace TurboXInput.Core
{
    public class TurboXInputException : Exception
    {
        public TurboXInputExceptionCode ErrorCode { get; private set; }

        public TurboXInputException(TurboXInputExceptionCode errorCode)
        {
            this.ErrorCode = errorCode;
        }

        public TurboXInputException(TurboXInputExceptionCode errorCode, string message) : base(message)
        {
            this.ErrorCode = errorCode;
        }

        public TurboXInputException(TurboXInputExceptionCode errorCode, string message, Exception innerException) : base(message, innerException)
        {
            this.ErrorCode = errorCode;
        }

        public override string ToString()
        {
            return $"{(int) this.ErrorCode} {this.ErrorCode.ToString()}";
        }
    }

    public enum TurboXInputExceptionCode
    {
        DeviceNotConnected = 1,
        FailedToAcquireVJoy = 2,
        UnexpectedError = 999,
    }
}
