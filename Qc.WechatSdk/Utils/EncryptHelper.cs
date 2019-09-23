using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Qc.WechatSdk.Utils
{
    public class EncryptHelper
    {
        /// <summary>
        /// use sha1 to encrypt string
        /// </summary>
        public static string SHA1Encrypt(string sourceString)
        {
            byte[] StrRes = Encoding.Default.GetBytes(sourceString);
            HashAlgorithm iSHA = new SHA1CryptoServiceProvider();
            StrRes = iSHA.ComputeHash(StrRes);
            StringBuilder EnText = new StringBuilder();
            foreach (byte iByte in StrRes)
            {
                EnText.AppendFormat("{0:x2}", iByte);
            }
            return EnText.ToString();
        }
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string MD5Encrypt(string password)
        {
            var data = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(password));
            var hash = new StringBuilder();

            foreach (var b in data)
            {
                hash.Append(b.ToString("x2"));
            }

            return hash.ToString().ToUpper();
        }
    }
}
