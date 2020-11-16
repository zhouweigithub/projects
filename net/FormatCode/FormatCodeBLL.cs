// ****************************************************
// FileName:FormatCodeBLL.cs
// Description:
// Tables:
// Author:ZhouWei
// Create Date:2020-04-17
// Revision History:
// ****************************************************
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace FormatCode
{
    public static class FormatCodeBLL
    {

        /// <summary>
        /// 当过滤类型出现多个时，会有问题，需要循环调用
        /// </summary>
        private const String filter = "*.cs";

        private static readonly String exceptFolders = ConfigurationManager.AppSettings.Get("ExceptFolders");

        public static void Do()
        {
            Console.WriteLine("FORMAT CODE");

            Console.Write("INPUT FOLDER ");

            String folder = Console.ReadLine();

            folder = folder.Trim();

            if (String.IsNullOrEmpty(folder))
            {
                Console.WriteLine("INPUT ERROR");
            }
            else
            {
                Do(folder);
            }

            Console.Write("PRESS ANY KEY TO CONTINUE...");

            Console.ReadKey();
        }

        /// <summary>
        /// 格式化代码
        /// </summary>
        /// <param name="path"></param>
        public static void Do(String path)
        {
            try
            {
                if (String.IsNullOrEmpty(path))
                {
                    Console.WriteLine("FOLDER EMPTY");

                    return;
                }

                if (!Directory.Exists(path))
                {
                    Console.WriteLine("FOLDER NOT EXIST");

                    return;
                }

                List<String> files = new List<String>();

                GetAllFiles(path, filter, files);

                Console.WriteLine("FILE COUNT:" + files.Count);

                ExegesisBLL.Do(files);

                MoveUsingBLL.Do(files);

                BreakLineBLL.Do(files);

                Console.WriteLine("OVER");
            }
            catch (Exception e)
            {
                Console.WriteLine();

                Console.WriteLine(e.ToString());

                Console.WriteLine();
            }
        }

        /// <summary>
        /// 获取目录中的所有文件
        /// </summary>
        /// <param name="path">目录路径</param>
        /// <param name="filter">过滤条件</param>
        /// <param name="result">结果集合</param>
        private static void GetAllFiles(String path, String filter, List<String> result)
        {
            String[] files = Directory.GetFiles(path, filter, SearchOption.TopDirectoryOnly);

            result.AddRange(files);

            String[] folders = Directory.GetDirectories(path);

            String[] exceptFolderArray = exceptFolders.Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var folderItem in folders)
            {
                String folderName = Path.GetFileName(folderItem);

                Boolean isFiltered = false;

                foreach (var filterItem in exceptFolderArray)
                {
                    if (String.Compare(folderName, filterItem, true) == 0)
                    {
                        isFiltered = true;

                        break;
                    }
                }

                if (isFiltered)
                    continue;

                GetAllFiles(folderItem, filter, result);
            }
        }
    }
}
