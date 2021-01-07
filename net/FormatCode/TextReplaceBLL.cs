
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace FormatCode
{
    /// <summary>
    /// 替换项目中的部分字符
    /// </summary>
    public static class TextReplaceBLL
    {
        public static void Do()
        {
            Console.WriteLine("REPLACE TEXT");

            Console.Write("INPUT FOLDER ");

            String folder = Console.ReadLine();

            folder = folder.Trim();

            if (String.IsNullOrEmpty(folder))
            {
                Console.WriteLine("INPUT ERROR");
            }
            else
            {
                Doing(folder);
            }

            Console.Write("PRESS ANY KEY TO CONTINUE...");

            Console.ReadKey();

        }

        public static void Doing(String path)
        {
            Console.WriteLine("REPLACE TEXT...");
            try
            {
                //文件类型限定
                String replaceTextFileLimit = ConfigurationManager.AppSettings.Get("ReplaceTextFileLimit");

                String[] filterList = replaceTextFileLimit.Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                Dictionary<String, String> replaceDic = GetRepalcePaic();


                foreach (var filter in filterList)
                {
                    String filterString = filter.Trim();

                    String[] files = Directory.GetFiles(path, filterString, SearchOption.AllDirectories);

                    Int32 successCount = 0;

                    foreach (String filePath in files)
                    {
                        //原始文本
                        String content = Util.ReadFileText(filePath);
                        String newContent = content;

                        foreach (var replaceInfo in replaceDic)
                        {
                            //替换后的文本
                            newContent = newContent.Replace(replaceInfo.Key, replaceInfo.Value);
                        }
                        if (newContent != content)
                        {
                            File.WriteAllText(filePath, newContent, CommonData.EncodingUTF8);

                            successCount++;
                        }
                    }

                    Console.WriteLine($"REPLACE TEXT FILES，{filterString}:{successCount}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// 获取替换的数据组
        /// </summary>
        /// <returns></returns>
        private static Dictionary<String, String> GetRepalcePaic()
        {
            Dictionary<String, String> result = new Dictionary<String, String>();
            String replaceTextPair = ConfigurationManager.AppSettings.Get("ReplaceTextPair");
            if (String.IsNullOrWhiteSpace(replaceTextPair))
            {
                return result;
            }
            else
            {
                String[] replacePairArray = replaceTextPair.Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in replacePairArray)
                {
                    String[] replacePair = item.Split(new Char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    result.Add(replacePair[0], replacePair.Length > 1 ? replacePair[1] : String.Empty);
                }
            }

            return result;
        }

    }
}
