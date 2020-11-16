// ***********************************************************************************
// �ļ����ƣ�ExegesisBLL.cs
// ����������cs�ļ���Ӷ���ע��
// ���ݱ�
// ���ߣ���Χ
// ���ڣ�2020-03-30
// �޸ļ�¼��
// ***********************************************************************************
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace FormatCode
{
    /// <summary>
    /// cs�ļ���Ӷ���ע��
    /// </summary>
    public static class ExegesisBLL
    {

        private static readonly String author = ConfigurationManager.AppSettings.Get("Author");

        /// <summary>
        /// cs�ļ���Ӷ���ע�ͣ�����ע�͵Ĳ������
        /// </summary>
        /// <param name="files"></param>
        public static void Do(List<String> files)
        {
            Console.WriteLine("ADD TOP COMMENT...");

            Int32 successCount = 0;

            foreach (String filePath in files)
            {
                var content = Util.ReadFileText(filePath);

                content = content.Trim();

                //���֮ǰûд��ע�ͣ������ע��
                if (!content.StartsWith("//***********************") && !content.StartsWith("// ***********************"))
                {
                    String fileName = Path.GetFileName(filePath);

                    File.WriteAllText(filePath, GetExegesisContent(fileName) + content, CommonData.EncodingUTF8);

                    successCount++;
                }

            }

            Console.WriteLine("ADD TOP COMMENTS FILES:" + successCount);
        }

        /// <summary>
        /// ��ȡ�ļ�ע������
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static String GetExegesisContent(String fileName)
        {
            return $@"// ****************************************************
// FileName:{fileName}
// Description:
// Tables:
// Author:{author}
// Create Date:{DateTime.Now:yyyy-MM-dd}
// Revision History:
// ****************************************************
";
        }
    }
}
