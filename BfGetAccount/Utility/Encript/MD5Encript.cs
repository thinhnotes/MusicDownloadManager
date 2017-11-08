using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Encript
{
    public static class Md5Encript
    {
        public static string GetMd5(this byte[] bytes)
        {
            var md5 = MD5.Create();
            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(bytes);
            return BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
        }

        public static string GetMd5(this string s)
        {
            byte[] encodedPassword = new UTF8Encoding().GetBytes(s);
            return encodedPassword.GetMd5();
        }
    }
}
