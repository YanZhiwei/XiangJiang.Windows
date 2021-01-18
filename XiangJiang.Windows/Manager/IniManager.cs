using System.IO;
using System.Text;
using XiangJiang.Core;
using XiangJiang.Windows.Core;

namespace XiangJiang.Windows.Manager
{
    /// <summary>
    ///     INI文件操作
    /// </summary>
    /// 时间：2016/8/25 14:59
    /// 备注：
    public class IniManager
    {
        private readonly string _filePath;

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="path">INI文件路径</param>
        /// 时间：2016/8/25 15:00
        /// 备注：
        public IniManager(string path)
        {
            Checker.Begin()
                .NotNullOrEmpty(path, nameof(path))
                .IsFilePath(path);
            _filePath = path;
        }

        public bool Exist => File.Exists(_filePath);

        public void Create()
        {
            if (File.Exists(_filePath)) return;
            var iniFolder = Path.GetDirectoryName(_filePath);
            var directoryInfo = new DirectoryInfo(iniFolder);
            if (!directoryInfo.Exists)
                directoryInfo.Create();
            using (File.Create(_filePath))
            {
            }
        }

        /// <summary>
        ///     读取INI
        /// </summary>
        /// <param name="section">段落名称</param>
        /// <param name="key">关键字</param>
        /// <returns>读取值</returns>
        public string Get(string section, string key)
        {
            CheckedIniParameter(section, key);
            var builder = new StringBuilder(500);
            Win32Api.GetPrivateProfileString(section, key, string.Empty, builder, 500, _filePath);
            return builder.ToString();
        }

        /// <summary>
        ///     读取INI
        /// </summary>
        /// <param name="section">段落名称</param>
        /// <param name="key">关键字</param>
        /// <param name="defaultValue">当根据KEY读取不到值得时候缺省值</param>
        /// <returns>读取的值</returns>
        public string Get(string section, string key, string defaultValue)
        {
            CheckedIniParameter(section, key);
            var builder = new StringBuilder(500);
            Win32Api.GetPrivateProfileString(section, key, defaultValue, builder, 500, _filePath);
            return builder.ToString();
        }

        /// <summary>
        ///     写入INI
        ///     eg:_iniHelper.WriteValue("测试", "Name", "YanZhiwei");
        /// </summary>
        /// <param name="section">段落名称</param>
        /// <param name="key">关键字</param>
        /// <param name="value">关键字对应的值</param>
        public void Write(string section, string key, string value)
        {
            CheckedIniParameter(section, key);
            Win32Api.WritePrivateProfileString(section, key, value, _filePath);
        }

        private void CheckedIniParameter(string iniSection, string iniKey)
        {
            Checker.Begin().NotNullOrEmpty(iniSection, nameof(iniSection)).NotNullOrEmpty(iniKey, nameof(iniKey));
        }
    }
}