using System;

namespace XiangJiang.Windows.Core
{
    [Serializable]
    public sealed class WindowsException : Exception
    {
        public readonly WindowsErrorCode ErrorCode;

        public WindowsException(WindowsErrorCode errorCode, string errorMsg) : base(errorMsg)
        {
            ErrorCode = errorCode;
        }

        public WindowsException(WindowsErrorCode errorCode) : this(errorCode, null)
        {

        }
    }
}