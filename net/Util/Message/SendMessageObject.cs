// ****************************************
// FileName:SendMessageObject.cs
// Description:发送的消息对象
// Tables:Nothing
// Author:Jordan Zuo
// Create Date:2014-07-14
// Revision History:
// ****************************************

using System;

namespace Util.Message
{
    /// <summary>
    /// 发送的消息对象
    /// </summary>
    public class SendMessageObject
    {
        /// <summary>
        /// 要发送的地址
        /// </summary>
        public String Url { get; set; }

        /// <summary>
        /// 要发送的消息
        /// </summary>
        public String Message { get; set; }

        /// <summary>
        /// 是否为POST方式发送
        /// </summary>
        public Boolean IsPost { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="url">要发送的地址</param>
        /// <param name="message">要发送的消息</param>
        /// <param name="isPost">是否为POST方式发送</param>
        public SendMessageObject(String url, String message, Boolean isPost)
        {
            this.Url = url;
            this.Message = message;
            this.IsPost = isPost;
        }
    }
}