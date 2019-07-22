using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WW
{

    /// <summary>  
    /// 客户端请求的Socket包装类  
    /// 作者：周公  
    /// 编写时间：2009-03-18  
    /// </summary>  
    public class ClientSocket : IDisposable
    {
        private Socket sender = null;
        private bool isListening = false;
        //定义用于接收服务器响应的存储区.  
        private readonly byte[] bytes = new byte[1024];
        //用于终止Soket的消息  
        private readonly string shutDownMessage = "<EOF>";

        public ClientSocket()
        {
            try
            {
                //设置要连接的主机信息并使用11000作为监听端口.  
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

                // 创建一个TCP/IP协议的socket连接  
                sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                sender.Connect(remoteEP);
                isListening = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void StartClient()
        {
            // 连接到远程主机，并捕获所有信息  
            try
            {
                Console.WriteLine("连接到主机{0}", sender.RemoteEndPoint.ToString());
                OutParameters();
                string consoleMessage = Console.ReadLine();
                while (isListening && !string.IsNullOrEmpty(consoleMessage))
                {
                    consoleMessage = consoleMessage.ToLower();
                    if (consoleMessage == "bye")
                    {
                        SendShutDownMessage();
                    }
                    else
                    {
                        string resultMessage = SendMessage(consoleMessage);
                        Console.WriteLine(resultMessage);
                        OutParameters();
                        consoleMessage = Console.ReadLine();
                        Console.WriteLine("consoleMessage=" + consoleMessage);
                    }
                }

            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("参数异常 : {0}", ane.ToString());
            }
            catch (SocketException se)
            {
                Console.WriteLine("出现Socket异常: {0}", se.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("出现了异常 : {0}", e.ToString());
            }

        }
        /// <summary>  
        /// 向远程主机发送信息  
        /// </summary>  
        /// <param name="message">要发送的信息</param>  
        public string SendMessage(string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            sender.Send(buffer);
            int count = sender.Receive(bytes);
            return Encoding.UTF8.GetString(bytes, 0, count);
        }
        /// <summary>  
        /// 向服务器发送关闭Socket信息，并中止与服务器的连接  
        /// </summary>  
        public void SendShutDownMessage()
        {
            SendMessage(shutDownMessage);
            Console.WriteLine("已经关闭与服务器的连接");
            isListening = false;
            Environment.Exit(0);
        }

        private void OutParameters()
        {
            Console.WriteLine("参数说明：");
            Console.WriteLine("获取服务器日期：date");
            Console.WriteLine("获取服务器时间：time");
            Console.WriteLine("获取服务器日期时间：datetime");
            Console.WriteLine("获取服务器年份：year");
            Console.WriteLine("获取服务器月份：month");
            Console.WriteLine("获取服务器天数：day");
            Console.WriteLine("关闭连接：bye");
            Console.WriteLine("请输入你要进行的操作：");
        }

        //public static int Main(String[] args)
        //{
        //    ClientSocket client = new ClientSocket();
        //    client.StartClient();
        //    return 0;
        //}

        #region IDisposable 成员  

        public void Dispose()
        {
            isListening = false;
            if (sender != null)
            {
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();
            }
        }

        #endregion
    }
}
