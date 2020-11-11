using System;
using System.IO;

namespace test
{
    public static class Util
    {
        /// <summary>
        /// UTF8中乱码符号
        /// </summary>
        const String utf8Char = "�";

        /// <summary>
        /// 是否包含乱码，用于UTF8编码读取GB2312编码内容时
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static Boolean IsGarbled(String text)
        {
            return text.Contains(utf8Char);
        }

        /// <summary>
        /// 先以UTF8读取文件，如有乱码，则以GB2312再次读取内容
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static String ReadFileText(String path)
        {
            String content = File.ReadAllText(path, CommonData.EncodingUTF8);
            if (content.Contains(utf8Char))
            {
                content = File.ReadAllText(path, CommonData.EncodingGB2312);
            }
            return content;
        }

        /// <summary>
        /// 先以UTF8读取文件，如有乱码，则以GB2312再次读取内容
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static String[] ReadFileAllLines(String path)
        {
            String[] contentArray = File.ReadAllLines(path, CommonData.EncodingUTF8);
            foreach (var item in contentArray)
            {
                if (item.Contains(utf8Char))
                {
                    contentArray = File.ReadAllLines(path, CommonData.EncodingGB2312);
                    break;
                }
            }
            return contentArray;
        }

    }
}
