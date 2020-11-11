// ***********************************************************************************
// 文件名称：ExegesisBLL.cs
// 功能描述：cs文件添加顶部注释
// 数据表：
// 作者：周围
// 日期：2020-03-30
// 修改记录：
// ***********************************************************************************
using System;
using System.Configuration;
using System.IO;

namespace test
{
    /// <summary>
    /// cs文件添加顶部注释
    /// </summary>
    public static class ExegesisBLL
    {
        /// <summary>
        /// 当过滤类型出现多个时，会有问题，需要循环调用
        /// </summary>
        private const String filter = "*.cs";

        private static readonly String author = ConfigurationManager.GetSection("Author").ToString();

        /// <summary>
        /// cs文件添加顶部注释，已有注释的不再添加
        /// </summary>
        public static void Do(String path)
        {
            Console.WriteLine("ADD TOP COMMENT...");

            String[] files = Directory.GetFiles(path, filter, SearchOption.AllDirectories);

            Console.WriteLine("FILE COUNT:" + files.Length);

            Int32 successCount = 0;

            foreach (String filePath in files)
            {
                var content = Util.ReadFileText(filePath);

                content = content.Trim();

                //如果之前没写过注释，则添加注释
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
        /// 获取文件注释内容
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
