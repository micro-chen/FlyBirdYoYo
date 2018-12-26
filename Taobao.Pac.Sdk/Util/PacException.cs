using System;
using System.Runtime.Serialization;

namespace Taobao.Pac.Sdk.Core
{
    /// <summary>
    /// Pac 客户端异常。
    /// </summary>
    public class PacException : Exception
    {
        private string errorCode;
        private string errorMsg;
        private string subErrorCode;
        private string subErrorMsg;

        public PacException()
            : base()
        {
        }

        public PacException(string message)
            : base(message)
        {
        }

        protected PacException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public PacException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public PacException(string errorCode, string errorMsg)
            : base(errorCode + ":" + errorMsg)
        {
            this.errorCode = errorCode;
            this.errorMsg = errorMsg;
        }

        public PacException(string errorCode, string errorMsg, string subErrorCode, string subErrorMsg)
            : base(errorCode + ":" + errorMsg + ":" + subErrorCode + ":" + subErrorMsg)
        {
            this.errorCode = errorCode;
            this.errorMsg = errorMsg;
            this.subErrorCode = subErrorCode;
            this.subErrorMsg = subErrorMsg;
        }

        public string ErrorCode
        {
            get { return this.errorCode; }
        }

        public string ErrorMsg
        {
            get { return this.errorMsg; }
        }

        public string SubErrorCode
        {
            get { return this.subErrorCode; }
        }

        public string SubErrorMsg
        {
            get { return this.subErrorMsg; }
        }


    }
}
