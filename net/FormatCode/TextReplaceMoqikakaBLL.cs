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

namespace FormatCode
{
    /// <summary>
    /// 替换项目中的部分字符，用于项目中移除Moqikaka字符，以及更新Util的引用
    /// </summary>
    public static class TextReplaceMoqikakaBLL
    {

        /// <summary>
        /// 当过滤类型出现多个时，会有问题，需要循环调用
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
                        //原始文本
                        String content = Util.ReadFileText(filePath);

                        //替换后的文本
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
        /// 获取Util.dll的相对位置
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
                        Console.WriteLine("找到 Public.CSUtil.dll 路径：" + fullPath);

                        return Path.Combine(parent, lib, path); ;
                    }
                }
            }

            Console.WriteLine("未找到 Public.CSUtil.dll 路径");

            return String.Empty;
        }

    }
}
