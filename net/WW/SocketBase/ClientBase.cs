using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketBase
{
    public class ClientBase : Base
    {
        private Action<string> _receivedAction; //接收到数据后的处理逻辑

        public ClientBase(string remoteIP, int port, Action<string> receivedAction) : base(remoteIP, port)
        {
            _receivedAction = receivedAction;
            Util.Log.LogUtil.SetLogPath(AppDomain.CurrentDomain.BaseDirectory + "Log");
            CreateClientSocket();
        }

        protected void CreateClientSocket()
        {
            IPAddress[] ipHostInfo = Dns.GetHostAddresses(_remoteIP);
            IPAddress ipAddress = null;
            foreach (IPAddress item in ipHostInfo)
            {
                if (item.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipAddress = item;
                    break;
                }
            }
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, _port);
            PrintMsg(localEndPoint.ToString());
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                _socket.Connect(localEndPoint);
                PrintMsg("已连接到主机" + _socket.RemoteEndPoint.ToString());
                new Thread(() => Listening()).Start();
            }
            catch (Exception e)
            {
                PrintMsg("连接远程服务器出错 : " + e.Message);
                Util.Log.LogUtil.Write("连接远程服务器出错：" + e.Message, Util.Log.LogType.Error);
            }
        }

        private void Listening()
        {
            string endPoint = _socket.RemoteEndPoint.ToString();
            try
            {
                while (true)
                {
                    (ContextType type, long length) = GetReceivedContentProperty(_socket);
                    GetContent(_socket, type, length);
                }
            }
            catch
            {
                PrintMsg("连接已断开 " + endPoint);
                _socket.Close();
            }
        }

        /// <summary>
        /// 获取传输具体内容
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="type"></param>
        /// <param name="length">内容总长度</param>
        private void GetContent(Socket socket, ContextType type, long length)
        {
            //获取传输具体内容
            byte[] bytes = new byte[length];
            socket.Receive(bytes);
            byte[] validBytes = GetValidBytes(bytes);

            switch (type)
            {
                case ContextType.Txt:
                    string text = encoding.GetString(validBytes);
                    _receivedAction("接收到服务端消息：" + text);
                    break;
                case ContextType.File:
                    break;
                case ContextType.Close:
                    PrintMsg("连接已断开 " + _socket.RemoteEndPoint.ToString());
                    _socket.Close();
                    break;
                case ContextType.Other:
                    break;
                default:
                    break;
            }
        }

    }
}
