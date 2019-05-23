using System;
using System.Security.Cryptography;
using System.Text;

namespace Spetmall.Common
{
    public class Function
    {
        /// <summary>
        /// 生成随机字符（字母为大写）
        /// </summary>
        /// <param name="length"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetRangeNumber(int length, RangeType type)
        {
            string source = string.Empty;
            switch (type)
            {
                case RangeType.Number:
                    source = "0123456789";
                    break;
                case RangeType.Letter:
                    source = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    break;
                case RangeType.NumberAndLetter:
                    source = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                    break;
                default:
                    break;
            }
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                sb.Append(source[new Random(Guid.NewGuid().GetHashCode()).Next(0, source.Length - 1)]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 生成字母数字随机字符串（字母区分大小写）
        /// </summary>
        /// <param name="maxsize">长度</param>
        /// <returns></returns>
        public static string GetUniqueKey(int maxsize)
        {
            char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            data = new byte[maxsize];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(maxsize);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length - 1)]);
            }
            return result.ToString();
        }
    }


    public enum RangeType
    {
        /// <summary>
        /// 纯数字
        /// </summary>
        Number,
        /// <summary>
        /// 纯大写字母
        /// </summary>
        Letter,
        /// <summary>
        /// 纯数字和大写字母
        /// </summary>
        NumberAndLetter
    }
}
