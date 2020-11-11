//***********************************************************************************
//�ļ����ƣ�ExegesisBLL.cs
//�����������ƶ�using���ô��룬ϵͳ���÷ŵ������ռ��⣬�������÷ŵ������ռ���
//���ݱ�
//���ߣ���Χ
//���ڣ�2020-03-30
//�޸ļ�¼��
//***********************************************************************************
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace test
{
    /// <summary>
    /// �ƶ�using���ô��룬ϵͳ���÷ŵ������ռ��⣬�������÷ŵ������ռ���
    /// </summary>
    public static class MoveUsingBLL
    {
        /// <summary>
        /// ���������ͳ��ֶ��ʱ���������⣬��Ҫѭ������
        /// </summary>
        private const String filter = "*.cs";

        /// <summary>
        /// 
        /// </summary>
        public static void Do(String path)
        {
            try
            {
                Console.WriteLine("MOVE USING...");

                String[] files = Directory.GetFiles(path, filter, SearchOption.AllDirectories);

                Int32 successCount = 0;

                foreach (String filePath in files)
                {
                    //���ڸ��ִ�����Ķ�ԭʼ�ı�
                    String[] contentLines = Util.ReadFileAllLines(filePath);

                    //����ԭʼ�ı�
                    List<String> contentLinesCopy = contentLines.ToList();

                    for (Int32 i = 0; i < contentLines.Length; i++)
                    {   //�Ƴ���β�հ��ַ�
                        contentLines[i] = contentLines[i].Trim();
                    }

                    //�����ռ�λ��
                    List<Int32> namespaceIndexList = GetStartIndexList(contentLines, "namespace ");

                    if (namespaceIndexList.Count == 0)
                        continue;

                    Int32 namespaceIndex = namespaceIndexList[0];

                    //ϵͳ����
                    List<Int32> systemUsingLineIndexList = GetStartIndexList(contentLines, "using System");

                    //��ϵͳ����
                    List<Int32> notSystemUsingLineIndexList = GetNotSystemUsingList(contentLines);

                    //�Ƴ�ԭʼ��using��
                    RemoveLines(contentLinesCopy, systemUsingLineIndexList.Union(notSystemUsingLineIndexList).ToList());

                    //��ȡ�µ������ռ��λ��
                    namespaceIndex = GetStartIndexList(contentLinesCopy.ToArray(), "namespace ")[0];

                    //�Ƴ������ռ丽���Ŀ���
                    RemoveWhiteLinesNearNamespace(contentLinesCopy, ref namespaceIndex);

                    //��������
                    InsertUsingLines(contentLines, contentLinesCopy, namespaceIndex, systemUsingLineIndexList, notSystemUsingLineIndexList);

                    //�����ļ�
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
        /// �Ƴ��������õ���
        /// </summary>
        /// <param name="contentLines"></param>
        /// <param name="notSystemUsingLineIndexList"></param>
        private static void RemoveExtraLines(String[] contentLines, List<Int32> notSystemUsingLineIndexList)
        {
            //�����������ַ�������Ϊ�����������
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
            //�Ȳ����ϵͳ����

            //���������ռ����һ��
            Int32 index = namespaceIndex + 2;

            if (notSystemUsingLineIndexList.Count > 0)
            {
                foreach (var item in notSystemUsingLineIndexList)
                {
                    contentLinesCopy.Insert(index, "    " + contentLines[item]);

                    index++;
                }

                //����һ����
                contentLinesCopy.Insert(index, "");
            }

            //�ٲ���ϵͳ����
            index = namespaceIndex;

            if (systemUsingLineIndexList.Count > 0)
            {
                //�Ȳ������
                contentLinesCopy.Insert(index, "");

                foreach (var item in systemUsingLineIndexList)
                {
                    contentLinesCopy.Insert(index, contentLines[item]);

                    index++;
                }
            }
        }

        /// <summary>
        /// ��ȡ��Ҫ�ƶ�����
        /// </summary>
        /// <param name="contentLines"></param>
        /// <returns></returns>
        private static List<Int32> GetNotSystemUsingList(String[] contentLines)
        {
            List<Int32> usingLineIndexList = GetStartIndexList(contentLines, "using ");

            List<Int32> systemUsingLineIndexList = GetStartIndexList(contentLines, "using System");

            var list = usingLineIndexList.Except(systemUsingLineIndexList).ToList();

            //�����������ַ�������Ϊ�����������
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
        /// ��ȡ��ָ���ַ���ͷ��λ��
        /// </summary>
        /// <param name="contentLines"></param>
        /// <param name="startChars">��ʼ�ַ�</param>
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
        /// ��ȡ����ָ���ַ���ͷ��λ��
        /// </summary>
        /// <param name="contentLines"></param>
        /// <param name="startChars">��ʼ�ַ�</param>
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
        /// �Ƴ�������
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
            //�Ƴ������ռ����Ŀ���
            Int32 index = namespaceIndex + 2;

            while (String.IsNullOrWhiteSpace(contentLines[index]))
            {
                contentLines.RemoveAt(index);
            }

            //�Ƴ������ռ�ǰ��Ŀ���
            index = namespaceIndex - 1;

            while (String.IsNullOrWhiteSpace(contentLines[index]))
            {
                contentLines.RemoveAt(index);

                index--;

                namespaceIndex--;
            }
        }

        /// <summary>
        /// ��������Ƿ����˸ı�
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
