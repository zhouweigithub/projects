// ****************************************
// FileName:HMACSHA1Util.cs
// Description:Hmac－SHA1加密助手类
// Tables:Nothing
// Author:Jordan Zuo
// Create Date:2013-10-10
// Revision History:
// ****************************************

using System;
using System.Text;
using System.Security.Cryptography;

namespace Util.Security
{
    /// <summary>
    /// Hmac－SHA1加密助手类
    /// </summary>
    public static class HMACSHA1Util
    {
        /// <summary>
        /// Hmac－SHA1加密
        /// </summary>
        /// <param name="sourceString">源字符串</param>
        /// <param name="encryptKey">加密key</param>
        /// <param name="letterCase">大小写枚举</param>
        /// <exception cref="System.ArgumentNullException">System.ArgumentNullException</exception>
        /// <returns>加密后的字符串</returns>
        public static String HmacSha1(String sourceString, String encryptKey, LetterCase letterCase = LetterCase.UpperCase)
        {
            if (String.IsNullOrWhiteSpace(sourceString) || String.IsNullOrEmpty(encryptKey)) throw new ArgumentNullException("Empty", "sourceString or encryptKey can't be empty.");
            
            //构造HMACSHA1对象，并计算哈希值
            HMACSHA1 hmacSHA1 = new HMACSHA1(Encoding.UTF8.GetBytes(encryptKey));
            Byte[] hashByteArray = hmacSHA1.ComputeHash(Encoding.UTF8.GetBytes(sourceString));

            //格式化数据并返回
            StringBuilder sBuilder = new StringBuilder();
            if (letterCase == LetterCase.LowerCase)
            {
                for (Int32 i = 0; i < hashByteArray.Length; i++)
                {
                    sBuilder.Append(hashByteArray[i].ToString("x2"));
                }
            }
            else
            {
                for (Int32 i = 0; i < hashByteArray.Length; i++)
                {
                    sBuilder.Append(hashByteArray[i].ToString("X2"));
                }
            }

            return sBuilder.ToString();
        }

        /// <summary>
        /// Hmac－SHA1加密
        /// </summary>
        /// <param name="sourceString">源字符串</param>
        /// <param name="encryptKey">加密key</param>
        /// <param name="ifBase64Encode">是否进行Base64编码(当需要Base64编码时,letterCase无效)</param>
        /// <param name="letterCase">大小写枚举</param>
        /// <exception cref="System.ArgumentNullException">System.ArgumentNullException</exception>
        /// <returns>加密后的字符串</returns>
        public static String HmacSha1(String sourceString, String encryptKey, Boolean ifBase64Encode, LetterCase letterCase = LetterCase.UpperCase)
        {
            if (String.IsNullOrWhiteSpace(sourceString) || String.IsNullOrEmpty(encryptKey)) throw new ArgumentNullException("Empty", "sourceString or encryptKey can't be empty.");

            //构造HMACSHA1对象，并计算哈希值
            HMACSHA1 hmacSHA1 = new HMACSHA1(Encoding.UTF8.GetBytes(encryptKey));
            Byte[] hashByteArray = hmacSHA1.ComputeHash(Encoding.UTF8.GetBytes(sourceString));

            String hashString = String.Empty;
            if (ifBase64Encode)
            {
                hashString = Convert.ToBase64String(hashByteArray);
            }
            else
            {
                StringBuilder sBuilder = new StringBuilder();
                if (letterCase == LetterCase.LowerCase)
                {
                    for (Int32 i = 0; i < hashByteArray.Length; i++)
                    {
                        sBuilder.Append(hashByteArray[i].ToString("x2"));
                    }
                }
                else
                {
                    for (Int32 i = 0; i < hashByteArray.Length; i++)
                    {
                        sBuilder.Append(hashByteArray[i].ToString("X2"));
                    }
                }

                hashString = sBuilder.ToString();
            }

            return hashString;
        }
    }
}