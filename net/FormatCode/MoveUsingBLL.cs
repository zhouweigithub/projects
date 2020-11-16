//***********************************************************************************
//文件名称：ExegesisBLL.cs
//功能描述：移动using引用代码，系统引用放到命名空间外，其他引用放到命名空间内
//数据表：
//作者：周围
//日期：2020-03-30
//修改记录：
//***********************************************************************************
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FormatCode
{
    /// <summary>
    /// 移动using引用代码，系统引用放到命名空间外，其他引用放到命名空间内
    /// </summary>
    public static class MoveUsingBLL
    {
        /// <summary>
        /// cs文件添加顶部注释，已有注释的不再添加
        /// </summary>
        /// <param name="files"></param>
        public static void Do(List<String> files)
        {
            try
            {
                Console.WriteLine("MOVE USING...");

                Int32 successCount = 0;

                foreach (String filePath in files)
                {
                    //用于各种处理，会改动原始文本
                    String[] contentLines = Util.ReadFileAllLines(filePath);

                    //保留原始文本
                    List<String> contentLinesCopy = contentLines.ToList();

                    for (Int32 i = 0; i < contentLines.Length; i++)
                    {   //移除首尾空白字符
                        contentLines[i] = contentLines[i].Trim();
                    }

                    //命名空间位置
                    List<Int32> namespaceIndexList = GetStartIndexList(contentLines, "namespace ");

                    if (namespaceIndexList.Count == 0)
                        continue;

                    Int32 namespaceIndex = namespaceIndexList[0];

                    //系统引用
                    List<Int32> systemUsingLineIndexList = GetStartIndexList(contentLines, "using System");

                    //非系统引用
                    List<Int32> notSystemUsingLineIndexList = GetNotSystemUsingList(contentLines);

                    //移除原始的using行
                    RemoveLines(contentLinesCopy, systemUsingLineIndexList.Union(notSystemUsingLineIndexList).ToList());

                    //获取新的命名空间的位置
                    namespaceIndex = GetStartIndexList(contentLinesCopy.ToArray(), "namespace ")[0];

                    //移除命名空间附近的空行
                    RemoveWhiteLinesNearNamespace(contentLinesCopy, ref namespaceIndex);

                    //插入引用
                    InsertUsingLines(contentLines, contentLinesCopy, namespaceIndex, systemUsingLineIndexList, notSystemUsingLineIndexList);

                    //保存文件
                    if (IsChanged(contentLines, contentLinesCopy))
                    {
                        File.WriteAllLines(filePath, contentLinesCopy, CommonData.EncodingUTF8);

                        successCount++;
                    }
                }

                Console.WriteLine("MOVE USING FILES:" + successCount);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// 移除不是引用的行
        /// </summary>
        /// <param name="contentLines"></param>
        /// <param name="notSystemUsingLineIndexList"></param>
        private static void RemoveExtraLines(String[] contentLines, List<Int32> notSystemUsingLineIndexList)
        {
            //若包含以下字符，则认为不是引用语句
            String[] exceptChars = new String[] { "(", ")" };

            for (Int32 i = notSystemUsingLineIndexList.Count - 1; i >= 0; i--)
            {
                foreach (String item in exceptChars)
                {
                    if (contentLines[notSystemUsingLineIndexList[i]].Contains(item))
                    {
                        notSystemUsingLineIndexList.RemoveAt(i);
                    }
                }
            }
        }

        private static void InsertUsingLines(String[] contentLines, List<String> contentLinesCopy, Int32 namespaceIndex, List<Int32> systemUsingLineIndexList, List<Int32> notSystemUsingLineIndexList)
        {
            //先插入非系统引用

            //插入命名空间的下一行
            Int32 index = namespaceIndex + 2;

            if (notSystemUsingLineIndexList.Count > 0)
            {
                foreach (var item in notSystemUsingLineIndexList)
                {
                    contentLinesCopy.Insert(index, "    " + contentLines[item]);

                    index++;
                }

                //插入一空行
                contentLinesCopy.Insert(index, "");
            }

            //再插入系统引用
            index = namespaceIndex;

            if (systemUsingLineIndexList.Count > 0)
            {
                //先插入空行
                contentLinesCopy.Insert(index, "");

                foreach (var item in systemUsingLineIndexList)
                {
                    contentLinesCopy.Insert(index, contentLines[item]);

                    index++;
                }
            }
        }

        /// <summary>
        /// 获取需要移动的项
        /// </summary>
        /// <param name="contentLines"></param>
        /// <returns></returns>
        private static List<Int32> GetNotSystemUsingList(String[] contentLines)
        {
            List<Int32> usingLineIndexList = GetStartIndexList(contentLines, "using ");

            List<Int32> systemUsingLineIndexList = GetStartIndexList(contentLines, "using System");

            var list = usingLineIndexList.Except(systemUsingLineIndexList).ToList();

            //若包含以下字符，则认为不是引用语句
            String[] exceptChars = new String[] { "=", "(", ")" };

            for (Int32 i = list.Count - 1; i >= 0; i--)
            {
                foreach (String item in exceptChars)
                {
                    if (contentLines[list[i]].Contains(item))
                    {
                        list.RemoveAt(i);

                        break;
                    }
                }
            }

            list.Sort();

            return list;
        }

        /// <summary>
        /// 获取以指定字符开头的位置
        /// </summary>
        /// <param name="contentLines"></param>
        /// <param name="startChars">开始字符</param>
        /// <returns></returns>
        private static List<Int32> GetStartIndexList(String[] contentLines, String startChars)
        {
            List<Int32> result = new List<Int32>();

            for (Int32 i = 0; i < contentLines.Length; i++)
            {
                if (contentLines[i].StartsWith(startChars))
                {
                    result.Add(i);
                }
            }

            result.Sort();

            return result;
        }

        /// <summary>
        /// 获取不以指定字符开头的位置
        /// </summary>
        /// <param name="contentLines"></param>
        /// <param name="startChars">开始字符</param>
        /// <returns></returns>
        private static List<Int32> GetNotStartIndexList(String[] contentLines, String startChars)
        {
            List<Int32> result = new List<Int32>();

            for (Int32 i = 0; i < contentLines.Length; i++)
            {
                if (!contentLines[i].StartsWith(startChars))
                {
                    result.Add(i);
                }
            }

            return result;
        }


        /// <summary>
        /// 移除部分行
        /// </summary>
        /// <param name="contentLines"></param>
        /// <param name="indexs"></param>
        /// <returns></returns>
        private static void RemoveLines(List<String> contentLines, List<Int32> indexs)
        {
            var sortedIndexs = indexs.OrderByDescending(a => a);

            for (Int32 i = contentLines.Count - 1; i >= 0; i--)
            {
                if (indexs.Contains(i))
                {
                    contentLines.RemoveAt(i);
                }
            }
        }

        private static void RemoveWhiteLinesNearNamespace(List<String> contentLines, ref Int32 namespaceIndex)
        {
            //移除命名空间后面的空行
            Int32 index = namespaceIndex + 2;

            while (String.IsNullOrWhiteSpace(contentLines[index]))
            {
                contentLines.RemoveAt(index);
            }

            //移除命名空间前面的空行
            index = namespaceIndex - 1;

            while (String.IsNullOrWhiteSpace(contentLines[index]))
            {
                contentLines.RemoveAt(index);

                index--;

                namespaceIndex--;
            }
        }

        /// <summary>
        /// 检测内容是否发生了改变
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="contentsCopy"></param>
        /// <returns></returns>
        private static Boolean IsChanged(String[] contents, List<String> contentsCopy)
        {
            if (contents.Length != contentsCopy.Count)
            {
                return true;
            }
            else
            {
                for (Int32 i = 0; i < contents.Length; i++)
                {
                    if (contents[i].Trim() != contentsCopy[i].Trim())
                        return true;
                }
            }

            return false;
        }
    }
}
