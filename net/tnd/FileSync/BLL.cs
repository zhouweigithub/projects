using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileSync
{
    public class BLL
    {
        private static string sourceFolder;
        private static string aimFolder;
        private static string fileLimit;
        /// <summary>
        /// 原始目录中的文件
        /// </summary>
        private static List<string> sourceFileList;
        /// <summary>
        /// 目标目录中的文件
        /// </summary>
        private static List<string> aimFileList;
        /// <summary>
        /// 需要同步的文件
        /// </summary>
        private static List<string> syncFileList;
        /// <summary>
        /// 复制失败的文件
        /// </summary>
        private static List<string> failedFileList;
        /// <summary>
        /// 正在复制的文件
        /// </summary>
        private static string currentCopyFile;
        /// <summary>
        /// 已复制成功的数量
        /// </summary>
        private static int successCount = 0;
        public static int SuccessCount => successCount;
        public static int AllSysncCount => syncFileList.Count;
        private static Thread thread;


        public static event EventHandler StartCheckHanlder;
        public static event EventHandler StartCopyHanlder;
        public static event EventHandlerString CoppingHanlder;
        public static event EventHandler EndCopyHanlder;

        static BLL()
        {
            sourceFileList = new List<string>();
            aimFileList = new List<string>();
            syncFileList = new List<string>();
            failedFileList = new List<string>();
        }

        public static void Start(string _sourceFolder, string _aimFolder, string _fileLimit)
        {
            sourceFolder = _sourceFolder;
            aimFolder = _aimFolder;
            fileLimit = _fileLimit;
            thread = new Thread(CheckFiles) { IsBackground = true };
            thread.Start();
        }

        public static void Close()
        {
            if (thread != null && thread.IsAlive)
                thread.Abort();

            //删除当前正在复制的那个文件
            if (!string.IsNullOrEmpty(currentCopyFile))
            {
                string copyingAimFile = currentCopyFile.Replace(sourceFolder, aimFolder);
                if (File.Exists(copyingAimFile))
                    File.Delete(copyingAimFile);
            }
        }

        private static void CheckFiles()
        {
            successCount = 0;
            sourceFileList.Clear();
            aimFileList.Clear();
            syncFileList.Clear();
            failedFileList.Clear();

            StartCheckHanlder?.Invoke(null, EventArgs.Empty);

            if (!Directory.Exists(sourceFolder))
                throw new Exception("原始目录不存在");
            if (!Directory.Exists(aimFolder))
                Directory.CreateDirectory(aimFolder);

            LogWrite("开始检测缺失文件");

            GetAllFiles(sourceFolder, fileLimit, true, sourceFileList);
            GetAllFiles(aimFolder, fileLimit, true, aimFileList);

            //移除外层目录字符
            for (int i = 0; i < sourceFileList.Count; i++)
            {
                sourceFileList[i] = sourceFileList[i].Replace(sourceFolder, string.Empty);
            }
            for (int i = 0; i < aimFileList.Count; i++)
            {
                aimFileList[i] = aimFileList[i].Replace(aimFolder, string.Empty);
            }

            syncFileList = sourceFileList.Except(aimFileList).ToList();

            StartCopyHanlder?.Invoke(null, EventArgs.Empty);

            LogWrite($"任务开始\r\n原始目录 {sourceFolder}\r\n目标目录 {aimFolder}\r\n文件限定 {fileLimit}\r\n待复制文件数量 {syncFileList.Count}");

            if (syncFileList.Count > 0)
            {
                SyncFiles();
            }

            LogWrite("任务结束");
            EndCopyHanlder?.Invoke(null, EventArgs.Empty);
        }

        private static void SyncFiles()
        {
            successCount = 0;
            foreach (var item in syncFileList)
            {
                currentCopyFile = sourceFolder + item;
                try
                {
                    string aimPath = Path.Combine(aimFolder, item.TrimStart(new char[] { '\\' }));
                    CoppingHanlder?.Invoke(null, new EventArgsString() { Value = currentCopyFile });

                    //创建目录
                    string newFolder = Path.GetDirectoryName(aimPath);
                    if (!Directory.Exists(newFolder))
                        Directory.CreateDirectory(newFolder);

                    File.Copy(currentCopyFile, aimPath);

                    successCount++;
                }
                catch (Exception e)
                {
                    failedFileList.Add(currentCopyFile);
                    LogWrite("复制失败 " + currentCopyFile + "\r\n" + e.Message);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="searchFlag">文件过滤项 (*.exe|*.txt)
        /// *（星号）在该位置的零个或多个字符。
        /// ?（问号）在该位置的零个或一个字符。</param>
        private static void GetAllFiles(string path, string searchFlag, bool containsSubFolder, List<string> result)
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
                    result.Add(file);
                }

                if (containsSubFolder)
                {
                    string[] subDirs = Directory.GetDirectories(path);
                    foreach (string folder in subDirs)
                    {   //递归查找子目录
                        if (!folder.Contains("$RECYCLE.BIN"))
                            GetAllFiles(folder, searchFlag, containsSubFolder, result);
                    }
                }
            }
            catch (Exception e)
            {
                LogWrite(e.Message);
            }
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

    public class EventArgsString : EventArgs
    {
        public string Value;
    }

    public delegate void EventHandlerString(object sender, EventArgsString e);
}
