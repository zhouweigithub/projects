// ****************************************
// FileName:LockUtil.cs
// Description: 锁助手类-共享锁
// Tables:Nothing
// Author:Jordan Zuo
// Create Date:2013-10-10
// Revision History:
//  Mendor:Jordan Zuo
//  Mend Date:2016-5-19
//  Mend Content:增加了返回读写锁的方法
// ****************************************

using System;
using System.Collections.Generic;

namespace Util.Lock
{
    /// <summary>
    /// 锁助手类-共享锁
    /// </summary>
    public class LockUtil
    {
        /// <summary>
        /// 定义全局锁对象
        /// </summary>
        private Object mLockObj = new Object();

        /// <summary>
        /// 定义存放所有锁的列表
        /// </summary>
        private Dictionary<String, Object> mLockObjDic = new Dictionary<String, Object>();

        /// <summary>
        /// 根据key获取锁对象
        /// </summary>
        /// <param name="key">锁的唯一标识</param>
        /// <returns>锁对象</returns>
        public Object GetLock(String key)
        {
            lock (this.mLockObj)
            {
                if (!this.mLockObjDic.ContainsKey(key))
                {
                    Object newLockObj = new Object();
                    this.mLockObjDic[key] = newLockObj;

                    return newLockObj;
                }

                return this.mLockObjDic[key];
            }
        }

        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="key">锁的唯一标识</param>
        public void ReleaseLock(String key)
        {
            lock (this.mLockObj)
            {
                this.mLockObjDic.Remove(key);
            }
        }

        /// <summary>
        /// 主动清空所有锁资源，避免长久驻留内存
        /// </summary>
        public void ReleaseAllLock()
        {
            lock (this.mLockObj)
            {
                this.mLockObjDic.Clear();
            }
        }
    }
}
