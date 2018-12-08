using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.Common
{

    /// <summary>
    /// 日志辅助类
    /// </summary>
    public class LogUtil
    {
        private static object lockObj = new object();

        public static void Write(string msg, LogType type)
        {
            lock (lockObj)
            {
                string rootFolder = AppDomain.CurrentDomain.BaseDirectory;
                string folder = $"{rootFolder}Log\\{DateTime.Now.ToString("yyyy")}\\{DateTime.Now.ToString("MM")}";
                string fileName = $"{DateTime.Now.ToString("yyyy-MM-dd-HH")}.{Enum.GetName(typeof(LogType), type)}.txt";
                string fullPath = $"{folder}\\{fileName}";

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string msgTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                File.AppendAllText(fullPath, $"#{msgTime} {msg}\r\n---------------------------------------------------------------------\r\n", Encoding.UTF8);
            }
        }
    }

    /// <summary>
    /// 日志类型
    /// </summary>
    public enum LogType
    {
        Info,
        Warn,
        Debug,
        Error
    }
}
