using System.Security.Principal;
using XiangJiang.Core;

namespace XiangJiang.Windows.Common
{
    public class WindowsHelper
    {
        public static string GetSid(string windowsUserName)
        {
            Checker.Begin().NotNullOrEmpty(windowsUserName, nameof(windowsUserName));
            var account = new NTAccount(windowsUserName);
            var s = (SecurityIdentifier) account.Translate(typeof(SecurityIdentifier));
            return s.ToString();
        }
    }
}