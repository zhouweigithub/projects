//***********************************************************************************
//文件名称：ThreadHelper.cs
//功能描述：多线程操作类
//数据表： 
//作者：周围
//日期：2017-09-18
//修改记录：
//***********************************************************************************
using Util.Log;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace Sunny.Common
{
    /// <summary>
    /// 启用多线程
    /// </summary>
    public class ThreadHelper
    {
        /// <summary>
        /// 用于多线程中写日志时的锁对象
        /// </summary>
        public static readonly object logLockObj = new object();

        /// <summary>
        /// 执行多线程任务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">循环的资源</param>
        /// <param name="maxThreadCount">最大线程数</param>
        /// <param name="waitSeconds">主线程单位等待时长（秒）</param>
        /// <param name="action">线程执行的具体任务</param>
        public static void Threading<T>(List<T> source, int maxThreadCount, int waitSeconds, Action<T> action)
        {
            if (source == null || source.Count == 0 || action == null)
                return;

            ConcurrentQueue<T> queue = new ConcurrentQueue<T>(source);  //多个线程分配的所有资源
            object lockobj = new object();     //资源锁
            int successCount = 0;   //已结束任务的线程数
            int threadCount = source.Count < maxThreadCount ? source.Count : maxThreadCount;    //需要分配的最大线程数量

            for (int i = 0; i < threadCount; i++)
            {   //循环创建线程
                System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    while (!queue.IsEmpty)      //若任务没完成则继续从任务队列中取出任务执行，直到所有任务都完成才退出线程
                    {
                        bool isGetDataSuccess = false;        //是否从队列中取到了资源
                        T item = default(T);
                        lock (lockobj)
                        {
                            isGetDataSuccess = queue.TryDequeue(out item);
                        }

                        if (isGetDataSuccess)
                        {
                            try
                            {   //执行具体任务
                                action(item);
                            }
                            catch (Exception ex)
                            {
                                lock (logLockObj)       //锁住日志文件
                                {
                                    LogUtil.Write(String.Format("线程操作出错：{0}", ex.ToString()), LogType.Error);
                                }
                            }
                            finally
                            {   //已结束任务的线程数加1
                                Interlocked.Add(ref successCount, 1);
                            }
                        }
                    }
                });
            }

            //直到所有任务处理完毕，主线程才能离开，否则一直休眠
            while (true)
            {
                if (successCount >= source.Count)
                    break;
                else
                    Thread.Sleep(1000 * waitSeconds);
            }
        }
    }
}
