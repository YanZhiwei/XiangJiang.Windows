using System;
using System.Diagnostics;
using System.Security.Principal;
using XiangJiang.Core;
using XiangJiang.Windows.Core;

namespace XiangJiang.Windows.Common
{
    public sealed class ProcessHelper
    {
        /// <summary>
        ///     获取进程所有者
        /// </summary>
        /// <param name="process">Process</param>
        /// <returns>进程所有者</returns>
        private static string GetOwner(Process process)
        {
            Checker.Begin().NotNull(process, nameof(process));
            var processHandle = IntPtr.Zero;
            try
            {
                Win32Api.OpenProcessToken(process.Handle, 8, out processHandle);
                var wi = new WindowsIdentity(processHandle);
                var user = wi.Name;
                return user.Contains(@"\") ? user.Substring(user.IndexOf(@"\", StringComparison.Ordinal) + 1) : user;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (processHandle != IntPtr.Zero) Win32Api.CloseHandle(processHandle);
            }
        }
    }
}