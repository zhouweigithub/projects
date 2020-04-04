//***********************************************************************************
//文件名称：ReflectionCache.cs
//功能描述：使用反射的方式，提供反射出的类的缓存
//数据表：Nothing
//作者：byron
//日期：2014/3/12 14:12:06
//修改记录：
//***********************************************************************************

using System;
using System.Reflection;
using System.Collections.Concurrent;

namespace Util.Reflection
{
    /// <summary>
    /// 类型缓存类
    /// </summary>
    static class ReflectionCache
    {
        #region 字段

        //缓存已经反射的方法对象
        private static ConcurrentDictionary<String, MethodInfo> mMethodCache = new ConcurrentDictionary<String, MethodInfo>();

        //缓存已经反射的类型对象
        private static ConcurrentDictionary<String, Type> mTypeCache = new ConcurrentDictionary<String, Type>();

        #endregion

        #region 方法

        /// <summary>
        /// 获取缓存的方法
        /// </summary>
        /// <param name="key">方法对应的key</param>
        /// <returns>如果key对应的方法存在，则返回；否则返回null</returns>
        public static MethodInfo GetMehtod(String key)
        {
            return mMethodCache.ContainsKey(key) ? mMethodCache[key] : null;
        }

        /// <summary>
        /// 设置方法缓存
        /// </summary>
        /// <param name="key">方法对应的key</param>
        /// <param name="method">要缓存的方法对象</param>
        public static void SetMethod(String key, MethodInfo method)
        {
            mMethodCache[key] = method;
        }

        /// <summary>
        /// 根据key取出缓存的方法
        /// </summary>
        /// <param name="key">类型对应的key</param>
        /// <returns>如果key对应的类型存在，则返回对应的类型，否则返回null</returns>
        public static Type GetType(String key)
        {
            return mTypeCache.ContainsKey(key) ? mTypeCache[key] : null;
        }

        /// <summary>
        /// 将方法放入缓存中，如果方法已经存在，则覆盖；不存在，则添加
        /// </summary>
        /// <param name="key">方法对应的key</param>
        /// <param name="type">要缓存的方法</param>
        public static void SetType(String key, Type type)
        {
            mTypeCache[key] = type;
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        public static void Clear()
        {
            mMethodCache.Clear();
            mTypeCache.Clear();
        }

        #endregion
    }
}