// ****************************************
// FileName:HMACMD5Util.cs
// Description:Hmac-MD5加密助手类
// Tables:Nothing
// Author:Jordan Zuo
// Create Date:2014-06-20
// Revision History:
// ****************************************

using System;
using System.Text;
using System.Security.Cryptography;

namespace Util.Security
{
    /// <summary>
    /// Hmac-MD5加密助手类
    /// </summary>
    public static class HMACMD5Util
    {
        /// <summary>
        /// Hmac－MD5加密
        /// </summary>
        /// <param name="sourceString">源字符串</param>
        /// <param name="encryptKey">加密key</param>
        /// <param name="ifBase64Encode">是否进行Base64编码(当需要Base64编码时,letterCase无效)</param>
        /// <param name="letterCase">大小写枚举</param>
        /// <exception cref="System.ArgumentNullException">System.ArgumentNullException</exception>
        /// <returns>加密后的字符串</returns>
        public static String HmacMD5(String sourceString, String encryptKey, Boolean ifBase64Encode, LetterCase letterCase = LetterCase.UpperCase)
        {
            if (String.IsNullOrWhiteSpace(sourceString) || String.IsNullOrEmpty(encryptKey)) throw new ArgumentNullException("Empty", "sourceString or encryptKey can't be empty.");

            //构造HMACMD5对象，并计算哈希值
            HMACMD5 hmacMD5 = new HMACMD5(Encoding.UTF8.GetBytes(encryptKey));
            Byte[] hashByteArray = hmacMD5.ComputeHash(Encoding.UTF8.GetBytes(sourceString));

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