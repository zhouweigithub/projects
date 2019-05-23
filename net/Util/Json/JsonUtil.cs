// ****************************************
// FileName:JsonUtil.cs
// Description:JSON助手类
// Tables:Nothing
// Author:Jordan Zuo
// Create Date:2013-10-10
// Revision History:
// Mendor:Jordan Zuo
// Amend Date:2016-07-05
// ****************************************

using System;
using System.Collections.Generic;

namespace Util.Json
{
    using Newtonsoft.Json;

    /// <summary>
    /// JSON助手类
    /// </summary>
    public static class JsonUtil
    {
        /// <summary>
        /// 序列化对象为Json数据
        /// </summary>
        /// <param name="obj">序列对象</param>
        /// <param name="format">格式</param>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.InvalidOperationException"></exception>
        /// <returns>序列化后的字符串</returns>
        public static String Serialize(Object obj, Formatting format = Formatting.None)
        {
            if (obj == null) throw new ArgumentNullException("Error", "obj can't be null.");

            return JsonConvert.SerializeObject(obj, format);
        }

        /// <summary>
        /// 序列化Dictionary对象为Json数据
        /// </summary>
        /// <param name="obj">Dictionary对象</param>
        /// <param name="format">格式</param>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.InvalidOperationException"></exception>
        /// <returns>序列化后的字符串</returns>
        public static String Serialize(Dictionary<String, Object> obj, Formatting format = Formatting.None)
        {
            if (obj == null || obj.Count == 0) throw new ArgumentNullException("Error", "obj can't be empty.");

            return Serialize((Object)obj, format);
        }

        /// <summary>
        /// 序列化数据集合为Json数据
        /// </summary>
        /// <param name="keys">key数组</param>
        /// <param name="values">value数组</param>
        /// <param name="obj">Dictionary对象</param>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.InvalidOperationException"></exception>
        /// <returns>序列化后的字符串</returns>
        public static String Serialize(String[] keys, Object[] values, Formatting format = Formatting.None)
        {
            if (keys == null || keys.Length == 0) throw new ArgumentNullException("Error", "keys can't be empty.");
            if (values == null || values.Length == 0) throw new ArgumentNullException("Error", "values can't be empty.");

            //组装数据
            Dictionary<String, Object> obj = new Dictionary<String, Object>();
            for (Int32 i = 0; i < keys.Length; i++)
            {
                obj[keys[i]] = values[i];
            }

            //序列化数据
            return Serialize(obj, format);
        }

        /// <summary>
        /// 反序列化Json数据为指定对象
        /// </summary>
        /// <typeparam name="T">指定返回对象</typeparam>
        /// <param name="str">Json字符串</param>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.InvalidOperationException"></exception>
        /// <returns>反序列化后的T对象</returns>
        public static T Deserialize<T>(String str)
        {
            if (String.IsNullOrEmpty(str)) throw new ArgumentNullException("Error", "str can't be empty or null.");

            return JsonConvert.DeserializeObject<T>(str);
        }

        /// <summary>
        /// 反序列化Json数据为Dictionary对象
        /// </summary>
        /// <param name="str">Json字符串</param>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.InvalidOperationException"></exception>
        /// <returns>反序列化后的Dictionary对象</returns>
        public static Dictionary<String, Object> Deserialize(String str)
        {
            if (String.IsNullOrEmpty(str)) throw new ArgumentNullException("Error", "str can't be empty or null.");

            return JsonConvert.DeserializeObject<Dictionary<String, Object>>(str);
        }
    }
}