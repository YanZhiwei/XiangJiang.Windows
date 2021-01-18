using System;
using System.Runtime.InteropServices;
using System.Text;

namespace XiangJiang.Windows.Core
{
    internal sealed class Win32Api
    {
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool OpenProcessToken(IntPtr processHandle, uint desiredAccess, out IntPtr tokenHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr hwnd);

        /// <summary>
        ///     卸载DLL
        /// </summary>
        /// <param name="hModule">hModule</param>
        /// <returns>卸载是否成功</returns>
        [DllImport("kernel32.dll")]
        public static extern bool FreeLibrary(IntPtr hModule);

        /// <summary>
        ///     调用方法指针
        /// </summary>
        /// <param name="hModule">hModule</param>
        /// <param name="lpProcName">lpProcName</param>
        /// <returns>IntPtr</returns>
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        /// <summary>
        ///     加载DLL
        /// </summary>
        /// <param name="lpFileName">lpFileName</param>
        /// <returns>IntPtr</returns>
        /// 时间：2016/11/7 13:58
        /// 备注：
        [DllImport("Kernel32.dll")]
        public static extern IntPtr LoadLibrary(string lpFileName);

        /// <summary>
        ///     声明INI文件的读操作函数
        /// </summary>
        /// <param name="section">段落名称</param>
        /// <param name="key">关键字</param>
        /// <param name="def">无法读取时候时候的缺省数值</param>
        /// <param name="retVal">读取数值</param>
        /// <param name="size">数值的大小></param>
        /// <param name="filePath">路径</param>
        /// <returns></returns>
        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal,
            int size, string filePath);

        /// <summary>
        ///     声明INI文件的写操作函数
        /// </summary>
        /// <param name="section">段落名称</param>
        /// <param name="key">关键字</param>
        /// <param name="val">关键字对应的值</param>
        /// <param name="filePath">路径</param>
        /// <returns></returns>
        [DllImport("kernel32")]
        public static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("user32.dll")]
        // ReSharper disable once UnusedMember.Local
        public static extern int SendMessage(int hWnd, uint msg, int wParam, int lParam);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int WriteProfileString(string lpszSection, string lpszKeyName, string lpszString);

        /// <summary>
        ///     Adds the font resource.
        /// </summary>
        /// <param name="lpFileName">Name of the lp file.</param>
        /// <returns>数值</returns>
        [DllImport("gdi32")]
        public static extern int AddFontResource(string lpFileName);
    }
}