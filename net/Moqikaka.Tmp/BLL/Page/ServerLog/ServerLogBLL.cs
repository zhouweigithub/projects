using Moqikaka.Tmp.Common;
using Moqikaka.Tmp.DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Moqikaka.Tmp.BLL.Page.ServerLog
{
    public class ServerLogBLL
    {
        public static readonly string fileContentType = "text/plain";
        private static readonly string fileExtensionLimit = "*.txt";
        private static readonly string advertAbsoluteRootPath = Const.RootWebPath;
        private static readonly string currentMonthString = string.Format("\\{0}\\{1}\\", DateTime.Now.Year, DateTime.Now.Month);

        /// <summary>
        /// 读取指定类型日志的所有文件名（已按最后修改日期降序排列）
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<FileData> GetFileList(LogType type)
        {
            List<FileData> result = new List<FileData>();
            try
            {
                string absolutePath = GetLogFilePath(type);
                string[] files = FileHelper.GetFileNames(absolutePath, fileExtensionLimit, false);
                foreach (string fllFilePath in files)
                {
                    FileInfo info = new FileInfo(fllFilePath);
                    FileData data = new FileData()
                    {
                        FileName = info.Name,
                        LastModify = info.LastWriteTime,
                        Size = (long)(info.Length / 1024d)
                    };
                    result.Add(data);
                }

                result = result.OrderByDescending(a => a.LastModify).ToList();
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write(string.Format("GetFileList读取日志文件列表失败：type={0}  \r\n {1}", type, e.ToString()), Util.Log.LogType.Error);
            }
            return result;
        }

        /// <summary>
        /// 根据日志文件名获取日志文件内容流
        /// </summary>
        /// <param name="type">站点类型</param>
        /// <param name="fileName">仅文件名</param>
        /// <returns></returns>
        public static byte[] GetFileBytes(LogType type, string fileName)
        {
            try
            {
                string absolutePath = Path.Combine(GetLogFilePath(type), fileName);
                string content = File.ReadAllText(absolutePath, Encoding.UTF8);
                byte[] bytes = Encoding.Default.GetBytes(content);
                return bytes;
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write(string.Format("GetFileBytes读取日志文件内容失败：type={0}  fileName={1} \r\n {2}", type, fileName, e.ToString()), Util.Log.LogType.Error);
            }
            return new byte[] { };
        }

        /// <summary>
        /// 获取日志文件目录的绝对路径
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static string GetLogFilePath(LogType type)
        {
            string absolutePath = string.Empty;
            switch (type)
            {
                case LogType.None:
                    break;
                case LogType.CurrentWeb:
                    absolutePath = advertAbsoluteRootPath;
                    break;
                default:
                    absolutePath = advertAbsoluteRootPath;
                    break;
            }
            absolutePath = absolutePath + "\\Log" + currentMonthString;
            return absolutePath;
        }

        public class FileData
        {
            public string FileName;
            public DateTime LastModify;
            public long Size;
        }

        public enum LogType
        {
            None,
            CurrentWeb
        }
    }
}
