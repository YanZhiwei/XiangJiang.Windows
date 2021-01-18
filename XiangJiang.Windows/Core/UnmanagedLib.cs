using System;
using System.Runtime.InteropServices;
using XiangJiang.Core;

namespace XiangJiang.Windows.Core
{
    /// <summary>
    ///     非托管DLL加载处理
    /// </summary>
    /// 时间：2016/11/7 13:50
    /// 备注：
    public sealed class UnmanagedLib : IDisposable
    {
        #region Constructors

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="libFilePath">非托管DLL路径</param>
        /// 时间：2016/11/7 13:59
        /// 备注：
        public UnmanagedLib(string libFilePath)
        {
            Checker.Begin()
                .NotNullOrEmpty(libFilePath, nameof(libFilePath))
                .CheckFileExists(libFilePath);
            LibFilePath = libFilePath;
        }

        #endregion Constructors

        #region Fields

        /// <summary>
        ///     非托管DLL 路径
        /// </summary>
        public readonly string LibFilePath;

        private IntPtr _instance; //dll实例

        #endregion Fields

        #region Methods

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// 时间：2016/11/7 14:10
        /// 备注：
        public void Dispose()
        {
            FreeLib();
        }

        /// <summary>
        ///     释放
        /// </summary>
        public void FreeLib()
        {
            Win32Api.FreeLibrary(_instance);
        }

        /// <summary>
        ///     加载DLL
        /// </summary>
        public void Load()
        {
            _instance = Win32Api.LoadLibrary(LibFilePath);

            if (_instance == IntPtr.Zero)
                throw new WindowsException(WindowsErrorCode.UnmanagedLibLoadFailed,
                    $"Failed to load unmanaged file {LibFilePath}");
        }

        /// <summary>
        ///     获取方法指针
        /// </summary>
        /// <param name="functionName">方法指针名称</param>
        /// <param name="t">类型</param>
        /// <returns>委托</returns>
        public Delegate GetAddress(string functionName, Type t)
        {
            var procAddr = Win32Api.GetProcAddress(_instance, functionName);

            if (procAddr == IntPtr.Zero)
                throw new WindowsException(WindowsErrorCode.UnmanagedLibFunctionNoExist,
                    $"The function {functionName} does not exist");
            return Marshal.GetDelegateForFunctionPointer(procAddr, t);
        }

        #endregion Methods
    }
}