using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Public.CSUtil.Json;
using Public.CSUtil.Log;

namespace Blog
{
    public class ChatBLL
    {

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg">消息体</param>
        /// <param name="onlineUsers">当前在线用户集</param>
        /// <returns></returns>
        public static async Task SendMsg(MessageModel msg, Dictionary<String, WebSocket> onlineUsers, String currentUser)
        {
            //对发送内容进行HTML编码
            msg.Content = HttpUtility.HtmlEncode(msg.Content);
            String msgString = JsonUtil.Serialize(msg);

            Byte[] bts = Encoding.UTF8.GetBytes(msgString);
            foreach (var item in onlineUsers)
            {
                try
                {
                    if (msg.To != null && msg.To.Count > 0)
                    {
                        if (msg.To.Contains(item.Key))
                            await item.Value.SendAsync(new ArraySegment<Byte>(bts), WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                    else if (item.Key != currentUser)
                    {   //发送给自己以外的人
                        await item.Value.SendAsync(new ArraySegment<Byte>(bts), WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                }
                catch (Exception e)
                {
                    LogUtil.Error($"{e}\r\n{msgString}");
                }
            }
        }

        public static String GetRandomString(Int32 lenth)
        {
            Random rnd = new Random();
            String chars = "ABCDEFGHJKMNPQRSTWXYZabcdefhijkmnprstwxyz2345678";
            String result = String.Empty;
            for (Int32 i = 0; i < lenth; i++)
            {
                result += chars[rnd.Next(0, chars.Length)];
            }
            return result;
        }

    }

    /// <summary>
    /// 消息体
    /// </summary>
    public class MessageModel
    {
        /// <summary>
        /// 消息的类型
        /// </summary>
        public MsgType MsgType { get; set; }
        /// <summary>
        /// 命令的类型
        /// </summary>
        public CommandType CommandType { get; set; }
        /// <summary>
        /// 消息的内容
        /// </summary>
        public String Content { get; set; }
        /// <summary>
        /// 发送人
        /// </summary>
        public String From { get; set; }
        /// <summary>
        /// 消息发送到的用户集合
        /// </summary>
        public List<String> To = new List<String>();
    }

    /// <summary>
    /// 消息的类型
    /// </summary>
    public enum MsgType
    {
        /// <summary>
        /// 命令
        /// </summary>
        Command,
        /// <summary>
        /// 文本
        /// </summary>
        Text,
    }

    public enum CommandType
    {
        /// <summary>
        /// 新用户上线
        /// </summary>
        AddUser,
        /// <summary>
        /// 用户下线
        /// </summary>
        RemoveUser,
    }
}