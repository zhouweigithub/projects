namespace CreateDBmodels.Common
{
    /// <summary>
    /// 日志操作类
    /// 文件功能描述：公共类，系统日志，通过本类可以快速地将系统日志写入磁盘文件
    /// 依赖说明：通过Config读取输出日志等级
    /// 异常处理：捕获异常，当写日志异常时，忽略错误
    /// </summary>
    public class WriteLog
    {
        /// <summary>
        /// 系统日志等级
        /// </summary>
        public enum LogLevel
        {
            /// <summary>
            /// 调试
            /// </summary>
            Info = 0,
            /// <summary>
            /// 错误
            /// </summary>
            Warn = 1,
            /// <summary>
            /// 致命
            /// </summary>
            Debug = 2,
            /// <summary>
            /// 信息
            /// </summary>
            Error = 3,
            /// <summary>
            /// 警告
            /// </summary>
            Fatal = 4,
        }

        private static System.String _logFilePath = Config.GetConfigToString("LogFilePath");
        //private static int _logFileMaxSize = Config.GetConfigToInt("LogFileMaxSize");
        //private static int _logWriteLevel = WebConfigData.LogWriteLevel;

        /// <summary>
        /// 日志文件路径
        /// </summary>
        public static System.String LogFilePath
        {
            get
            {
                if (System.String.IsNullOrWhiteSpace(_logFilePath.ToString()))
                {
                    return Config.BaseDirectory;
                }
                else
                {
                    return _logFilePath;
                }
            }
        }

        /// <summary>
        /// 写日志文件
        /// </summary>
        /// <param name="logLevel">日志等级</param>
        /// <param name="message">日志内容</param>
        public static void Write(LogLevel logLevel, System.String message)
        {
            Util.Log.LogUtil.SetLogPath(LogFilePath + "Log");
            Util.Log.LogUtil.Write(message, (Util.Log.LogType)logLevel);
        }
    }
}
