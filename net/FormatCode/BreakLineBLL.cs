//***********************************************************************************
//文件名称：BreakLineBLL.cs
//功能描述：处理代码换行
//数据表：
//作者：周围
//日期：2020-03-30
//修改记录：
//***********************************************************************************
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace FormatCode
{
    /// <summary>
    /// 处理代码换行
    /// </summary>
    public static class BreakLineBLL
    {
        /// <summary>
        /// 当过滤类型出现多个时，会有问题，需要循环调用
        /// </summary>
        private const String filter = "*.cs";

        /// <summary>
        /// cs文件添加顶部注释，已有注释的不再添加
        /// </summary>
        public static void Do(String path)
        {
            Console.WriteLine("BREAK LINE...");

            String[] files = Directory.GetFiles(path, filter, SearchOption.AllDirectories);

            Int32 successCount = 0;

            foreach (String filePath in files)
            {
                List<String> newContentList = new List<String>();

                String[] contentArray = Util.ReadFileAllLines(filePath);

                List<String> contentListCopy = new List<String>(contentArray);

                //移除首尾空白字符
                Regex reg = new Regex(".+; *//");

                for (Int32 i = 0; i < contentArray.Length; i++)
                {
                    contentArray[i] = contentArray[i].Trim();

                    //移除语句后面的注释
                    //分号位置
                    if (!contentArray[i].StartsWith("//"))
                    {
                        Match match = reg.Match(contentArray[i]);

                        if (match.Success)
                        {
                            contentArray[i] = match.Value.TrimEnd('/').Trim();
                        }
                    }
                }

                //原文件是否有变化
                Boolean isChange = false;

                //最后一行不处理
                for (Int32 i = 0; i < contentArray.Length; i++)
                {
                    String lineContent = contentArray[i];

                    //添加原来行
                    newContentList.Add(contentListCopy[i]);

                    //添加空白行
                    if (i < contentArray.Length - 1 && IsCodeLine(lineContent, contentArray[i + 1]))
                    {
                        newContentList.Add(String.Empty);

                        isChange = true;
                    }
                }

                //保存文件
                if (isChange)
                {
                    File.WriteAllLines(filePath, newContentList, CommonData.EncodingUTF8);

                    successCount++;
                }
            }

            Console.WriteLine("BREAK LINE FILES:" + successCount);
        }

        /// <summary>
        /// 检测当前行是否为有效代码行，只有有效代码行才能在其后添加空行
        /// </summary>
        /// <param name="content">当前行内容</param>
        /// <param name="nextContent">下一行内容</param>
        /// <returns></returns>
        private static Boolean IsCodeLine(String content, String nextContent)
        {
            Boolean result;

            //以分号结束，下一行以分号或括号结束
            result = (content.EndsWith(";") || content.EndsWith("}"))
                && (nextContent.EndsWith(";") || nextContent.EndsWith(")") || nextContent.StartsWith("//"));

            //不以特定的字符开头
            String[] notCodePrefix1 = new String[] { "using ", "namespace", "catch", "finally", "//", "{", "+", "," };

            String[] notCodePrefix2 = new String[] { "catch", "finally", "else", "}" };

            result = result && !StartWith(content, notCodePrefix1) && !StartWith(nextContent, notCodePrefix2);

            return result;
        }

        /// <summary>
        /// 是否以指定字符开头
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private static Boolean StartWith(String content, String[] notCodePrefix)
        {
            //不是有效代码行的开始字符
            foreach (String item in notCodePrefix)
            {
                if (content.StartsWith(item))
                    return true;
            }

            return false;
        }


    }
}
