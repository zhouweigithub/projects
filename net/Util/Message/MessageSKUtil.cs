//***********************************************************************************
//文件名称：MessageSKUtil.cs
//功能描述：处理消息发送的助手类(基于sokect)
//数据表：Nothing
//作者：byron
//日期：2015/11/25 10:39:03
//修改记录：
//  Mendor:Jordan Zuo
//  Mend Date:2016-5-19
//  Mend Content:使用Monitor的Wait、Pulse方法来协调消费者和生产者之间的互动
//***********************************************************************************

using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Collections.Generic;

namespace Util.Message
{
    using Util.OS;
    using Util.Log;
    using Util.Json;
    using Util.Zlib;

    /// <summary>
    /// 处理消息发送的助手类(基于sokect)
    /// </summary>
    public class MessageSKUtil
    {
        #region 全局变量、字段和属性

        //内存中能够保存的最大数量
        private const Int32 MAX_COUNT_IN_MEMORY = 10000;

        //存储消息的队列，以及对应的锁对象
        private Object mLockObj = new Object();
        private Queue<SendMessageObject> mMessageQueue = new Queue<SendMessageObject>();

        //Socket列表，以及对应的锁对象
        private Object mLockForSocket = new Object();
        private List<Socket> mSocketList = new List<Socket>();

        /// <summary>
        /// 保存失败消息的文件夹
        /// </summary>
        public String MessageFolder { get; private set; }

        /// <summary>
        /// 线程助手对象
        /// </summary>
        public ThreadUtil ThreadUtilObj { get; private set; }

        /// <summary>
        /// socket配置
        /// </summary>
        public SKConfig SKConfig { get; private set; }

        #endregion

        #region 构造函数

        /// <summary>
        /// 静态构造函数，用于初始化数据
        /// </summary>
        public MessageSKUtil(String messageFolder, Int32 minThreadCount, Int32 maxThreadCount, Int32 messageCountPerThread, SKConfig cf)
        {
            //初始化
            this.MessageFolder = messageFolder;
            this.SKConfig = cf;
            this.ThreadUtilObj = new ThreadUtil(minThreadCount, maxThreadCount, messageCountPerThread, ConsumeMessage);

            //记录日志
            LogUtil.Write(String.Format("MessageFolder={0},MinThreadCount={1},MaxThreadCount={2}", messageFolder, minThreadCount, maxThreadCount), LogType.Debug);

            //启动定时线程
            Thread t = new Thread(TimingMethod) { IsBackground = true };
            t.Start();
        }

        #endregion

        #region 内部调用方法

        /// <summary>
        /// 处理消息
        /// </summary>
        /// <param name="socket">socket对象</param>
        /// <param name="message">消息</param>
        private void HandleMessage(Socket socket, SendMessageObject message)
        {
            if (socket == null || socket.Connected == false)
            {
                LogUtil.Write("Socket连接未建立或连接已经断开", LogType.Error);
                return;
            }

            try
            {
                //将数据进行zlib压缩
                Byte[] contentData = ZlibUtil.Compress(Encoding.UTF8.GetBytes(message.Message));
                Byte[] headerData = BitConverter.GetBytes(contentData.Length);
                Byte[] buffer = new Byte[contentData.Length + 4];
                headerData.CopyTo(buffer, 0);
                contentData.CopyTo(buffer, 4);

                socket.Send(buffer);
            }
            catch (SocketException ex)
            {
                //记录日志
                String exception = (ex.StackTrace == null ? ex.Message : ex.StackTrace + Environment.NewLine + ex.Message);
                String log = String.Format("Exception:{0};Message:{1}", exception, JsonUtil.Serialize(message));
                LogUtil.Write(log, LogType.Error);

                SaveMessage(message);

                //如果是Socket引发的异常，直接抛出
                throw ex;
            }
        }

        /// <summary>
        /// 心跳
        /// </summary>
        /// <param name="socket">socket对象</param>
        private void HeartBeat(Socket socket)
        {
            if (socket == null || socket.Connected == false)
            {
                LogUtil.Write("Socket连接未建立或连接已经断开", LogType.Debug);
                return;
            }

            try
            {
                socket.Send(BitConverter.GetBytes(0));
                LogUtil.Write("发送心跳包成功", LogType.Debug);
            }
            catch (SocketException ex)
            {
                //记录日志
                String log = String.Format("发送心跳包异常，Message:{0}，Stack:{1}", ex.Message, ex.StackTrace);
                LogUtil.Write(log, LogType.Error);

                //如果是Socket引发的异常，直接抛出
                throw ex;
            }
        }

        /// <summary>
        /// Saves the message.
        /// </summary>
        /// <param name="message">The message.</param>
        private void SaveMessage(SendMessageObject message)
        {
            FileUtil.WriteFile(this.MessageFolder, Guid.NewGuid().ToString(), false, JsonUtil.Serialize(message));
        }

        /// <summary>
        /// 定时执行的方法
        /// </summary>
        private void TimingMethod()
        {
            //启动时先休眠1分钟，以免处理大量的数据，从而使得启动时CPU或IO占用太大
            Thread.Sleep(1000 * 60);

            while (true)
            {
                try
                {
                    //给所有的Socket连接发送心跳包
                    lock (mLockForSocket)
                    {
                        LogUtil.Write(String.Format("当前Socket的数量为：{0}", mSocketList.Count), LogType.Debug);
                        foreach (Socket socket in mSocketList)
                        {
                            HeartBeat(socket);
                        }
                    }

                    //获取当前队列数据的数量
                    Int32 count = 0;
                    lock (mLockObj)
                    {
                        count = this.mMessageQueue.Count;
                    }

                    //动态调整线程数量
                    this.ThreadUtilObj.DynamicAdjustThread(count);

                    //记录日志
                    Int32 initedThreadCount = this.ThreadUtilObj.InitedThreadCount;
                    LogUtil.Write(String.Format("已初始化的线程数量为：{0}，队列中未处理的数量为：{1}，平均每个线程需处理数量为：{2}", initedThreadCount, count, initedThreadCount == 0 ? 0 : count / initedThreadCount), LogType.Debug);

                    //如果内存超过5000条数据，则保存到文件夹中，以免导致内存中数据量太大
                    if (count > MAX_COUNT_IN_MEMORY)
                    {
                        for (Int32 i = 0; i < count; i++)
                        {
                            SendMessageObject message = null;
                            lock (mLockObj)
                            {
                                if (this.mMessageQueue.Count > 0)
                                {
                                    message = this.mMessageQueue.Dequeue();
                                }
                            }
                            if (message != null)
                            {
                                SaveMessage(message);
                            }
                        }
                    }
                    else
                    {
                        //获取文件名称列表，如果不为空，则遍历文件，并读取其中的内容
                        String[] fileNameList = FileUtil.GetFileNameList(this.MessageFolder);
                        if (fileNameList == null || fileNameList.Length == 0) continue;

                        //将缓存中的数据增加到MAX_COUNT_IN_MEMORY
                        for (Int32 i = 0; i < fileNameList.Length && i < (MAX_COUNT_IN_MEMORY - count); i++)
                        {
                            String fileName = fileNameList[i];

                            //读取文件内容
                            String strMessage = FileUtil.ReadFile(fileName);

                            //如果文件不为空，则加入到队列中
                            if (!String.IsNullOrEmpty(strMessage))
                            {
                                SendMessageObject message = JsonUtil.Deserialize<SendMessageObject>(strMessage);
                                lock (mLockObj)
                                {
                                    this.mMessageQueue.Enqueue(message);
                                }
                            }

                            //最后删除文件
                            FileUtil.DeleteFile(fileName);
                        }
                    }
                }
                catch (ThreadAbortException)
                {
                    //专门用于处理此类异常，此类异常不记录日志，以免日志数据过大，仅作一个标识
                    //但需要break，否则当线程被终止时，会导致死循环
                    //整个过程如下：
                    //1、如果是修改web.config或替换dll文件，会在当前进程内启动新的应用程序域（不会启动新的线程，只有回收时才会）
                    //2、同时销毁老的应用程序域（在销毁时，却无法退出本方法的while(true)）
                    //3、从而导致死循环
                    break;
                }
                catch (Exception ex)
                {
                    LogUtil.Write(ex.StackTrace == null ? ex.Message : ex.StackTrace + Environment.NewLine + ex.Message, LogType.Error);
                }
                finally
                {
                    //休眠1分钟
                    Thread.Sleep(1000 * 60);
                }
            }
        }

        /// <summary>
        /// 消耗消息
        /// </summary>
        private void ConsumeMessage()
        {
            Socket socket = null;
            try
            {
                IPAddress ip = IPAddress.Parse(this.SKConfig.Ip);
                IPEndPoint ipe = new IPEndPoint(ip, this.SKConfig.Port);
                socket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(ipe);

                //将Socket添加到列表中
                lock (mLockForSocket)
                {
                    mSocketList.Add(socket);
                }

                while (true)
                {
                    try
                    {
                        //如果没有数据，则等待
                        lock (mLockObj)
                        {
                            if (this.mMessageQueue.Count == 0)
                            {
                                Monitor.Wait(mLockObj);
                            }
                        }

                        SendMessageObject message = null;
                        lock (mLockObj)
                        {
                            if (this.mMessageQueue.Count > 0)
                            {
                                message = this.mMessageQueue.Dequeue();
                            }
                        }
                        if (message != null)
                        {
                            HandleMessage(socket, message);
                        }
                    }
                    catch (ThreadAbortException)
                    {
                        //专门用于处理此类异常，此类异常不记录日志，以免日志数据过大，仅作一个标识
                        //但需要break，否则当线程被终止时，会导致死循环
                        //整个过程如下：
                        //1、如果是修改web.config或替换dll文件，会在当前进程内启动新的应用程序域（不会启动新的线程，只有回收时才会）
                        //2、同时销毁老的应用程序域（在销毁时，却无法退出本方法的while(true)）
                        //3、从而导致死循环
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.Write(ex.StackTrace == null ? ex.Message : ex.StackTrace + Environment.NewLine + ex.Message, LogType.Error);
            }
            finally
            {
                if (socket != null)
                {
                    socket.Close();
                }
            }
        }

        #endregion

        #region 外部调用方法

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void SendMessage(SendMessageObject message)
        {
            //判断数据是否有效
            if (message == null) return;

            //将消息存储到队列中，并通知处于等待的线程
            lock (mLockObj)
            {
                this.mMessageQueue.Enqueue(message);
                Monitor.Pulse(mLockObj);
            }
        }

        /// <summary>
        /// 将队列中尚未发送的数据保存到文件中
        /// </summary>
        public void SaveMessage()
        {
            //记录日志
            LogUtil.Write(String.Format("Begin to save message, count={0}", this.mMessageQueue.Count), LogType.Debug);

            //保存数据
            while (true)
            {
                try
                {
                    //如果没有数据，则等待
                    lock (mLockObj)
                    {
                        if (this.mMessageQueue.Count == 0)
                        {
                            break;
                        }
                    }

                    SendMessageObject message = null;
                    lock (mLockObj)
                    {
                        if (this.mMessageQueue.Count > 0)
                        {
                            message = this.mMessageQueue.Dequeue();
                        }
                    }
                    if (message != null)
                    {
                        SaveMessage(message);
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.Write(ex.StackTrace == null ? ex.Message : ex.StackTrace + Environment.NewLine + ex.Message, LogType.Error);
                }
            }

            LogUtil.Write("Save message successfully.", LogType.Debug);
        }

        #endregion
    }

    /// <summary>
    /// sk配置
    /// </summary>
    public class SKConfig
    {
        /// <summary>
        /// ip
        /// </summary>
        public String Ip { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public Int32 Port { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ip">ip</param>
        /// <param name="port">端口</param>
        public SKConfig(String ip, Int32 port)
        {
            this.Ip = ip;
            this.Port = port;
        }
    }
}
