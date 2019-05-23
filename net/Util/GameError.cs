//***********************************************************************************
//文件名称：GameError.cs
//功能描述：游戏错误类
//数据表：Nothing
//作者：Jordan
//日期：2014-03-25 11:34:00
//修改记录：
//***********************************************************************************

using System;

namespace Util
{
    /// <summary>
    /// 游戏错误类
    /// </summary>
    public sealed class GameError
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        public String Message { get; set; }

        /// <summary>
        /// 是否已处理
        /// </summary>
        public Boolean IfHandled { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">错误信息</param>
        public GameError(String message)
        {
            this.Message = message;
            this.IfHandled = false;
        }
    }
}