// ****************************************
// FileName:DoubleUtil.cs
// Description: Double助手类
// Tables:Nothing
// Author:Jordan Zuo
// Create Date:2013-10-10
// Revision History:
// 1、由于Random不是线程安全的，所以添加了锁
// 2、添加了批量生成随机数的方法
// ****************************************

using System;

namespace Util.Math
{
    /// <summary>
    /// Double助手类
    /// </summary>
    public static class DoubleUtil
    {
        // 随机数对象。
        private static Random mRandom = new Random();
        private static Object mLockObj = new Object();

        /// <summary>
        /// 返回一个介于 0.0 和 1.0 之间的随机数。
        /// </summary>
        /// <returns>大于等于 0.0 并且小于 1.0 的双精度浮点数</returns>
        public static Double GetRandNum()
        {
            lock (mLockObj)
            {
                return mRandom.NextDouble();
            }
        }
    }
}