// ****************************************
// FileName:IntUtil.cs
// Description: Int助手类
// Tables:Nothing
// Author:Jordan Zuo
// Create Date:2013-10-10
// Revision History:
// Jordan Zuo 2015-12-14 
// 1、由于Random不是线程安全的，所以添加了锁
// 2、添加了批量生成随机数的方法
// ****************************************

using System;
using System.Collections.Generic;

namespace Util.Math
{
    /// <summary>
    /// Int助手类
    /// </summary>
    public static class IntUtil
    {
        private static Random mRandom = new Random();
        private static Object mLockObj = new Object();

        /// <summary>
        ///  一个大于等于 minValue 且小于或等于 maxValue 的 32 位带符号整数
        /// </summary>
        /// <param name="minValue">返回的随机数的下界（随机数可取该下界值）</param>
        /// <param name="maxValue">返回的随机数的上界maxValue 必须大于等于 minValue</param>
        /// <param name="includeMaxValue">是否包括上限值</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <returns>随机数</returns>
        public static Int32 GetRandNum(Int32 minValue, Int32 maxValue, IncludeMaxValue includeMaxValue = IncludeMaxValue.No)
        {
            if (minValue > maxValue) throw new ArgumentOutOfRangeException("minValue", "minValue can't be bigger than maxValue.");

            lock (mLockObj)
            {
                if (includeMaxValue == IncludeMaxValue.Yes)
                {
                    return mRandom.Next(minValue, maxValue + 1);
                }
                else
                {
                    return mRandom.Next(minValue, maxValue);
                }
            }
        }

        /// <summary>
        /// 将字符串数组转化为Int32列表
        /// </summary>
        /// <param name="strArray">字符串数组</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.FormatException"></exception>
        /// <exception cref="System.OverflowException"></exception>
        /// <returns>Int32列表</returns>
        public static IEnumerable<Int32> ConvertToInt32List(String[] strArray)
        {
            if (strArray == null) throw new ArgumentNullException("strArray", "strArray can't be null.");

            List<Int32> intList = new List<Int32>();
            for (Int32 i = 0; i < strArray.Length; i++)
            {
                intList.Add(Convert.ToInt32(strArray[i]));
            }

            return intList;
        }

        /// <summary>
        /// 将字符串数组转化为Int64列表
        /// </summary>
        /// <param name="strArray">字符串数组</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.FormatException"></exception>
        /// <exception cref="System.OverflowException"></exception>
        /// <returns>Int64列表</returns>
        public static IEnumerable<Int64> ConvertToInt64List(String[] strArray)
        {
            if (strArray == null) throw new ArgumentNullException("strArray", "strArray can't be null.");

            List<Int64> intList = new List<Int64>();
            for (Int32 i = 0; i < strArray.Length; i++)
            {
                intList.Add(Convert.ToInt64(strArray[i]));
            }

            return intList;
        }

        /// <summary>
        /// 计算数据
        /// </summary>
        /// <param name="value">用于计算的值</param>
        /// <param name="multiplier">乘数</param>
        /// <param name="divisor">除数</param>
        /// <returns>计算后结果</returns>
        /// <exception cref="OverflowException">如果计算的结果溢出，则抛出异常</exception>
        public static Int32 CalcForInt32(Int32 value, Int32 multiplier, Int32 divisor)
        {
            Double result = (Double)value;
            result *= (Double)multiplier;
            result /= (Double)divisor;

            //判断是否超出界限
            if (result > Int32.MaxValue)
            {
                throw new OverflowException(String.Format("计算的结果{0}大于Int32的最大值{1}，数据溢出", result.ToString(), Int32.MaxValue.ToString()));
            }

            return (Int32)result;
        }

        /// <summary>
        /// 计算数据
        /// </summary>
        /// <param name="value">用于计算的值</param>
        /// <param name="multiplier">乘数</param>
        /// <param name="divisor">除数</param>
        /// <returns>计算后结果</returns>
        /// <exception cref="OverflowException">如果计算的结果溢出，则抛出异常</exception>
        public static Int64 CalcForInt64(Int64 value, Int64 multiplier, Int64 divisor)
        {
            Double result = (Double)value;
            result *= (Double)multiplier;
            result /= (Double)divisor;

            //判断是否超出界限
            if (result > Int64.MaxValue)
            {
                throw new OverflowException(String.Format("计算的结果{0}大于Int64的最大值{1}，数据溢出", result.ToString(), Int32.MaxValue.ToString()));
            }

            return (Int64)result;
        }
    }
}