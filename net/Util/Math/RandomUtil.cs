// ****************************************
// FileName:RandomUtil.cs
// Description: 随机数助手类
// Tables:Nothing
// Author:Jordan Zuo
// Create Date:2015-12-14
// Revision History:
// ****************************************

using System;
using System.Linq;
using System.Collections.Generic;

namespace Util.Math
{
    /// <summary>
    /// 随机数助手类
    /// </summary>
    public static class RandomUtil
    {
        /// <summary>
        /// 权重函数
        /// </summary>
        /// <typeparam name="T">数据项类型</typeparam>
        /// <param name="item">数据项</param>
        /// <returns>符合条件的数据项</returns>
        public delegate Int32 ItemWeight<T>(T item);

        /// <summary>
        /// 获取一个随机项
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="source">源数据集合</param>
        /// <returns>随机项</returns>
        public static T GetRandItem<T>(IList<T> source)
        {
            Int32 randIndex = IntUtil.GetRandNum(0, source.Count, IncludeMaxValue.No);

            return source[randIndex];
        }

        /// <summary>
        /// 根据权值获取随机项
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="source">源数据集合</param>
        /// <param name="itemWeight">权重函数</param>
        /// <returns>随机项</returns>
        public static T GetRandItem<T>(IList<T> source, ItemWeight<T> itemWeight)
        {
            //获取总的权重值
            Int32 totalWeight = source.Sum(p => itemWeight(p));

            //获取随机数
            Int32 randNum = IntUtil.GetRandNum(1, totalWeight, IncludeMaxValue.Yes);

            //遍历，找到符合条件的数据项
            Int32 currWeight = 0;
            foreach (var item in source)
            {
                currWeight += itemWeight(item);
                if (randNum <= currWeight)
                {
                    return item;
                }
            }

            throw new Exception("没有找到权重匹配的数据项");
        }

        /// <summary>
        /// 获取随机列表
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="source">源列表</param>
        /// <param name="count">随机数量</param>
        /// <param name="ifAllowDuplicate">是否允许重复</param>
        /// <returns>随机后结果</returns>
        public static List<T> GetRandList<T>(IList<T> source, Int32 count, Boolean ifAllowDuplicate)
        {
            if (source.Count < count)
            {
                throw new ArgumentOutOfRangeException("随机的数量超过列表的元素数量");
            }

            //使用源列表的数据量来初始化一个仅存放索引值的数组
            Int32[] indexList = new Int32[source.Count];
            for (Int32 index = 0; index < indexList.Length; index++)
            {
                indexList[index] = index;
            }

            //遍历列表并获取随机对象            
            List<T> resultList = new List<T>();
            Random random = new Random(Guid.NewGuid().GetHashCode());

            Int32 maxIndex = indexList.Length - 1;
            while (resultList.Count < count)
            {
                //获取随机索引(由于Next方法不取上限值，所以需要maxIndex+1)
                Int32 randIndex = random.Next(0, maxIndex + 1);

                //将数据添加到列表，并增加findCount
                resultList.Add(source[indexList[randIndex]]);

                //如果不允许重复，则需要特殊处理
                if (!ifAllowDuplicate)
                {
                    //并将该位置的数据设置为当前遍历的最大值
                    indexList[randIndex] = indexList[maxIndex];

                    //将随机的范围缩小
                    maxIndex--;
                }
            }

            return resultList;
        }

        /// <summary>
        /// 获取随机数列表（1~10000，超过10000会抛出异常）
        /// </summary>
        /// <param name="minValue">获取随机数的区间下限值</param>
        /// <param name="maxValue">获取随机数的区间上限值</param>
        /// <param name="count">随机数量</param>
        /// <param name="ifAllowDuplicate">是否允许重复</param>
        /// <returns>随机数列表</returns>
        public static List<Int32> GetRandNumList(Int32 minValue, Int32 maxValue, Int32 count, Boolean ifAllowDuplicate)
        {
            if (minValue > maxValue) throw new ArgumentOutOfRangeException("minValue", "minValue can't be bigger than maxValue.");
            if ((maxValue - minValue + 1) < count) throw new ArgumentOutOfRangeException("随机的数量超过区间的元素数量");
            if ((maxValue - minValue + 1) > 10000) throw new ArgumentOutOfRangeException("随机数的区间不能大于10000");

            List<Int32> resultList = new List<Int32>();

            //如果允许重复，则直接随机；否则调用GetRandList来随机
            if (ifAllowDuplicate)
            {
                Random random = new Random();
                while (resultList.Count < count)
                {
                    resultList.Add(random.Next(minValue, maxValue + 1));
                }
            }
            else
            {
                Int32[] inArray = new Int32[maxValue - minValue + 1];
                for (Int32 index = 0; index < inArray.Length; index++)
                {
                    inArray[index] = minValue + index;
                }

                resultList = GetRandList(inArray, count, false);
            }

            return resultList;
        }
    }
}
