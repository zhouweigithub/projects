using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketBase
{

    public class Base : IClient, IServer
    {
        protected readonly int _port;  //连接服务器的端口
        protected readonly string _remoteIP;   //连接服务器的远程服务器IP
        protected Socket _socket;
        protected readonly Encoding encoding = Encoding.UTF8;
        /// <summary>
        /// 分段循环发送文件时，每次发送和读取的长度
        /// </summary>
        protected readonly int onceSendLength = 1024 * 1000;


        public Base(string remoteIP, int port)
        {
            _port = port;
            _remoteIP = remoteIP;
        }

        /// <summary>
        /// 获取文件以外的类型的发送字节
        /// </summary>
        /// <param name="type">内容类型</param>
        /// <param name="bytes">发送的内容</param>
        /// <returns></returns>
        protected byte[] GetSendBytes(ContextType type, byte[] bytes)
        {
            byte[] typeAndLengthBytes = GetTypeAndLengthBytes(type, bytes.Length);
            return typeAndLengthBytes.Concat(bytes).ToArray();
        }

        /// <summary>
        /// 获取文件的属性字节
        /// </summary>
        /// <param name="suffix"></param>
        /// <param name="bytes"></param>
        /// <returns></returns>
        protected byte[] GetFileSendPropertyBytes(string suffix, long totalLength, FileSendMode mode)
        {
            suffix = suffix.PadRight(10, ' ');  //后缀用空格补齐10个字符
            byte[] typeAndLengthBytes = GetTypeAndLengthBytes(ContextType.File, totalLength);
            byte[] sufixBytes = encoding.GetBytes(suffix);
            byte[] modeByte = encoding.GetBytes(((int)mode).ToString()); //发送模式占1个字节
            return typeAndLengthBytes.Concat(sufixBytes).Concat(modeByte).ToArray();
        }

        /// <summary>
        /// 获取类型和长度的字节码
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="length">长度</param>
        /// <returns></returns>
        private byte[] GetTypeAndLengthBytes(ContextType type, long length)
        {
            byte[] typeBytes = encoding.GetBytes(((int)type).ToString());  //类型占据1个字节
            byte[] lengthBytes = encoding.GetBytes(length.ToString("D10"));  //内容总长度占据10个字节
            return typeBytes.Concat(lengthBytes).ToArray();
        }


        /// <summary>
        /// 获取传输过来的数据的类型及总长度
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        protected (ContextType type, long length) GetReceivedContentProperty(Socket socket)
        {
            try
            {
                //取类型标识
                byte[] bytes1 = new byte[1];
                socket.Receive(bytes1, bytes1.Length, SocketFlags.None);
                string tmpType = encoding.GetString(bytes1);
                ContextType type = (ContextType)int.Parse(tmpType);

                //取内容总长度
                byte[] bytes2 = new byte[10];
                socket.Receive(bytes2, 10, SocketFlags.None);
                string tmpLength = encoding.GetString(bytes2);
                long length = long.Parse(tmpLength);

                return (type, length);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write("GetReceivedContentProperty 获取接收内容属性出错：" + e.Message, Util.Log.LogType.Error);
                throw new Exception(e.Message);
            }
            //return (ContextType.Other, 0);
        }

        protected byte[] GetValidBytes(byte[] bytes)
        {
            List<byte> result = new List<byte>();
            for (int i = 0; i < bytes.Length; i++)
            {
                if (bytes[i] != 0)
                {
                    result.Add(bytes[i]);
                }
            }
            return result.ToArray<byte>();
        }

        /// <summary>
        /// 客户端发送文件以外的内容
        /// </summary>
        /// <param name="contextType"></param>
        /// <param name="bytes"></param>
        public void ClientSend(ContextType contextType, byte[] bytes)
        {
            try
            {
                byte[] tmpBytes = GetSendBytes(contextType, bytes);
                _socket.Send(tmpBytes);
            }
            catch (Exception e)
            {
                PrintMsg("发送数据出错：" + e.Message);
                Util.Log.LogUtil.Write("发送数据出错：" + e.Message, Util.Log.LogType.Error);
            }
        }

        /// <summary>
        /// 一次发送整个文件的全部内容
        /// </summary>
        /// <param name="filePath"></param>
        public void ClientSendFile(string filePath)
        {
            try
            {
                string suffix = Path.GetExtension(filePath);
                FileStream fs = new FileStream(filePath, FileMode.Open);
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                fs.Close();
                byte[] tmpBytes = GetFileSendPropertyBytes(suffix, bytes.Length, FileSendMode.Once);
                byte[] sendBytes = tmpBytes.Concat(bytes).ToArray();
                _socket.Send(sendBytes);
            }
            catch (Exception e)
            {
                PrintMsg("ClientSendFile 发送数据出错：" + e.Message);
                Util.Log.LogUtil.Write("ClientSendFile 发送数据出错：" + e.Message, Util.Log.LogType.Error);
            }
        }

        /// <summary>
        /// 将一个文件分段循环发送
        /// </summary>
        /// <param name="filePath"></param>
        public void ClientSendFileLoop(string filePath)
        {
            try
            {
                PrintMsg("sending file " + filePath);
                using (FileStream fs = File.OpenRead(filePath))
                {
                    //先发送文件相关属性
                    string suffix = Path.GetExtension(filePath);
                    byte[] propertyBytes = GetFileSendPropertyBytes(suffix, fs.Length, FileSendMode.Loop);
                    _socket.Send(propertyBytes);

                    //循环发送文件内容
                    long totalSendLength = 0;
                    int readLengthOnce = onceSendLength;
                    while (totalSendLength < fs.Length)
                    {
                        if (totalSendLength + readLengthOnce > fs.Length)
                            readLengthOnce = (int)(fs.Length - totalSendLength);

                        byte[] bytes = new byte[readLengthOnce];
                        fs.Read(bytes, 0, readLengthOnce);
                        _socket.Send(bytes);

                        totalSendLength += readLengthOnce;
                    }
                }
                PrintMsg("send over");
            }
            catch (Exception e)
            {
                PrintMsg("ClientSendFileLoop 发送数据出错：" + e.Message);
                Util.Log.LogUtil.Write("ClientSendFileLoop 发送数据出错：" + e.Message, Util.Log.LogType.Error);
            }

        }

        protected void ServerSend(Socket socket, ContextType contextType, byte[] bytes)
        {
            try
            {
                byte[] tmpBytes = GetSendBytes(ContextType.Txt, bytes);
                socket.Send(tmpBytes);
            }
            catch (Exception e)
            {
                PrintMsg("ServerSend 发送数据出错 : " + e.Message);
                Util.Log.LogUtil.Write("ServerSend 发送数据出错：" + e.Message, Util.Log.LogType.Error);
            }
        }

        protected void PrintMsg(string msg)
        {
            Console.WriteLine(string.Format("#{0} {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), msg));
        }
    }


    /// <summary>
    /// 传输内容的类型
    /// </summary>
    public enum ContextType
    {
        /// <summary>
        /// 文本
        /// </summary>
        Txt,
        /// <summary>
        /// 文件
        /// </summary>
        File,
        /// <summary>
        /// 关闭连接
        /// </summary>
        Close,
        /// <summary>
        /// </summary>
        /// </summary>
        Other,
    }

    /// <summary>
    /// 传输文件的方式
    /// </summary>
    public enum FileSendMode
    {
        /// <summary>
        /// 一次性传输整个文件
        /// </summary>
        Once,
        /// <summary>
        /// 分段循环发送
        /// </summary>
        Loop,
    }
}
