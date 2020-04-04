// ****************************************
// FileName:MoqikakaUtil.cs
// Description: 摩奇卡卡安全工具类
// Tables:Nothing
// Author:Jordan Zuo
// Create Date:2013-10-10
// Revision History:
// ****************************************

using System;
using System.Text;

namespace Util.Security
{
    /// <summary>
    /// 摩奇卡卡安全工具类
    /// </summary>
    public static class MoqikakaUtil
    {
        /// <summary>
        /// 摩奇卡卡的加密代码
        /// </summary>
        /// <param name="source">待加密字符串</param>
        /// <param name="key">加密key</param>
        /// <returns>加密字符串</returns>
        public static String EncodeString(String source, String key)
        {
            Byte[] inputBytes = Encoding.UTF8.GetBytes(source);
            Byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            for (Int32 i = 0; i < inputBytes.Length; i++)
            {
                inputBytes[i] = (Byte)(inputBytes[i] ^ keyBytes[i % keyBytes.Length]);
            }

            return Convert.ToBase64String(inputBytes);
        }
    }
}
