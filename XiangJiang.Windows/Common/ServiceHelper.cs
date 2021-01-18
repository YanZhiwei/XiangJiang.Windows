using System;
using System.Linq;
using System.ServiceProcess;
using XiangJiang.Core;

namespace XiangJiang.Windows.Common
{
    /// <summary>
    ///     Windows Service辅助类
    /// </summary>
    public sealed class ServiceHelper
    {
        /// <summary>
        ///     判断服务是否存在
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <returns>是否存在</returns>
        public static bool IsExisted(string serviceName)
        {
            Checker.Begin().NotNullOrEmpty(serviceName, nameof(serviceName));
            var services = ServiceController.GetServices();
            return services.Count(c =>
                string.Compare(c.ServiceName, serviceName, StringComparison.OrdinalIgnoreCase) == 0) > 0;
        }


        /// <summary>
        ///     启动服务
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        public static void Start(string serviceName)
        {
            Checker.Begin().NotNullOrEmpty(serviceName, nameof(serviceName));
            using (var control = new ServiceController(serviceName))
            {
                if (control.Status == ServiceControllerStatus.Stopped) control.Start();
            }
        }

        /// <summary>
        ///     停止服务
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        public static void Stop(string serviceName)
        {
            Checker.Begin().NotNullOrEmpty(serviceName, nameof(serviceName));
            using (var control = new ServiceController(serviceName))
            {
                if (control.Status == ServiceControllerStatus.Running) control.Stop();
            }
        }
    }
}