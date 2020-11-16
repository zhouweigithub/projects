using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.WebSockets;
using Public.CSUtil.Json;
using Public.CSUtil.Log;

namespace Blog.Controllers
{
    public class ChatController : Controller
    {
        static readonly Dictionary<String, WebSocket> clientSockets = new Dictionary<String, WebSocket>();

        // GET: Chat
        public ActionResult Index()
        {
            if (Session["token"] == null)
                Session["token"] = ChatBLL.GetRandomString(8);

            ViewBag.token = Session["token"].ToString();
            return View();
        }

        public void WebServer()
        {
            if (HttpContext.IsWebSocketRequest)
                HttpContext.AcceptWebSocketRequest(ProcessChat);
        }

        private async Task ProcessChat(AspNetWebSocketContext context)
        {

            WebSocket socket = context.WebSocket;

            //用户标识
            String userToken = context.QueryString["token"];

            if (!clientSockets.ContainsKey(userToken))
            {   //不存在，添加
                clientSockets.Add(userToken, socket);
            }
            else
            {   //当前对象不一致，更新
                if (socket != clientSockets[userToken])
                    clientSockets[userToken] = socket;
            }

            //给当前用户发送所有在线用户信息
            await ChatBLL.SendMsg(new MessageModel()
            {
                MsgType = MsgType.Command,
                CommandType = CommandType.AddUser,
                Content = String.Join("|", clientSockets.Select(a => a.Key)),
                To = new List<String>() { userToken },
            }, clientSockets, userToken);

            //通知所有用户自己上线信息
            await ChatBLL.SendMsg(new MessageModel()
            {
                MsgType = MsgType.Command,
                CommandType = CommandType.AddUser,
                Content = userToken,
            }, clientSockets, userToken);

            while (true)
            {
                try
                {
                    ArraySegment<Byte> buffer = new ArraySegment<Byte>(new Byte[2048]);
                    WebSocketReceiveResult receivedData = await socket.ReceiveAsync(buffer, CancellationToken.None);

                    if (socket.State != WebSocketState.Open)
                    {
                        clientSockets.Remove(userToken);

                        //通知所有用户
                        await ChatBLL.SendMsg(new MessageModel()
                        {
                            MsgType = MsgType.Command,
                            CommandType = CommandType.RemoveUser,
                            Content = userToken,
                        }, clientSockets, userToken);

                        break;
                    }

                    //发送过来的消息
                    String userMsg = Encoding.UTF8.GetString(buffer.Array, 0, receivedData.Count);

                    MessageModel msg = JsonUtil.Deserialize<MessageModel>(userMsg);
                    msg.From = userToken;

                    //传递信息给指定用户
                    await ChatBLL.SendMsg(msg, clientSockets, userToken);
                }
                catch (Exception e)
                {
                    LogUtil.Error(e.ToString());
                    break;
                }
            }
        }


    }
}