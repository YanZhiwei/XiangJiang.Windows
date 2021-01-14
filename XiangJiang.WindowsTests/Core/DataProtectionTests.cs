using System.Security.Cryptography;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XiangJiang.Windows.Core;

namespace XiangJiang.WindowsTests.Core
{
    [TestClass]
    public class DataProtectionTests
    {
        private readonly DataProtection _dataProtection;

        public DataProtectionTests()
        {
            _dataProtection = new DataProtection(Encoding.Default.GetBytes("aaabbbccddffgghhddaa"), "XiangJiang");
        }

        [TestMethod]
        public void ProtectTest()
        {
            var protectText = _dataProtection.Protect("hello", DataProtectionScope.LocalMachine);
            var actual = _dataProtection.Unprotect(protectText, DataProtectionScope.LocalMachine);
            Assert.IsNotNull(actual, "hello");
        }
    }
}