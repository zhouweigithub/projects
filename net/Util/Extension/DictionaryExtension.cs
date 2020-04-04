//***********************************************************************************
//文件名称：DictionaryExtension.cs
//功能描述：字典扩展类
//数据表：Nothing
//作者：byron
//日期：2014/4/30 17:58:00
//修改记录：
//***********************************************************************************

using System;
using System.Linq;
using System.Collections.Generic;

namespace Util.Extension
{
    /// <summary>
    /// Dictionary扩展类
    /// </summary>
    public static class DictionaryExtension
    {
        /// <summary>
        /// 判断关键字是否都在字典中存在
        /// </summary>
        /// <typeparam name="T1">字典的key类型</typeparam>
        /// <typeparam name="T2">字典的value类型</typeparam>
        /// <param name="dict">字典</param>
        /// <param name="keys">要检测的关键字对象</param>
        /// <returns>如果关键字都存在，则为true，否则为false</returns>
        public static Boolean AllKeysIsExist<T1, T2>(this Dictionary<T1, T2> dict, params T1[] keys)
        {
            if (keys == null)
                return false;

            return keys.All(a => dict.ContainsKey(a));
        }

        /// <summary>
        /// 判断关键字是否都在字典中存在
        /// </summary>
        /// <typeparam name="T1">字典的key类型</typeparam>
        /// <typeparam name="T2">字典的value类型</typeparam>
        /// <param name="dict">字典</param>
        /// <param name="keys">要检测的关键字对象</param>
        /// <returns>如果关键字都存在，则为true，否则为false</returns>
        public static Boolean AllKeysIsExist<T1, T2>(this Dictionary<T1, T2> dict, IEnumerable<T1> keys)
        {
            if (keys == null)
                return false;

            return keys.All(a => dict.ContainsKey(a));
        }
    }
}