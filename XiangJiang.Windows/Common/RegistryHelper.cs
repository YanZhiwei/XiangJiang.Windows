using Microsoft.Win32;
using XiangJiang.Core;
using XiangJiang.Windows.Core;

namespace XiangJiang.Windows.Common
{
    public sealed class RegistryHelper
    {
        private static readonly string _regWinLogonPath = "SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon";

        public static void DisableDefaultLogin()
        {
            var subKey = Registry.LocalMachine.CreateSubKey(_regWinLogonPath);
            if (subKey == null)
                throw new WindowsException(WindowsErrorCode.UnauthorizedAccessException,
                    $"No registry {_regWinLogonPath} access");
            using (subKey)
            {
                subKey.DeleteValue("DefaultUserName", false);
                subKey.DeleteValue("DefaultPassword", false);
                subKey.DeleteValue("AutoAdminLogon", false);
            }
        }

        /// <summary>
        ///     设置Windows 免密登录
        /// </summary>
        /// <param name="windowsUserName">账户名称</param>
        /// <param name="password">账户密码</param>
        public static void EnableDefaultLogin(string windowsUserName, string password)
        {
            Checker.Begin().NotNullOrEmpty(windowsUserName, nameof(windowsUserName));

            var subKey = Registry.LocalMachine.CreateSubKey(_regWinLogonPath);
            if (subKey == null)
                throw new WindowsException(WindowsErrorCode.UnauthorizedAccessException,
                    $"No registry {_regWinLogonPath} access");

            using (subKey)
            {
                subKey.SetValue("AutoAdminLogon", "1");
                subKey.SetValue("DefaultUserName", windowsUserName);
                subKey.SetValue("DefaultPassword", password);
            }
        }

        public static void SetStartup(string startupItemPath, string startupItemKey, bool perMachine = true)
        {
            StartupSet(startupItemPath, startupItemKey, true, perMachine);
        }

        public static void DisableStartup(string startupItemPath, string startupItemKey, bool perMachine = true)
        {
            StartupSet(startupItemPath, startupItemKey, false, perMachine);
        }

        private static void StartupSet(string startupItemPath, string startupItemKey, bool set, bool perMachine = true)
        {
            Checker.Begin()
                .NotNullOrEmpty(startupItemPath, nameof(startupItemPath))
                .CheckFileExists(startupItemPath)
                .NotNullOrEmpty(startupItemKey, nameof(startupItemKey));

            using (var registry = perMachine ? Registry.LocalMachine : Registry.CurrentUser)
            {
                var subKey = registry.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);

                if (set)
                {
                    subKey?.SetValue(startupItemKey, startupItemPath);
                }
                else
                {
                    var value = subKey?.GetValue(startupItemKey);

                    if (value != null) subKey.DeleteValue(startupItemKey);
                }
            }
        }
    }
}