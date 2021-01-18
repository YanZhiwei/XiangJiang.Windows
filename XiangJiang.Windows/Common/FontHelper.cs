using System;
using System.IO;
using XiangJiang.Windows.Core;

namespace XiangJiang.Windows.Common
{
    /// <summary>
    ///     FONT帮助类
    /// </summary>
    public static class FontHelper
    {
        #region Methods

        /// <summary>
        ///     字体安装
        /// </summary>
        /// <param name="fontSourcePath">字体所在路径</param>
        /// <returns>是否安装成功</returns>
        public static bool Install(string fontSourcePath)
        {
            var fontFile = Path.GetFileName(fontSourcePath);
            var targetFontPath = $@"{Environment.GetEnvironmentVariable("WINDIR")}\fonts\{fontFile}";

            try
            {
                var fontName = Path.GetFileNameWithoutExtension(targetFontPath);

                if (!File.Exists(targetFontPath) && File.Exists(fontSourcePath))
                {
                    File.Copy(fontSourcePath, targetFontPath);
                    Win32Api.AddFontResource(targetFontPath);
                    Win32Api.WriteProfileString("fonts", fontName + "(TrueType)", fontFile);
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        #endregion Methods
    }
}