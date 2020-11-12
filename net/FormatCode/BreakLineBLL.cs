//***********************************************************************************
//�ļ����ƣ�BreakLineBLL.cs
//����������������뻻��
//���ݱ�
//���ߣ���Χ
//���ڣ�2020-03-30
//�޸ļ�¼��
//***********************************************************************************
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace FormatCode
{
    /// <summary>
    /// ������뻻��
    /// </summary>
    public static class BreakLineBLL
    {
        /// <summary>
        /// ���������ͳ��ֶ��ʱ���������⣬��Ҫѭ������
        /// </summary>
        private const String filter = "*.cs";

        /// <summary>
        /// cs�ļ���Ӷ���ע�ͣ�����ע�͵Ĳ������
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

                //�Ƴ���β�հ��ַ�
                Regex reg = new Regex(".+; *//");

                for (Int32 i = 0; i < contentArray.Length; i++)
                {
                    contentArray[i] = contentArray[i].Trim();

                    //�Ƴ��������ע��
                    //�ֺ�λ��
                    if (!contentArray[i].StartsWith("//"))
                    {
                        Match match = reg.Match(contentArray[i]);

                        if (match.Success)
                        {
                            contentArray[i] = match.Value.TrimEnd('/').Trim();
                        }
                    }
                }

                //ԭ�ļ��Ƿ��б仯
                Boolean isChange = false;

                //���һ�в�����
                for (Int32 i = 0; i < contentArray.Length; i++)
                {
                    String lineContent = contentArray[i];

                    //���ԭ����
                    newContentList.Add(contentListCopy[i]);

                    //��ӿհ���
                    if (i < contentArray.Length - 1 && IsCodeLine(lineContent, contentArray[i + 1]))
                    {
                        newContentList.Add(String.Empty);

                        isChange = true;
                    }
                }

                //�����ļ�
                if (isChange)
                {
                    File.WriteAllLines(filePath, newContentList, CommonData.EncodingUTF8);

                    successCount++;
                }
            }

            Console.WriteLine("BREAK LINE FILES:" + successCount);
        }

        /// <summary>
        /// ��⵱ǰ���Ƿ�Ϊ��Ч�����У�ֻ����Ч�����в����������ӿ���
        /// </summary>
        /// <param name="content">��ǰ������</param>
        /// <param name="nextContent">��һ������</param>
        /// <returns></returns>
        private static Boolean IsCodeLine(String content, String nextContent)
        {
            Boolean result;

            //�ԷֺŽ�������һ���ԷֺŻ����Ž���
            result = (content.EndsWith(";") || content.EndsWith("}"))
                && (nextContent.EndsWith(";") || nextContent.EndsWith(")") || nextContent.StartsWith("//"));

            //�����ض����ַ���ͷ
            String[] notCodePrefix1 = new String[] { "using ", "namespace", "catch", "finally", "//", "{", "+", "," };

            String[] notCodePrefix2 = new String[] { "catch", "finally", "else", "}" };

            result = result && !StartWith(content, notCodePrefix1) && !StartWith(nextContent, notCodePrefix2);

            return result;
        }

        /// <summary>
        /// �Ƿ���ָ���ַ���ͷ
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private static Boolean StartWith(String content, String[] notCodePrefix)
        {
            //������Ч�����еĿ�ʼ�ַ�
            foreach (String item in notCodePrefix)
            {
                if (content.StartsWith(item))
                    return true;
            }

            return false;
        }


    }
}
