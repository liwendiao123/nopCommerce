using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Nop.Core.Infrastructure
{


   /// <summary>
   /// md5工具
   /// </summary>
   public class Md5Util
    {
        private static readonly string ENCRYPT_KEY = "qs!aa@kj";



        /// <summary>
        /// 加密 
        /// </summary>
        /// <param name="encryptString"></param>
        /// <returns></returns>
        public static string EncryptDES(string encryptString)
        {
            try
            {
                if (string.IsNullOrEmpty(encryptString))
                    return string.Empty;
                string pKey, iKey;
                pKey = iKey = ENCRYPT_KEY;
                byte[] private_key = Encoding.UTF8.GetBytes(pKey.PadRight(8).Substring(0, 8));
                byte[] public_key = Encoding.UTF8.GetBytes(iKey);
                byte[] buffer = Encoding.UTF8.GetBytes(encryptString);

                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                using (MemoryStream mStream = new MemoryStream())
                using (CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(private_key, public_key), CryptoStreamMode.Write))
                {
                    cStream.Write(buffer, 0, buffer.Length);
                    cStream.FlushFinalBlock();
                    return Convert.ToBase64String(mStream.ToArray());
                }
            }
            catch
            {
                return encryptString;
            }
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="decryptString"></param>
        /// <returns></returns>
        public static string DecryptDES(string decryptString)
        {
            try
            {
                if (string.IsNullOrEmpty(decryptString))
                    return string.Empty;
                string pKey, iKey;
                pKey = iKey = ENCRYPT_KEY;
                byte[] private_key = Encoding.UTF8.GetBytes(pKey.PadRight(8).Substring(0, 8));
                byte[] public_key = Encoding.UTF8.GetBytes(iKey);
                byte[] inputByteArray = Convert.FromBase64String(decryptString);

                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                using (MemoryStream mStream = new MemoryStream())
                using (CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(private_key, public_key), CryptoStreamMode.Write))
                {
                    cStream.Write(inputByteArray, 0, inputByteArray.Length);
                    cStream.FlushFinalBlock();
                    return Encoding.UTF8.GetString(mStream.ToArray());
                }
            }
            catch
            {
                return decryptString;
            }
        }
    }
}
