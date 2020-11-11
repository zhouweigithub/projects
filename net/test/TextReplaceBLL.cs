// ****************************************************
// FileName:TextReplaceBLL.cs
// Description:
// Tables:
// Author:ZhouWei
// Create Date:2020-04-27
// Revision History:
// ****************************************************
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace test
{
    /// <summary>
    /// �滻��Ŀ�еĲ����ַ���������Ŀ���Ƴ�Moqikaka�ַ����Լ�����Util������
    /// </summary>
    public static class TextReplaceBLL
    {

        /// <summary>
        /// ���������ͳ��ֶ��ʱ���������⣬��Ҫѭ������
        /// </summary>
        private static readonly String[] filterList = new String[] { "*.csproj", "*.cs", "*.cshtml", "*.ashx", "*.asax" };

        public static void Do(String path)
        {
            Console.WriteLine("REPLACE TEXT...");
            try
            {

                foreach (var filter in filterList)
                {
                    String[] files = Directory.GetFiles(path, filter, SearchOption.AllDirectories);

                    Regex regUtil = new Regex(" Util\\.");

                    Int32 successCount = 0;

                    foreach (String filePath in files)
                    {
                        //ԭʼ�ı�
                        String content = Util.ReadFileText(filePath);

                        //�滻����ı�
                        String newContent = content;

                        if (filter == "*.csproj")
                        {
                            //String folderPath = Path.GetDirectoryName(filePath);
                            //String utilPath = GetUtilPath(folderPath);
                            newContent = newContent.Replace("Util.dll", "Public.CSUtil.dll")
                                //.Replace(@"..\Lib\Util.dll", utilPath)
                                .Replace("Include=\"Util", "Include=\"CSUtil")
                                .Replace("Moqikaka.", "");
                        }
                        else
                        {
                            newContent = newContent
                                .Replace("Moqikaka.Util", "Public.CSUtil")
                                .Replace("Moqikaka.", "");

                            newContent = regUtil.Replace(newContent, " Public.CSUtil.");
                        }

                        if (newContent != content)
                        {
                            File.WriteAllText(filePath, newContent, CommonData.EncodingUTF8);

                            successCount++;
                        }
                    }

                    Console.WriteLine("REPLACE TEXT FILES:" + successCount);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// ��ȡUtil.dll�����λ��
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        private static String GetUtilPath(String folderPath)
        {
            String parent = String.Empty;

            String path = @"Public.CSUtil.dll";

            String[] dllFolderList = new String[] { "Lib" };

            for (Int32 i = 0; i < 5; i++)
            {
                parent += @"..\";

                foreach (var lib in dllFolderList)
                {
                    String fullPath = Path.Combine(folderPath, parent, lib, path);

                    if (File.Exists(fullPath))
                    {
                        Console.WriteLine("�ҵ� Public.CSUtil.dll ·����" + fullPath);

                        return Path.Combine(parent, lib, path); ;
                    }
                }
            }

            Console.WriteLine("δ�ҵ� Public.CSUtil.dll ·��");

            return String.Empty;
        }

    }
}
