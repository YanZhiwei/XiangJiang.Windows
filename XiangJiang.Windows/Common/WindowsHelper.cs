using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management;
using System.Security.Principal;
using XiangJiang.Common;
using XiangJiang.Core;
using XiangJiang.Windows.Models;

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

        /// <summary>
        ///     获取本机用户列表
        /// </summary>
        /// <returns>用户列表</returns>
        public static ReadOnlyCollection<WindowsAccount> GetUsers()
        {
            return GetUsers(Environment.MachineName);
        }

        /// <summary>
        ///     获取指定机器用户列表
        /// </summary>
        /// <param name="domain">机器名称</param>
        /// <returns>用户列表</returns>
        public static ReadOnlyCollection<WindowsAccount> GetUsers(string domain)
        {
            var users = new List<WindowsAccount>();
            var query = new SelectQuery("Win32_UserAccount", $"Domain='{domain}'");
            var searcher = new ManagementObjectSearcher(query);
            var searcherResult = searcher.Get();
            foreach (var item in searcherResult)
            {
                var account = new WindowsAccount(
                    item["Name"].ToStringOrDefault(),
                    item["FullName"].ToStringOrDefault(),
                    item["AccountType"].ToStringOrDefault(),
                    item["Description"].ToStringOrDefault(),
                    item["Caption"].ToStringOrDefault(),
                    item["Domain"].ToStringOrDefault(),
                    item["Disabled"].ToBooleanOrDefault(),
                    item["LocalAccount"].ToBooleanOrDefault(),
                    item["Status"].ToStringOrDefault(),
                    item["Lockout"].ToBooleanOrDefault());
                users.Add(account);
            }

            return new ReadOnlyCollection<WindowsAccount>(users);
        }
    }
}