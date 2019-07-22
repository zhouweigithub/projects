using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace WW
{
    public class SocketBLL
    {
        private List<string> onLines;

        public static void Listen(int port)
        {
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 9729);
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);

                while (true)
                {
                    Console.WriteLine("等待客户端连接...");
                    //线程将一直阻塞直到有新的客户端连接  
                    Socket handler = listener.Accept();
                    //启用一个新的线程用于处理客户端连接  
                    //这样主线程还可以继续接受客户端连接  
                    ServerThread socketThread = new ServerThread(handler);
                }
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write("监听出错：" + e.Message, Util.Log.LogType.Error);
            }
        }
    }
}
