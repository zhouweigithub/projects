// ****************************************
// FileName:SendPattern.cs
// Description:发送模式
// Tables:Nothing
// Author:Jordan Zuo
// Create Date:2013-10-10
// Revision History:
// ****************************************

namespace Util.Mail
{
    /// <summary>
    /// 发送模式
    /// </summary>
    public enum SendPattern : byte
    {
        /// <summary>
        /// 同步模式
        /// </summary>
        Synchronous,

        /// <summary>
        /// 异步模式
        /// </summary>
        Asynchronous
    }
}