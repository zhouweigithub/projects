// ****************************************
// FileName:ByteUtil.cs
// Description: Byte助手类
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
    /// Byte助手类
    /// </summary>
    public static class ByteUtil
    {
        private static Random mRandom = new Random();
        private static Object mLockObj = new Object();

        /// <summary>
        ///  一个大于等于 minValue 且小于或等于 maxValue 的 8 位无符号整数
        /// </summary>
        /// <param name="minValue">返回的随机数的下界（随机数可取该下界值）</param>
        /// <param name="maxValue">返回的随机数的上界maxValue 必须大于等于 minValue</param>
        /// <param name="includeMaxValue">是否包括上限值</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <returns>随机数</returns>
        public static Byte GetRandNum(Byte minValue, Byte maxValue, IncludeMaxValue includeMaxValue = IncludeMaxValue.No)
        {
            if (minValue > maxValue) throw new ArgumentOutOfRangeException("minValue", "minValue can't be bigger than maxValue.");

            lock (mLockObj)
            {
                if (includeMaxValue == IncludeMaxValue.Yes)
                {
                    return (Byte)(mRandom.Next(minValue, maxValue + 1));
                }
                else
                {
                    return (Byte)(mRandom.Next(minValue, maxValue));
                }
            }
        }

        /// <summary>
        /// 交换字节
        /// </summary>
        /// <param name="byteArray">字节数组</param>
        /// <param name="i">第一个字符</param>
        /// <param name="j">第二个字符</param>
        /// <returns>交换位置后的字节数组</returns>
        public static Byte[] SwapByte(Byte[] byteArray, Int32 i, Int32 j)
        {
            Byte tmpByte = byteArray[i];
            byteArray[i] = byteArray[j];
            byteArray[j] = tmpByte;

            return byteArray;
        }

        /// <summary>
        /// 合并两个字节数组
        /// </summary>
        /// <param name="a">字节数组</param>
        /// <param name="b">字节数组</param>
        /// <returns></returns>
        public static Byte[] Combine(Byte[] a, Byte[] b)
        {
            Byte[] result = new Byte[a.Length + b.Length];
            Int32 index = 0;

            //先合并a到数组
            foreach (Byte item in a)
            {
                result[index++] = item;
            }

            //再合并a到数组
            foreach (Byte item in b)
            {
                result[index++] = item;
            }

            return result;
        }

        /// <summary>
        /// 获取子数组
        /// </summary>
        /// <param name="byteArray">字节数组</param>
        /// <param name="lowerIndex">开始索引</param>
        /// <param name="length">长度</param>
        /// <returns>子数组</returns>
        public static Byte[] GetSubArray(Byte[] byteArray, Int32 lowerIndex, Int32 length)
        {
            if (lowerIndex < 0 || lowerIndex > byteArray.Length - 1 || byteArray.Length < lowerIndex + length)
            {
                throw new IndexOutOfRangeException("数组的索引越界，请检查参数");
            }

            Byte[] subArray = new Byte[length];
            for (Int32 i = 0; i < length; i++)
            {
                subArray[i] = byteArray[lowerIndex + i];
            }

            return subArray;
        }
    }
}