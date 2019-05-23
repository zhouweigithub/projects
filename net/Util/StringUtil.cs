//***********************************************************************************
//文件名称：StringUtil.cs
//功能描述：字符串助手类
//数据表：Nothing
//作者：Jordan
//日期：2014-03-25 11:34:00
//修改记录：
//***********************************************************************************

using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Util
{
    /// <summary>
    /// 字符串工具类
    /// </summary>
    public static class StringUtil
    {
        #region 全局变量

        //全角转半角的变量
        private static readonly Int32 minCharValue = 65280;//全角字符的最小int值
        private static readonly Int32 maxCharValue = 65375;//全角字符的最大int值
        private static readonly Int32 reduceCharValue = 65248;//全角字符与半角字符转换的差值
        private static readonly Int32 spaceCharValue = 32;//半角空格对应的int值
        private static readonly Int32 spaceFullCharValue = 12288;//全角空格对应的int值

        //定义GUID正则表达式
        private static Regex guidRegex = new Regex("^[A-Fa-f0-9]{8}(-[A-Fa-f0-9]{4}){3}-[A-Fa-f0-9]{12}$", RegexOptions.Compiled);

        //验证中文的正则表达式
        private static Regex chineseRegex = new Regex(@"([\u4e00-\u9fbf])+", RegexOptions.IgnoreCase);

        #endregion

        #region 方法

        /// <summary>
        /// 判断字符串是否为中文
        /// </summary>
        /// <param name="str">待验证字符串</param>
        /// <returns>是否为中文</returns>
        public static Boolean IsChinese(String str)
        {
            return chineseRegex.IsMatch(str);
        }

        /// <summary>
        /// 判断字符串是否为GUID类型
        /// </summary>
        /// <param name="str">被检测的字符串</param>
        /// <returns>串是否为GUID类型</returns>
        public static Boolean IsGuid(String str)
        {
            return guidRegex.IsMatch(str);
        }

        /// <summary>
        /// Base64 Encoding
        /// </summary>
        /// <param name="str">待编码的字符串</param>
        /// <exception cref="System.ArgumentNullException">System.ArgumentNullException</exception>
        /// <exception cref="System.Text.EncoderFallbackException">System.Text.EncoderFallbackException</exception>
        /// <returns>编码后的字符串</returns>
        public static String Base64Encode(String str)
        {
            if (String.IsNullOrWhiteSpace(str))
            {
                return String.Empty;
            }

            Byte[] data = Encoding.UTF8.GetBytes(str);

            return Convert.ToBase64String(data);
        }

        /// <summary>
        /// Base64 Decoding
        /// </summary>
        /// <param name="str">待解码的字符串</param>
        /// <exception cref="System.ArgumentNullException">System.ArgumentNullException</exception>
        /// <exception cref="System.FormatException">System.FormatException</exception>
        /// <returns>解码后的字符串</returns>
        public static String Base64Decode(String str)
        {
            if (String.IsNullOrWhiteSpace(str))
            {
                return String.Empty;
            }

            Byte[] data = Convert.FromBase64String(str);

            return Encoding.UTF8.GetString(data);
        }

        /// <summary>
        /// 分隔字符串，得到字符串数组
        /// </summary>
        /// <param name="sourceString">源字符串</param>
        /// <param name="delimiterArray">分隔字符串数组</param>
        /// <exception cref="System.ArgumentException">System.ArgumentException</exception>
        /// <exception cref="System.ArgumentNullException">System.ArgumentNullException</exception>
        /// <returns>分隔后的字符串数组</returns>
        public static String[] Split(String sourceString, String[] delimiterArray)
        {
            if (String.IsNullOrWhiteSpace(sourceString)) throw new ArgumentNullException("sourceString", "sourceString can't be empty.");
            if (delimiterArray.Length == 0) throw new ArgumentNullException("delimiters", "delimiters can't be empty.");

            //分隔字符串，得到分隔后的字符串数组
            return sourceString.Split(delimiterArray, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// 分隔字符串，得到字符串数组
        /// </summary>
        /// <param name="sourceString">源字符串</param>
        /// <param name="delimiterArray">分隔符数组</param>
        /// <exception cref="System.ArgumentException">System.ArgumentException</exception>
        /// <exception cref="System.ArgumentNullException">System.ArgumentNullException</exception>
        /// <returns>分隔后的字符串数组</returns>
        public static String[] Split(String sourceString, Char[] delimiterArray)
        {
            if (String.IsNullOrWhiteSpace(sourceString)) throw new ArgumentNullException("sourceString", "sourceString can't be empty.");
            if (delimiterArray.Length == 0) throw new ArgumentNullException("delimiters", "delimiters can't be empty.");

            //分隔字符串，得到分隔后的字符串数组
            return sourceString.Split(delimiterArray, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// 分隔字符串，得到字符串数组
        /// </summary>
        /// <param name="sourceString">源字符串</param>
        /// <param name="delimiter">分隔符</param>
        /// <exception cref="System.ArgumentException">System.ArgumentException</exception>
        /// <exception cref="System.ArgumentNullException">System.ArgumentNullException</exception>
        /// <returns>分隔后的字符串数组</returns>
        public static String[] Split(String sourceString, Char delimiter)
        {
            if (String.IsNullOrWhiteSpace(sourceString)) throw new ArgumentNullException("sourceString", "sourceString can't be null or empty.");

            //定义分隔符数组
            Char[] delimiterArray = new Char[] { delimiter };

            return Split(sourceString, delimiterArray);
        }

        /// <summary>
        /// 替换源字符串中的参数
        /// </summary>
        /// <param name="sourceString">源字符串</param>
        /// <param name="source">需要替换的参数名称</param>
        /// <param name="dest">替换的参数值</param>
        /// <exception cref="System.ArgumentException">System.ArgumentException</exception>
        /// <returns>替换后的字符串</returns>
        public static String Replace(String sourceString, String source, String dest)
        {
            if (String.IsNullOrWhiteSpace(sourceString)) throw new ArgumentNullException("sourceString", "sourceString can't be null or empty.");
            if (source == null) throw new ArgumentNullException("source", "source can't be null.");
            if (dest == null) throw new ArgumentNullException("dest", "dest can't be null.");

            return sourceString.Replace(source, dest);
        }

        /// <summary>
        /// 替换源字符串中的参数
        /// </summary>
        /// <param name="sourceString">源字符串</param>
        /// <param name="source">需要替换的内容数组</param>
        /// <param name="dest">进行替换的内容数组</param>
        /// <exception cref="System.ArgumentException">System.ArgumentException</exception>
        /// <returns>替换后的字符串</returns>
        public static String Replace(String sourceString, String[] source, String[] dest)
        {
            if (String.IsNullOrWhiteSpace(sourceString)) throw new ArgumentNullException("sourceString", "sourceString can't be null or empty.");
            if (source.Length == 0) throw new ArgumentNullException("source", "source can't be empty.");
            if (dest.Length == 0) throw new ArgumentNullException("dest", "dest can't be empty.");

            for (Int32 i = 0; i < source.Length; i++)
            {
                sourceString = sourceString.Replace(source[i], dest[i]);
            }

            return sourceString;
        }

        /// <summary>
        /// 替换源字符串中的参数
        /// </summary>
        /// <param name="sourceString">源字符串</param>
        /// <param name="paramList">需要替换的名称/值对</param>
        /// <exception cref="System.ArgumentException">System.ArgumentException</exception>
        /// <returns>替换后的字符串</returns>
        public static String Replace(String sourceString, Dictionary<String, Object> paramList)
        {
            if (String.IsNullOrWhiteSpace(sourceString)) throw new ArgumentNullException("sourceString", "sourceString can't be null or empty.");
            if (paramList.Count == 0) throw new ArgumentNullException("paramList", "paramList can't be empty.");

            foreach (String key in paramList.Keys)
            {
                sourceString = sourceString.Replace(key, paramList[key].ToString());
            }

            return sourceString;
        }

        /// <summary>
        /// 将指定的全角字符转换为半角字符
        /// </summary>
        /// <param name="input">要转换的全角字符</param>
        /// <returns>返回转换后的半角字符</returns>
        public static Char ToDBC(Char input)
        {
            //如果是空格全角字符，则直接转换为
            if (input == spaceFullCharValue)
            {
                input = (Char)spaceCharValue;
            }
            //如果在全角字符内，则转换为半角字符
            else if (input > minCharValue && input < maxCharValue)
            {
                input = (Char)(input - reduceCharValue);
            }

            return input;
        }

        /// <summary>
        /// 将指定的全角字符串转换为半角字符串
        /// </summary>
        /// <param name="input">要转换的全角字符串</param>
        /// <returns>对应的半角字符串</returns>
        public static String ToDBC(String input)
        {
            if (input == null)
            {
                return null;
            }

            //转换字符串
            Char[] ch = input.ToCharArray();
            for (Int32 i = 0; i < ch.Length; i++)
            {
                ch[i] = ToDBC(ch[i]);
            }

            return new String(ch);
        }

        /// <summary>
        /// 验证是否是手机号码
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns>是否是手机号码</returns>
        public static Boolean IsMobilePhone(String input)
        {
            Regex regex = new Regex("^1\\d{10}$");

            return regex.IsMatch(input);
        }

        #endregion
    }
}