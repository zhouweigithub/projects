using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace CheckFileRepeat
{
    public class BLL
    {
        private static Dictionary<string, List<string>> sources = new Dictionary<string, List<string>>();

        public static void CheckRepeatFiles(string path, string searchFlag, bool containsSubFolder)
        {
            sources.Clear();
            GetAllFiles(path, searchFlag, containsSubFolder);
            sources = sources.Where(a => a.Value.Count > 1).ToDictionary(b => b.Key, b => b.Value);
        }

        public static List<string> GetFilePaths(string fileName)
        {
            var files = sources.FirstOrDefault(a => a.Key == fileName);
            return files.Value;
        }

        public static List<FileModel> GetRepeatedFiles()
        {
            List<FileModel> result = new List<FileModel>();
            foreach (var item in sources)
            {
                result.Add(new FileModel()
                {
                    FileName = item.Key,
                    RepeatTimes = item.Value.Count
                });
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="searchFlag">文件过滤项 (* 和 ?)</param>
        private static void GetAllFiles(string path, string searchFlag, bool containsSubFolder)
        {
            if (!Directory.Exists(path))
                throw new Exception("指定的路径不存在");

            try
            {
                string[] files = null;
                if (!string.IsNullOrWhiteSpace(searchFlag))
                {
                    List<string> fileList = new List<string>();
                    string[] flagArray = searchFlag.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string item in flagArray)
                    {
                        fileList.AddRange(Directory.GetFiles(path, item).ToList());
                    }
                    files = fileList.Distinct().ToArray();
                }
                else
                    files = Directory.GetFiles(path);

                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);
                    if (!sources.ContainsKey(fileName))
                        sources.Add(fileName, new List<string>() { file });
                    else
                        sources[fileName].Add(file);
                }

                if (containsSubFolder)
                {
                    string[] subDirs = Directory.GetDirectories(path);
                    foreach (string folder in subDirs)
                    {   //递归查找子目录
                        if (!folder.Contains("$RECYCLE.BIN"))
                            GetAllFiles(folder, searchFlag, containsSubFolder);
                    }
                }
            }
            catch (Exception e)
            {
                LogWrite(e.Message);
            }
        }



        /// <summary>
        /// 利用系统默认程序打开文件
        /// </summary>
        /// <param name="filePath"></param>
        public static void DataFileView(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                return;
            }

            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = filePath;
            psi.UseShellExecute = true;

            Process.Start(psi);
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="msg"></param>
        private static void LogWrite(string msg)
        {
            string fileName = DateTime.Now.ToString("yyyy-MM-dd-HH");
            string timeString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string path = AppDomain.CurrentDomain.BaseDirectory + fileName + ".txt";
            File.AppendAllText(path, "#" + timeString + " " + msg + "\r\n", Encoding.Default);
        }
    }
}
