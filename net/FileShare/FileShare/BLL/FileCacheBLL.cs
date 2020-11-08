using System;
using System.Collections.Generic;
using FileShare.Models;

namespace FileShare.BLL
{
    public static class FileCacheBLL
    {
        /// <summary>
        /// 缓存数据
        /// </summary>
        private static readonly Dictionary<String, List<FileDetail>> cache = new Dictionary<String, List<FileDetail>>();

        /// <summary>
        /// 最后一次清空缓存的时间
        /// </summary>
        private static DateTime lastClearTime = default;




        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<FileDetail> GetData(String path)
        {
            path = path.Replace("\\", "/").TrimStart('/');
            if (cache.ContainsKey(path))
            {
                var list = cache[path].GetRange(0, cache[path].Count);

                if (DateTime.Now.Subtract(lastClearTime).TotalMinutes > 10)
                {   //如果距离上次清空缓存时间超过10分钟，则再次清空缓存
                    cache.Clear();
                    lastClearTime = DateTime.Now;
                }

                return list;
            }
            else
                return new List<FileDetail>();
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="path"></param>
        /// <param name="details"></param>
        public static void Add(String path, List<FileDetail> details)
        {
            path = path.Replace("\\", "/").TrimStart('/');
            cache[path] = details.GetRange(0, details.Count);
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="path"></param>
        public static void Remove(String path)
        {
            path = path.Replace("\\", "/").TrimStart('/');
            cache.Remove(path);
        }

        /// <summary>
        /// 是否存在缓存
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Boolean IsExists(String path)
        {
            path = path.Replace("\\", "/").TrimStart('/');
            return cache.ContainsKey(path);
        }
    }
}