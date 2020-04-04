// ****************************************
// FileName:ThreadUtil.cs
// Description:Thread助手类
// Tables:Nothing
// Author:Jordan Zuo
// Create Date:2015-08-24
// Revision History:
// ****************************************

using System;
using System.Threading;
using System.Collections.Concurrent;

namespace Util.OS
{
    using Util.Log;

    /// <summary>
    /// Thread助手类
    /// </summary>
    public class ThreadUtil
    {
        #region 字段

        // 所有已经初始化的线程列表
        private ConcurrentQueue<Thread> mThreadQueue = new ConcurrentQueue<Thread>();

        #endregion

        #region 属性

        /// <summary>
        /// 已经初始化的线程数量
        /// </summary>
        public Int32 InitedThreadCount
        {
            get
            {
                return this.mThreadQueue.Count;
            }
        }

        /// <summary>
        /// 最小的线程数量
        /// </summary>
        public Int32 MinThreadCount { get; private set; }

        /// <summary>
        /// 最大的线程数量
        /// </summary>
        public Int32 MaxThreadCount { get; private set; }

        /// <summary>
        /// 每个线程平均需要处理的数量
        /// </summary>
        public Int32 MessageCountPerThread { get; private set; }

        /// <summary>
        /// 供初始化线程的方法
        /// </summary>
        public ThreadStart ThreadStartFunc { get; private set; }

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="minThreadCount">最小的线程数量[10,100]</param>
        /// <param name="maxThreadCount">最大的线程数量[minThreadCount, 500]</param>
        /// <param name="messageCountPerThread">每个线程平均需要处理的数量[10,100]</param>
        /// <param name="threadStartFunc">FuncForThread</param>
        public ThreadUtil(Int32 minThreadCount, Int32 maxThreadCount, Int32 messageCountPerThread, ThreadStart threadStartFunc)
        {
            //检查最小的线程数量
            if (minThreadCount < 1)
            {
                throw new Exception("minThreadCount不能小于1");
            }
            if (minThreadCount > 100)
            {
                throw new Exception("minThreadCount不能大于100");
            }

            //检查最大的线程数量
            if (maxThreadCount < minThreadCount)
            {
                throw new Exception(String.Format("maxThreadCount:{0}比minThreadCount：{1}小", maxThreadCount, minThreadCount));
            }
            if (maxThreadCount > 500)
            {
                throw new Exception("maxThreadCount不能大于500");
            }

            //检查每个线程平均需要处理的数量
            if (messageCountPerThread < 10)
            {
                throw new Exception("messageCountPerThread不能小于10");
            }
            if (messageCountPerThread > 100)
            {
                throw new Exception("messageCountPerThread不能大于100");
            }

            //检查FuncForThread
            if (threadStartFunc == null)
            {
                throw new Exception("threadStartFunc不能为空");
            }

            //赋值
            this.MinThreadCount = minThreadCount;
            this.MaxThreadCount = maxThreadCount;
            this.MessageCountPerThread = messageCountPerThread;
            this.ThreadStartFunc = threadStartFunc;

            //启动最小数量的线程
            InitNewThread(this.MinThreadCount);
        }

        #endregion

        #region 方法

        /// <summary>
        /// 初始化新线程
        /// </summary>
        /// <param name="count">线程数量，大于0表示增加，小于0表示删除</param>
        private void InitNewThread(Int32 count)
        {
            if (count > 0)
            {
                for (Int32 i = 0; i < count; i++)
                {
                    //不能大于最大线程数量
                    if (this.mThreadQueue.Count >= this.MaxThreadCount) return;

                    //创建新线程
                    Thread t = new Thread(this.ThreadStartFunc) { IsBackground = true };
                    t.Start();

                    //将线程添加到列表中
                    this.mThreadQueue.Enqueue(t);
                }
            }
            else
            {
                count = count * -1;
                for (Int32 i = 0; i < count; i++)
                {
                    //不能小于最小线程数量
                    if (this.mThreadQueue.Count <= this.MinThreadCount) return;

                    //取出线程并终止
                    Thread t = null;
                    if (this.mThreadQueue.TryDequeue(out t))
                    {
                        //终止线程会触发异常，所以用try...catch
                        try
                        {
                            t.Abort();
                        }
                        catch { }
                    }
                }
            }
        }

        /// <summary>
        /// 动态调整线程数量
        /// </summary>
        /// <param name="queueCount">队列中数据的数量</param>
        public void DynamicAdjustThread(Int32 queueCount)
        {
            try
            {
                // 动态调整包括三个方面：保持不变、增加和减少
                // 保持不变：队列中需要的线程数量=已经初始化的线程数量
                // 增加：队列中需要的线程数量>已经初始化的线程数量
                // 减少：队列中需要的线程数量<已经初始化的线程数量
                Int32 diffThreadCount = (queueCount / this.MessageCountPerThread) - this.mThreadQueue.Count;
                if (diffThreadCount == 0)
                {
                    return;
                }
                else
                {
                    InitNewThread(diffThreadCount);
                }
            }
            catch (ThreadAbortException)
            {
                //专门用于处理此类异常，此类异常不记录日志，以免日志数据过大，仅作一个标识
            }
            catch (Exception ex)
            {
                LogUtil.Write(ex.StackTrace == null ? ex.Message : ex.StackTrace + Environment.NewLine + ex.Message, LogType.Error);
            }
        }

        #endregion
    }
}
