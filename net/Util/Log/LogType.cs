// ****************************************
// FileName:LogType.cs
// Description:日志类型
// Tables:Nothing
// Author:Jordan Zuo
// Create Date:2013-10-10
// Revision History:
// ****************************************

namespace Util.Log
{
    /// <summary>
    /// 日志类型
    /// </summary>
    public enum LogType : byte
    {
        /// <summary>
        /// 信息
        /// </summary>
        Info,

        /// <summary>
        /// 警告
        /// </summary>
        Warn,

        /// <summary>
        /// 调试
        /// </summary>
        Debug,

        /// <summary>
        /// 错误
        /// </summary>
        Error,

        /// <summary>
        /// 致命
        /// </summary>
        Fatal
    }
}