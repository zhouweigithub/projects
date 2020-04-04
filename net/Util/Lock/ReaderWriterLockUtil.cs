// ****************************************
// FileName:ReaderWriterLockUtil.cs
// Description: 读写锁工具类
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
    /// 读写锁工具类
    /// </summary>
    public class ReaderWriterLockUtil
    {
        /// <summary>
        /// 锁类型枚举
        /// </summary>
        public enum LockTypeEnum
        {
            /// <summary>
            /// 读,在此方式下，如果要切换到写。则会报异常
            /// </summary>
            Reader,

            /// <summary>
            /// 写
            /// </summary>
            Writer,

            /// <summary>
            /// 可升级的读，在读中可能需要切换到写锁，用此方式，此方式性能比Writer高
            /// </summary>
            EnterUpgradeableReader
        }

        /// <summary>
        /// 自定义锁对象
        /// </summary>
        private class CustomMonitor : IDisposable
        {
            /// <summary>
            /// 锁对象
            /// </summary>
            private ReaderWriterLockSlim rwLockObj = null;

            /// <summary>
            /// 锁类型
            /// </summary>
            private LockTypeEnum lockType;

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="rwLockObj">读写锁对象</param>
            /// <param name="lockType">获取方式</param>
            public CustomMonitor(ReaderWriterLockSlim rwLockObj, LockTypeEnum lockType)
            {
                this.rwLockObj = rwLockObj;
                this.lockType = lockType;
            }

            /// <summary>
            /// 锁释放
            /// </summary>
            public void Dispose()
            {
                if (lockType == LockTypeEnum.Reader && rwLockObj.IsReadLockHeld)
                {
                    rwLockObj.ExitReadLock();
                }
                else if (lockType == LockTypeEnum.Writer && rwLockObj.IsWriteLockHeld)
                {
                    rwLockObj.ExitWriteLock();
                }
                else if (lockType == LockTypeEnum.EnterUpgradeableReader && rwLockObj.IsUpgradeableReadLockHeld)
                {
                    rwLockObj.ExitUpgradeableReadLock();
                }
            }
        }

        /// <summary>
        /// lockObjData同步对象
        /// </summary>
        private Object mLockObj = new Object();

        /// <summary>
        /// 锁集合
        /// </summary>
        private Dictionary<String, ReaderWriterLockSlim> mLockSlimDic = new Dictionary<String, ReaderWriterLockSlim>();

        /// <summary>
        /// 获取锁对象
        /// </summary>
        /// <param name="key">锁的唯一标识</param>
        /// <param name="lockType">锁类型</param>
        /// <param name="millisecondsTimeout">等待的毫秒数；<=0表示无限期等待。</param>
        /// <returns>返回锁对象</returns>
        /// <exception cref="TimeoutException">获取锁对象超时时，抛出此异常</exception>
        /// <remarks>
        /// 对同一个锁操作时，读的代码块内不能包含写的代码块，也不能在写的代码块包含读的代码块，这会导致内部异常,如果确实想要写，请使用<see cref="LockTypeEnum.EnterUpgradeableReader"/>，比如：
        /// <code>
        /// using("test", LockTypeEnum.EnterUpgradeableReader)
        /// {
        ///     using("test",LockTypeEnum.Write)
        ///     {
        ///         // do something
        ///     }
        /// }
        /// </code>
        /// </remarks>
        public IDisposable GetLock(String key, LockTypeEnum lockType, Int32 millisecondsTimeout = 100)
        {
            // 获取锁对象
            ReaderWriterLockSlim lockSlimObj = GetLockSlim(key);

            return GetLock(lockSlimObj, lockType, millisecondsTimeout);
        }

        /// <summary>
        /// 获取锁对象
        /// </summary>
        /// <param name="lockSlimObj">锁对象</param>
        /// <param name="lockType">锁类型</param>
        /// <param name="millisecondsTimeout">等待的毫秒数；&lt;=0表示无限期等待。</param>
        /// <returns>返回锁对象</returns>
        /// <exception cref="TimeoutException">获取锁对象超时时，抛出此异常</exception>
        /// <remarks>
        /// 对同一个锁操作时，读的代码块内不能包含写的代码块，也不能在写的代码块包含读的代码块，这会导致内部异常,如果确实想要写，请使用<see cref="LockTypeEnum.EnterUpgradeableReader"/>，比如：
        /// <code>
        /// using("test", LockTypeEnum.EnterUpgradeableReader)
        /// {
        ///     using("test",LockTypeEnum.Write)
        ///     {
        ///         // do something
        ///     }
        /// }
        /// </code>
        /// </remarks>
        public static IDisposable GetLock(ReaderWriterLockSlim lockSlimObj, LockTypeEnum lockType, Int32 millisecondsTimeout = 100)
        {
            // 判断是否为无限期等待
            if (millisecondsTimeout <= 0)
            {
                return GetLockSlimByInfiniteWait(lockSlimObj, lockType);
            }

            // 进入锁
            switch (lockType)
            {
                case LockTypeEnum.Reader:
                    {
                        if (lockSlimObj.IsUpgradeableReadLockHeld || lockSlimObj.IsReadLockHeld || lockSlimObj.TryEnterReadLock(millisecondsTimeout))
                        {
                            return new CustomMonitor(lockSlimObj, lockType);
                        }

                        throw new TimeoutException("等待读锁超时");
                    }
                case LockTypeEnum.Writer:
                    {
                        if (lockSlimObj.IsWriteLockHeld || lockSlimObj.TryEnterWriteLock(millisecondsTimeout))
                        {
                            return new CustomMonitor(lockSlimObj, lockType);
                        }

                        throw new TimeoutException("等待写锁超时");
                    }
                case LockTypeEnum.EnterUpgradeableReader:
                    {
                        if (lockSlimObj.IsUpgradeableReadLockHeld || lockSlimObj.TryEnterUpgradeableReadLock(millisecondsTimeout))
                        {
                            return new CustomMonitor(lockSlimObj, lockType);
                        }

                        throw new TimeoutException("等待读锁超时");
                    }
            }

            return null;
        }

        /// <summary>
        /// 主动释放锁资源，避免长久驻留内存
        /// </summary>
        /// <param name="key">锁的唯一标识</param>
        public void DisposeLock(String key)
        {
            lock (mLockObj)
            {
                mLockSlimDic.Remove(key);
            }
        }

        /// <summary>
        /// 主动清空所有锁资源，避免长久驻留内存
        /// </summary>
        public void DisposeAllLock()
        {
            lock (mLockObj)
            {
                mLockSlimDic.Clear();
            }
        }

        /// <summary>
        /// 获取锁对象，获取过程会死等。直到获取到锁对象
        /// </summary>
        /// <param name="lockObj">锁对象</param>
        /// <param name="lockType">锁类型</param>
        /// <returns>返回锁对象</returns>
        private static IDisposable GetLockSlimByInfiniteWait(ReaderWriterLockSlim lockObj, LockTypeEnum lockType)
        {
            // 进入锁
            switch (lockType)
            {
                case LockTypeEnum.Reader:
                    {
                        if (lockObj.IsReadLockHeld == false)
                        {
                            lockObj.EnterReadLock();
                        }

                        return new CustomMonitor(lockObj, lockType);
                    }
                case LockTypeEnum.Writer:
                    {
                        if (lockObj.IsWriteLockHeld == false)
                        {
                            lockObj.EnterWriteLock();
                        }

                        return new CustomMonitor(lockObj, lockType);
                    }
                case LockTypeEnum.EnterUpgradeableReader:
                    {
                        if (lockObj.IsUpgradeableReadLockHeld == false)
                        {
                            lockObj.EnterUpgradeableReadLock();
                        }

                        return new CustomMonitor(lockObj, lockType);
                    }
            }

            return null;
        }

        /// <summary>
        /// 获取锁对象信息
        /// </summary>
        /// <param name="key">锁的唯一标识</param>
        /// <returns>返回锁对象</returns>
        private ReaderWriterLockSlim GetLockSlim(String key)
        {
            lock (mLockObj)
            {
                if (!mLockSlimDic.ContainsKey(key))
                {
                    // 如果获取不到锁，则创建一个锁对象
                    ReaderWriterLockSlim lockObj = new ReaderWriterLockSlim();
                    mLockSlimDic[key] = lockObj;

                    return lockObj;
                }

                return mLockSlimDic[key];
            }
        }
    }
}
