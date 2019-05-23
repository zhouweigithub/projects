// ****************************************
// FileName:MessageUtil.cs
// Description:处理消息发送的助手类
// Tables:Nothing
// Author:Jordan Zuo
// Create Date:2014-07-14
// Revision History:
//  Mendor:Jordan Zuo
//  Mend Date:2016-5-19
//  Mend Content:使用Monitor的Wait、Pulse方法来协调消费者和生产者之间的互动
// ****************************************

using System;
using System.Threading;
using System.Collections.Generic;

namespace Util.Message
{
    using Util.OS;
    using Util.Log;
    using Util.Web;
    using Util.Json;

    /// <summary>
    /// 处理消息发送的助手类
    /// </summary>
    public static class MessageUtil
    {
        #region 全局变量、字段和属性

        //内存中能够保存的最大数量
        private const Int32 MAX_COUNT_IN_MEMORY = 5000;

        //存储消息的队列，以及对应的锁对象
        private static Object mLockObj = new Object();
        private static Queue<SendMessageObject> mMessageQueue = new Queue<SendMessageObject>();

        /// <summary>
        /// 保存失败消息的文件夹
        /// </summary>
        private static String mMessageFolder = String.Empty;

        /// <summary>
        /// 线程助手对象
        /// </summary>
        private static ThreadUtil mThreadUtilObj = null;

        #endregion

        #region 内部调用方法

        /// <summary>
        /// 处理消息
        /// </summary>
        /// <param name="message">消息</param>
        private static void HandleMessage(SendMessageObject message)
        {
            try
            {
                if (message.IsPost)
                {
                    WebUtil.PostWebData(message.Url, message.Message, DataCompress.NotCompress);
                }
                else
                {
                    WebUtil.GetWebData(message.Url, message.Message, DataCompress.NotCompress);
                }
            }
            catch (Exception ex)
            {
                //记录日志
                String exception = (ex.StackTrace == null ? ex.Message : ex.StackTrace + Environment.NewLine + ex.Message);
                String log = String.Format("Exception:{0};Message:{1}", exception, JsonUtil.Serialize(message));
                LogUtil.Write(log, LogType.Error);

                SaveMessage(message);
            }
        }

        /// <summary>
        /// Saves the message.
        /// </summary>
        /// <param name="message">The message.</param>
        private static void SaveMessage(SendMessageObject message)
        {
            FileUtil.WriteFile(mMessageFolder, Guid.NewGuid().ToString(), false, JsonUtil.Serialize(message));
        }

        /// <summary>
        /// 定时执行的方法
        /// </summary>
        private static void TimingMethod()
        {
            //启动时先休眠1分钟，以免处理大量的数据，从而使得启动时CPU或IO占用太大
            Thread.Sleep(1000 * 60);

            while (true)
            {
                try
                {
                    //获取当前队列数据的数量
                    Int32 count = 0;
                    lock (mLockObj)
                    {
                        count = mMessageQueue.Count;
                    }

                    //动态调整线程数量
                    mThreadUtilObj.DynamicAdjustThread(count);

                    //记录日志
                    Int32 initedThreadCount = mThreadUtilObj.InitedThreadCount;
                    LogUtil.Write(String.Format("已初始化的线程数量为{0}，队列中未处理的数量为{1}，平均每个线程需处理数量为{2}",
                    initedThreadCount, mMessageQueue.Count, initedThreadCount == 0 ? 0 : mMessageQueue.Count / initedThreadCount),
                    LogType.Debug);

                    //如果内存超过5000条数据，则保存到文件夹中，以免导致内存中数据量太大
                    if (count > MAX_COUNT_IN_MEMORY)
                    {
                        for (Int32 i = 0; i < count; i++)
                        {
                            SendMessageObject message = null;
                            lock (mLockObj)
                            {
                                if (mMessageQueue.Count > 0)
                                {
                                    message = mMessageQueue.Dequeue();
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
                        String[] fileNameList = FileUtil.GetFileNameList(mMessageFolder);
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
                                    mMessageQueue.Enqueue(message);
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
        private static void ConsumeMessage()
        {
            while (true)
            {
                try
                {
                    //如果没有数据，则等待
                    lock (mLockObj)
                    {
                        if (mMessageQueue.Count == 0)
                        {
                            Monitor.Wait(mLockObj);
                        }
                    }

                    SendMessageObject message = null;
                    lock (mLockObj)
                    {
                        if (mMessageQueue.Count > 0)
                        {
                            message = mMessageQueue.Dequeue();
                        }
                    }
                    if (message != null)
                    {
                        HandleMessage(message);
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
            }
        }

        #endregion

        #region 外部调用方法

        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="messageFolder">存储消息的目录</param>
        /// <param name="minThreadCount">最小的线程数量[10,100]</param>
        /// <param name="maxThreadCount">最大的线程数量[minThreadCount, 500]</param>
        /// <param name="messageCountPerThread">每个线程平均需要处理的数量</param>
        public static void SetParam(String messageFolder, Int32 minThreadCount, Int32 maxThreadCount, Int32 messageCountPerThread)
        {
            lock (mLockObj)
            {
                //判断是否已经初始化
                if (mThreadUtilObj != null)
                {
                    return;
                }

                //初始化
                mMessageFolder = messageFolder;
                mThreadUtilObj = new ThreadUtil(minThreadCount, maxThreadCount, messageCountPerThread, ConsumeMessage);
            }

            //记录日志
            LogUtil.Write(String.Format("MessageFolder={0},MinThreadCount={1},MaxThreadCount={2}", messageFolder, minThreadCount, maxThreadCount), LogType.Debug);

            //启动定时线程
            Thread t = new Thread(TimingMethod) { IsBackground = true };
            t.Start();
        }

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void SendMessage(SendMessageObject message)
        {
            //判断数据是否有效
            if (message == null) return;

            //将消息存储到队列中，并通知处于等待的线程
            lock (mLockObj)
            {
                mMessageQueue.Enqueue(message);
                Monitor.Pulse(mLockObj);
            }
        }

        /// <summary>
        /// 将队列中尚未发送的数据保存到文件中
        /// </summary>
        public static void SaveMessage()
        {
            //记录日志
            LogUtil.Write(String.Format("Begin to save message, count={0}", mMessageQueue.Count), LogType.Debug);

            //保存数据
            while(true)
            {
                try
                {
                    //如果没有数据，则等待
                    lock (mLockObj)
                    {
                        if (mMessageQueue.Count == 0)
                        {
                            break;
                        }
                    }

                    SendMessageObject message = null;
                    lock (mLockObj)
                    {
                        if (mMessageQueue.Count > 0)
                        {
                            message = mMessageQueue.Dequeue();
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
}