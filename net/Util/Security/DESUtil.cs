// ****************************************
// FileName:DESUtil.cs
// Description:DES加密助手类
// Tables:Nothing
// Author:Jordan Zuo
// Create Date:2014-07-24
// Revision History:
// ****************************************

using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace Util.Security
{
    /// <summary>
    /// DES加密助手类
    /// </summary>
    public static class DESUtil
    {
        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="sourceString">源字符串</param>
        /// <param name="key">DES加密的私钥，必须是8位长的字符串</param>
        /// <returns>加密后的字符串</returns>
        public static String EncryptString(String sourceString, String key)
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = UTF8Encoding.UTF8.GetBytes(key);
                des.IV = UTF8Encoding.UTF8.GetBytes(key);
                des.Mode = CipherMode.ECB;

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        Byte[] inputByteArray = Encoding.UTF8.GetBytes(sourceString);

                        cs.Write(inputByteArray, 0, inputByteArray.Length);
                        cs.FlushFinalBlock();
                        cs.Close();
                    }

                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="encryptedString">源字符串</param>
        /// <param name="key">DES加密的私钥，必须是8位长的字符串</param>
        /// <returns>解密后的字符串</returns>
        public static String DecryptString(String encryptedString, String key)
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = UTF8Encoding.UTF8.GetBytes(key);
                des.IV = UTF8Encoding.UTF8.GetBytes(key);
                des.Mode = CipherMode.ECB;

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        Byte[] inputByteArray = Convert.FromBase64String(encryptedString);

                        cs.Write(inputByteArray, 0, inputByteArray.Length);
                        cs.FlushFinalBlock();
                        cs.Close();
                    }

                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
        }
    }
}