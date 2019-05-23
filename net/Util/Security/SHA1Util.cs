// ****************************************
// FileName:SHA1Util.cs
// Description: MSHA1Util助手类
// Tables:Nothing
// Author:Jordan Zuo
// Create Date:2014-06-18
// Revision History:
// ****************************************

using System;
using System.Text;
using System.Security.Cryptography;

namespace Util.Security
{
    /// <summary>
    /// SHA1助手类
    /// </summary>
    public static class SHA1Util
    {
        /// <summary>
        /// SHA1加密
        /// </summary>
        /// <param name="str">需要加密的字符串</param>
        /// <param name="letterCase">大小写枚举</param>
        /// <exception cref="System.ArgumentNullException">System.ArgumentNullException</exception>
        /// <returns>加密后的字符串</returns>
        public static String SHA1(String str, LetterCase letterCase = LetterCase.UpperCase)
        {
            if (String.IsNullOrWhiteSpace(str)) throw new ArgumentNullException("str", "str can't be empty.");

            return SHA1(Encoding.UTF8.GetBytes(str), letterCase);
        }

        /// <summary>
        /// SHA1加密
        /// </summary>
        /// <param name="byteArray">需要加密的字节数组</param>
        /// <param name="letterCase">大小写枚举</param>
        /// <exception cref="System.ArgumentNullException">System.ArgumentNullException</exception>
        /// <returns>加密后的字符串</returns>
        public static String SHA1(Byte[] byteArray, LetterCase letterCase = LetterCase.UpperCase)
        {
            if (byteArray == null || byteArray.Length == 0) throw new ArgumentNullException("Error", "str can't be empty.");

            using (SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider())
            {
                //计算data字节数组的哈希值 
                Byte[] sha1Data = sha1.ComputeHash(byteArray);

                //将加密后的字节数组转换为16进制的字符串
                StringBuilder sBuilder = new StringBuilder();
                if (letterCase == LetterCase.LowerCase)
                {
                    for (Int32 i = 0; i < sha1Data.Length; i++)
                    {
                        sBuilder.Append(sha1Data[i].ToString("x2"));
                    }
                }
                else
                {
                    for (Int32 i = 0; i < sha1Data.Length; i++)
                    {
                        sBuilder.Append(sha1Data[i].ToString("X2"));
                    }
                }

                return sBuilder.ToString();
            }            
        }
    }
}