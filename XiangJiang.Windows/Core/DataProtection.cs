using System;
using System.Security.Cryptography;
using System.Text;

namespace XiangJiang.Windows.Core
{
    public sealed class DataProtection
    {
        private readonly byte[] _additionalEntropy;
        private readonly string _prefixSuffix;

        public DataProtection(byte[] additionalEntropy, string prefixSuffix = null)
        {
            _additionalEntropy = additionalEntropy ?? throw new ArgumentNullException(nameof(additionalEntropy));
            _prefixSuffix = prefixSuffix;
        }

        public string Protect(string data,
            DataProtectionScope scope = DataProtectionScope.CurrentUser)
        {
            var encryptedSecret = Convert.ToBase64String(ProtectedData.Protect(Encoding.UTF8.GetBytes(data),
                _additionalEntropy,
                scope));
            return string.Concat(_prefixSuffix, encryptedSecret, _prefixSuffix);
        }

        public string Unprotect(string data,
            DataProtectionScope scope = DataProtectionScope.CurrentUser)
        {
            var prefixSuffixLength = string.IsNullOrEmpty(_prefixSuffix) ? 0 : _prefixSuffix.Length;
            data = data.Substring(prefixSuffixLength, data.Length - prefixSuffixLength * 2);

            return Encoding.UTF8.GetString(ProtectedData.Unprotect(Convert.FromBase64String(data), _additionalEntropy,
                scope));
        }
    }
}