using System;
using System.Web;

namespace CreateDBmodels.Common
{
    /// <summary>
    /// 通用缓存类
    /// 文件功能描述：模块类，缓存操作
    /// 依赖说明：无依赖
    /// 异常处理：本类不捕获异常
    /// </summary>
    public class Cache
    {
        /// <summary>
        /// 添加缓存 默认 按绝对过期时间存储
        /// </summary>
        /// <param name="cacheKey">键值</param>
        /// <param name="data">数据</param>
        /// <param name="timeOut">超时时间,单位：分</param>
        /// <param name="isAbsolute">是否按绝对时间缓存:超过多少秒后直接过期、相对时间：超过多少时间不调用就失效，默认按绝对时间缓存</param>
        public static void Add(string cacheKey, object data, int timeOut, bool isAbsolute = true)
        {
            timeOut = Math.Max(1, timeOut);
            //HttpContext.Current.Cache.Insert(cacheKey, data, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(timeOut));
            //HttpRuntime.Cache.Insert(cacheKey, data, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(timeOut));
            HttpRuntime.Cache.Insert(cacheKey, data, null, System.Web.Caching.Cache.NoAbsoluteExpiration, isAbsolute ? TimeSpan.Zero : TimeSpan.FromMinutes(timeOut));
        }

        /// <summary>
        /// 删除缓存数据
        /// </summary>
        /// <param name="cacheKey">键值</param>
        public static void Remove(string cacheKey)
        {
            //HttpContext.Current.Cache.Remove(cacheKey);
            HttpRuntime.Cache.Remove(cacheKey);
        }

        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="cacheKey">键值</param>
        /// <returns>找到则返回 object数据,否则为null </returns>
        public static object Get(string cacheKey)
        {
            //return HttpContext.Current.Cache.Get(cacheKey);
            return HttpRuntime.Cache.Get(cacheKey);
        }
    }
}
