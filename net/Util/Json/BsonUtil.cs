// ****************************************
// FileName:BsonUtil.cs
// Description:BSON助手类
// Tables:Nothing
// Author:Jordan Zuo
// Create Date:2013-10-10
// Revision History:
// ****************************************

using System;
using System.IO;
using System.Collections.Generic;

namespace Util.Json
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Bson;

    /// <summary>
    /// BSON助手类
    /// </summary>
    public static class BsonUtil
    {
        /// <summary>
        /// 序列化对象为Bson数据
        /// </summary>
        /// <param name="obj">序列对象</param>
        /// <exception cref="System.ArgumentException">System.ArgumentException</exception>
        /// <exception cref="System.ArgumentNullException">System.ArgumentNullException</exception>
        /// <exception cref="System.InvalidOperationException">System.InvalidOperationException</exception>
        /// <returns>序列化后的字符串</returns>
        public static Byte[] Serialize(Object obj)
        {
            if (obj == null) throw new ArgumentNullException("Error", "obj can't be null.");

            //构造序列化所需对象
            MemoryStream ms = new MemoryStream();
            JsonSerializer serializer = new JsonSerializer();

            // serialize product to BSON
            BsonWriter writer = new BsonWriter(ms);
            serializer.Serialize(writer, obj);

            return ms.ToArray();
        }

        /// <summary>
        /// 序列化Dictionary对象为Bson数据
        /// </summary>
        /// <param name="obj">Dictionary对象</param>
        /// <exception cref="System.ArgumentException">System.ArgumentException</exception>
        /// <exception cref="System.ArgumentNullException">System.ArgumentNullException</exception>
        /// <exception cref="System.InvalidOperationException">System.InvalidOperationException</exception>
        /// <returns>序列化后的字符串</returns>
        public static Byte[] Serialize(Dictionary<String, Object> obj)
        {
            if (obj == null || obj.Count == 0) throw new ArgumentNullException("Error", "obj can't be empty.");

            return Serialize((Object)obj);
        }

        /// <summary>
        /// 序列化数据集合为Bson数据
        /// </summary>
        /// <param name="keys">key数组</param>
        /// <param name="values">value数组</param>
        /// <exception cref="System.ArgumentException">System.ArgumentException</exception>
        /// <exception cref="System.ArgumentNullException">System.ArgumentNullException</exception>
        /// <exception cref="System.InvalidOperationException">System.InvalidOperationException</exception>
        /// <returns>序列化后的字符串</returns>
        public static Byte[] Serialize(String[] keys, Object[] values)
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
            return Serialize(obj);
        }

        /// <summary>
        /// 反序列化Json数据为指定对象
        /// </summary>
        /// <typeparam name="T">指定返回对象</typeparam>
        /// <param name="byteArray">Bson二进制数据</param>
        /// <exception cref="System.ArgumentException">System.ArgumentException</exception>
        /// <exception cref="System.ArgumentNullException">System.ArgumentNullException</exception>
        /// <exception cref="System.InvalidOperationException">System.InvalidOperationException</exception>
        /// <returns>反序列化后的T对象</returns>
        public static T Deserialize<T>(Byte[] byteArray)
        {
            if (byteArray == null || byteArray.Length == 0) throw new ArgumentNullException("Error", "byteArray can't be empty or null.");

            //构造序列化所需对象
            MemoryStream ms = new MemoryStream(byteArray);
            JsonSerializer serializer = new JsonSerializer();

            // deserialize product from BSON
            BsonReader reader = new BsonReader(ms);
            return serializer.Deserialize<T>(reader);         
        }

        /// <summary>
        /// 反序列化Json数据为Dictionary对象
        /// </summary>
        /// <param name="byteArray">Bson二进制数据</param>
        /// <exception cref="System.ArgumentException">System.ArgumentException</exception>
        /// <exception cref="System.ArgumentNullException">System.ArgumentNullException</exception>
        /// <exception cref="System.InvalidOperationException">System.InvalidOperationException</exception>
        /// <returns>反序列化后的Dictionary对象</returns>
        public static Dictionary<String, Object> Deserialize(Byte[] byteArray)
        {
            if (byteArray == null || byteArray.Length == 0) throw new ArgumentNullException("Error", "byteArray can't be empty or null.");

            return Deserialize<Dictionary<String, Object>>(byteArray);
        }
    }
}