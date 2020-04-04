// ****************************************
// FileName:LogUtil.cs
// Description:日志助手类
// Tables:Nothing
// Author:Jordan Zuo
// Create Date:2013-10-10
// Revision History:
// ****************************************

using System;

namespace Util.Log
{
    /// <summary>
    /// 日志助手类
    /// </summary>
    public static class LogUtil
    {
        //定义锁变量
        private static Object mLockObj = new Object();

        //定义日志文件存放的路径
        private static String mLogPath = String.Empty;

        /// <summary>
        /// 获取日志文件存放的路径
        /// </summary>
        /// <returns>日志文件存放的路径</returns>
        public static String GetLogPath()
        {
            return mLogPath;
        }

        /// <summary>
        /// 设置日志文件存放的路径
        /// </summary>
        /// <param name="logPath">日志文件存放的路径</param>
        public static void SetLogPath(String logPath)
        {
            mLogPath = logPath;
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="logType">日志类型</param>
        /// <param name="ifIncludeHour">是否包含小时</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static void Write(String msg, LogType logType, Boolean ifIncludeHour = true)
        {
            if (String.IsNullOrEmpty(mLogPath)) throw new ArgumentNullException("FilePath", "File not exists.Please set the file path.");

            //获取当前时间
            DateTime dtNow = DateTime.Now;

            //构造文件路径和文件名
            String filePath = String.Format("{0}\\{1}\\{2}", mLogPath, dtNow.Year, dtNow.Month);
            String fileName = String.Empty;
            if (ifIncludeHour)
            {
                fileName = String.Format("{0}-{1}.{2}.{3}", DateTimeUtil.GetShortGreenWichTime(dtNow), dtNow.Hour.ToString(), Enum.GetName(typeof(LogType), logType), "txt");
            }
            else
            {
                fileName = String.Format("{0}.{1}.{2}", DateTimeUtil.GetShortGreenWichTime(dtNow), Enum.GetName(typeof(LogType), logType), "txt");
            }

            try
            {
                //独占方式，因为文件只能由一个进程写入
                lock (mLockObj)
                {
                    msg = String.Format("{0}{1} {2}", "#", DateTimeUtil.GetGreenWichTime(dtNow), msg);
                    FileUtil.WriteFile(filePath, fileName, true, msg, "---------------------------------------------------------------------");
                }
            }
            catch
            {
                //由于这很可能是在出现异常后的记录日志的行为，所以此处不对外再抛出异常，以免出现未处理的异常
            }
        }
    }
}