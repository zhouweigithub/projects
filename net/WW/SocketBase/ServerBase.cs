using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace SocketBase
{
    public class ServerBase : Base
    {
        private Dictionary<string, Socket> clients = new Dictionary<string, Socket>();

        public ServerBase(string remoteIP, int port) : base(remoteIP, port)
        {
            Util.Log.LogUtil.SetLogPath(AppDomain.CurrentDomain.BaseDirectory + "Log");
            CreateListenerSocket();
        }

        protected void CreateListenerSocket()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(string.IsNullOrEmpty(_remoteIP) ? Dns.GetHostName() : _remoteIP);
            IPAddress ipAddress = null;
            foreach (IPAddress item in ipHostInfo.AddressList)
            {
                if (item.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipAddress = item;
                    break;
                }
            }
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, _port);
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.Bind(localEndPoint);
            _socket.Listen(10);
            PrintMsg(string.Format("服务器启动成功，ip {0} port {1}", ipAddress.ToString(), _port));
            PrintMsg("等待客户端连接...");
            try
            {
                while (true)
                {
                    //线程将一直阻塞直到有新的客户端连接  
                    Socket handler = _socket.Accept();
                    PrintMsg(string.Format("收到客户端连接，{0}", handler.RemoteEndPoint.ToString()));
                    var remoteIpString = handler.RemoteEndPoint.ToString();
                    clients[remoteIpString] = handler;
                    new Thread(() => Listening(handler)).Start();

                    for (int i = 100; i < 110; i++)
                    {
                        ServerSend(handler, ContextType.Txt, encoding.GetBytes(i.ToString()));
                    }
                }
            }
            catch (Exception e)
            {
                PrintMsg("监听出错 : " + e.Message);
                Util.Log.LogUtil.Write("监听出错：" + e.Message, Util.Log.LogType.Error);
            }
        }

        private void Listening(Socket socket)
        {
            bool isTransferFileLoop = false;    //是否处于接收客户端循环发送过来的文件内容中
            string fileName = string.Empty;
            string endPoint = socket.RemoteEndPoint.ToString();
            long length = 0;
            try
            {
                while (true)
                {
                    if (!isTransferFileLoop)
                    {
                        //没有在循环传输文件时，需要读取每次传输的文件的属性
                        ContextType type;
                        (type, length) = GetReceivedContentProperty(socket);
                        GetContent(socket, type, length, out isTransferFileLoop, out fileName);
                    }
                    else
                    {
                        //接收客户端循环发送过来的文件内容
                        SaveFileLoop(socket, length, fileName, ref isTransferFileLoop);
                    }

                }
            }
            catch
            {
                PrintMsg("连接已断开 " + endPoint);
                clients.Remove(endPoint);
                socket.Close();
            }
        }

        /// <summary>
        /// 获取传输具体内容
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="type"></param>
        /// <param name="length"></param>
        /// <param name="isTransferFileLoop"></param>
        private void GetContent(Socket socket, ContextType type, long length, out bool isTransferFileLoop, out string fileName)
        {
            //获取传输具体内容
            fileName = string.Empty;
            isTransferFileLoop = false;
            switch (type)
            {
                case ContextType.Txt:
                    byte[] bytes = new byte[length];
                    socket.Receive(bytes);
                    byte[] validBytes = GetValidBytes(bytes);
                    string text = encoding.GetString(validBytes);
                    PrintMsg("接收到客户端消息：" + text);
                    ServerSend(socket, ContextType.Txt, encoding.GetBytes("接收到客户端消息：" + text));
                    break;
                case ContextType.File:
                    //读取文件后缀
                    byte[] bytesSuffix = new byte[10];
                    socket.Receive(bytesSuffix, bytesSuffix.Length, SocketFlags.None);
                    string suffix = encoding.GetString(bytesSuffix).Trim();

                    //读取文件传输方式（整体传输/分段循环传输）
                    byte[] bytesMode = new byte[1];
                    socket.Receive(bytesMode, bytesMode.Length, SocketFlags.None);
                    FileSendMode mode = (FileSendMode)int.Parse(encoding.GetString(bytesMode));

                    string folder = AppDomain.CurrentDomain.BaseDirectory + "upload\\";
                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);

                    fileName = folder + DateTime.Now.ToString("yyyyMMddHHmmssffff") + suffix;

                    if (mode == FileSendMode.Once)
                    {
                        SaveFileOnce(socket, length, fileName);
                        PrintMsg("成功接收文件" + fileName);
                    }
                    else
                    {
                        isTransferFileLoop = true;
                        PrintMsg("start to receive file " + fileName);
                    }
                    break;
                case ContextType.Close:
                    clients.Remove(socket.RemoteEndPoint.ToString());
                    PrintMsg("连接已断开 " + socket.RemoteEndPoint.ToString());
                    socket.Close();
                    break;
                case ContextType.Other:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 处理客户端一次性发送整个文件
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="totalLength"></param>
        private void SaveFileOnce(Socket socket, long totalLength, string fileName)
        {
            if (totalLength == 0 || string.IsNullOrWhiteSpace(fileName))
            {
                PrintMsg(string.Format("收到的文件属性异常：length: {0} filename: {1}", totalLength, fileName));
                return;
            }

            using (FileStream fs = File.OpenWrite(fileName))
            {
                long readedLength = 0;   //读取的总量
                int readLengthOnce = onceSendLength;
                while (readedLength < totalLength)
                {
                    if (readedLength + readLengthOnce > totalLength)
                        readLengthOnce = (int)(totalLength - readedLength);

                    byte[] bytesOnce = new byte[readLengthOnce];
                    socket.Receive(bytesOnce, readLengthOnce, SocketFlags.None);
                    PrintMsg("接收到一次性完整文件");
                    fs.Write(bytesOnce, 0, readLengthOnce);
                    fs.Flush();
                    readedLength += bytesOnce.Length;
                }
            }
            ServerSend(socket, ContextType.Txt, encoding.GetBytes("已成功接收文件"));
        }

        private void SaveFileLoop(Socket socket, long fileLength, string fileName, ref bool isTransferFileLoop)
        {
            if (fileLength == 0 || string.IsNullOrWhiteSpace(fileName))
            {
                isTransferFileLoop = false;
                PrintMsg(string.Format("收到的文件属性异常：length: {0} filename: {1}", fileLength, fileName));
                return;
            }

            try
            {
                using (FileStream fs = File.OpenWrite(fileName))
                {
                    bool isEnd = fs.Length + onceSendLength >= fileLength;
                    int receiveLength = (int)(isEnd ? fileLength - fs.Length : onceSendLength);
                    byte[] bytesTotal = new byte[receiveLength];
                    socket.Receive(bytesTotal, receiveLength, SocketFlags.None);

                    fs.Position = fs.Length;
                    fs.Write(bytesTotal, 0, receiveLength);

                    if (isEnd) //文件结束标志
                    {
                        fs.Flush();
                        isTransferFileLoop = false;
                        PrintMsg("receive file over");
                        PrintMsg(string.Format("file length {0}", fileLength));
                        PrintMsg(string.Format("total received bytes {0}", fs.Length));
                        ServerSend(socket, ContextType.Txt, encoding.GetBytes("已成功接收文件"));
                    }
                }
            }
            catch (Exception e)
            {
                PrintMsg("分段接收文件出错：" + e.Message);
                Util.Log.LogUtil.Write("分段接收文件出错：" + e.Message, Util.Log.LogType.Error);
            }
        }
    }

}
