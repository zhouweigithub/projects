// ****************************************************
// FileName:CheckText.cs
// Description:
// Tables:
// Author:ZhouWei
// Create Date:2020-11-11
// Revision History:
// ****************************************************
using System;
using System.IO;

namespace FormatCode
{
    /// <summary>
    /// 检查文件中是否带有moqikaka相关内容
    /// </summary>
    public class CheckText
    {
        public static void Do(String path)
        {
            String[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);

            CheckFileName(files);

            CheckFileContent(files);
        }

        private static void CheckFileName(String[] files)
        {
            foreach (String filePath in files)
            {
                String tmp = filePath.ToLower();

                if (tmp.Contains("moqikaka") && !tmp.Contains("\\bin\\") && !tmp.Contains("\\obj\\") && !tmp.Contains(".vs") && !tmp.Contains(".git") && !tmp.Contains("\\lib\\") && !tmp.Contains("\\Dependance\\"))
                {
                    Console.WriteLine("fileNameError:" + filePath);
                }
            }
        }

        private static void CheckFileContent(String[] files)
        {
            foreach (String filePath in files)
            {
                String tmp = filePath.ToLower();

                if (!tmp.Contains("\\bin\\") && !tmp.Contains("\\obj\\") && !tmp.Contains(".vs") && !tmp.Contains(".git") && !tmp.Contains("\\lib\\") && !tmp.Contains("\\Dependance\\"))
                {
                    String content = Util.ReadFileText(filePath);

                    String tmpContent = content;//.ToLower();

                    if (tmpContent.Contains("Moqikaka"))
                    {
                        Console.WriteLine("find Moqikaka:" + filePath);
                    }

                    //if (tmpContent.Contains("moqikaka"))
                    //{
                    //    Console.WriteLine("find moqikaka:" + filePath);
                    //}
                    if (tmpContent.Contains("摩奇卡卡"))
                    {
                        Console.WriteLine("find 摩奇卡卡:" + filePath);
                    }
                }
            }
        }

        private static void ReplaceContent(String[] files)
        {
            foreach (String filePath in files)
            {
                String tmp = filePath.ToLower();

                if (!tmp.Contains("\\bin\\") && !tmp.Contains("\\obj\\") && !tmp.Contains(".vs") && !tmp.Contains(".git") && !tmp.Contains("\\lib\\") && !tmp.Contains("\\Dependance\\"))
                {
                    Boolean isEdit = false;

                    String content = Util.ReadFileText(filePath);

                    if (content.Contains("摩奇卡卡"))
                    {
                        Console.WriteLine("Replace 摩奇卡卡:" + filePath);

                        content = content.Replace("摩奇卡卡", "79游");

                        isEdit = true;
                    }

                    if (content.Contains("moqikaka"))
                    {
                        Console.WriteLine("Replace moqikaka:" + filePath);

                        content = content.Replace("moqikaka", "79yougame");

                        isEdit = true;
                    }

                    if (content.Contains("Moqikaka"))
                    {
                        Console.WriteLine("Replace Moqikaka:" + filePath);

                        content = content.Replace("Moqikaka", "79yougame");

                        isEdit = true;
                    }

                    if (isEdit)
                        File.WriteAllText(filePath, content, CommonData.EncodingUTF8);
                }
            }
        }
    }
}
