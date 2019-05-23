// ****************************************
// FileName:MonitorUtil.cs
// Description: 排他锁工具类
// Tables:Nothing
// Author:刘军
// Create Date:2016-05-23
// Revision History:
// ****************************************

using System;
using System.Threading;
using System.Collections.Generic;

namespace Util.Lock
{
    /// <summary>
    /// 排他锁工具类
    /// </summary>
    public class MonitorUtil
    {
        /// <summary>
        /// 自定义锁对象
        /// </summary>
        private class CustomMonitor : IDisposable
        {
            /// <summary>
            /// 锁对象
            /// </summary>
            private Object lockObj = null;

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="lockObj">锁对象</param>
            public CustomMonitor(Object lockObj)
            {
                this.lockObj = lockObj;
            }

            /// <summary>
            /// 锁释放
            /// </summary>
            public void Dispose()
            {
                try
                {
                    if (Monitor.IsEntered(this.lockObj))
                    {
                        Monitor.Exit(this.lockObj);
                    }
                }
                catch
                {
                    // 当前线程如果没有获取到锁时，则会抛出异常。此处直接吞掉
                }
            }
        }

        /// <summary>
        /// 锁信息对象
        /// </summary>
        private class LockInfo
        {
            /// <summary>
            /// 自定义锁对象
            /// </summary>
            public CustomMonitor CustomMonitor { get; private set; }

            /// <summary>
            /// 锁实例
            /// </summary>
            public Object LockObj { get; private set; }

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="customMonitor">自定义锁对象</param>
            /// <param name="lockObj">锁实例</param>            
            public LockInfo(CustomMonitor customMonitor, Object lockObj)
            {
                this.CustomMonitor = customMonitor;
                this.LockObj = lockObj;
            }
        }

        /// <summary>
        /// 同步对象
        /// </summary>
        private Object lockInfoDicLockObj = new Object();

        /// <summary>
        /// 锁信息集合
        /// </summary>
        private Dictionary<String, LockInfo> mLockInfoDic = new Dictionary<String, LockInfo>();

        /// <summary>
        /// 获取锁对象信息
        /// </summary>
        /// <param name="key">锁的唯一标识</param>
        /// <returns>返回锁对象</returns>
        private LockInfo GetLockInfo(String key)
        {
            lock (lockInfoDicLockObj)
            {
                if (!mLockInfoDic.ContainsKey(key))
                {
                    // 如果获取不到锁，则创建一个锁对象
                    Object lockObj = new Object();
                    LockInfo lockInfoObj = new LockInfo(new CustomMonitor(lockObj), lockObj);

                    // 添加到Dictionary中
                    mLockInfoDic[key] = lockInfoObj;

                    return lockInfoObj;
                }

                return mLockInfoDic[key];
            }
        }

        /// <summary>
        /// 获取锁对象
        /// </summary>
        /// <param name="lockName">锁的唯一标识</param>
        /// <param name="millisecondsTimeout">等待的毫秒数；<=0表示无限期等待。</param>
        /// <returns>返回锁对象</returns>
        /// <exception cref="TimeoutException">获取锁对象超时时，抛出此异常</exception>
        public IDisposable GetLock(String key, Int32 millisecondsTimeout = 100)
        {
            // 获取锁信息对象
            LockInfo lockInfoObj = GetLockInfo(key);

            // 根据等待时间来选择不同的处理方式
            if (millisecondsTimeout <= 0)
            {
                Monitor.Enter(lockInfoObj.LockObj);
            }
            else
            {
                if (Monitor.TryEnter(lockInfoObj.LockObj, millisecondsTimeout) == false)
                {
                    throw new TimeoutException("等待锁超时");
                }
            }

            return lockInfoObj.CustomMonitor;
        }

        /// <summary>
        /// 获取已存在的锁的操作类
        /// </summary>
        /// <param name="lockObj">同步对象</param>
        /// <returns>返回锁的操作类</returns>
        /// <exception cref="TimeoutException">获取锁对象超时时，抛出此异常</exception>
        /// <example>
        /// Object lockObj = new Object();
        /// using(MonitorUtil.GetLock(lockObj))
        /// {
        ///     // do something
        /// }
        /// </example>
        public static IDisposable GetLock(Object lockObj, Int32 millisecondsTimeout = 100)
        {
            // 根据等待时间来选择不同的处理方式
            if (millisecondsTimeout <= 0)
            {
                Monitor.Enter(lockObj);
            }
            else
            {
                if (Monitor.TryEnter(lockObj, millisecondsTimeout) == false)
                {
                    throw new TimeoutException("等待锁超时");
                }
            }

            return new CustomMonitor(lockObj);
        }

        /// <summary>
        /// 主动释放锁资源，避免长久驻留内存
        /// </summary>
        /// <param name="key">锁的唯一标识</param>
        public void ReleaseLock(String key)
        {
            lock (lockInfoDicLockObj)
            {
                mLockInfoDic.Remove(key);
            }
        }

        /// <summary>
        /// 主动清空所有锁资源，避免长久驻留内存
        /// </summary>
        public void ReleaseAllLock()
        {
            lock (lockInfoDicLockObj)
            {
                mLockInfoDic.Clear();
            }
        }
    }
}