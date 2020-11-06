using System;
using System.Collections.Generic;
using System.IO;

namespace FileShare.BLL
{
    /// <summary>
    /// 操作记录缓存（记录哪些ip操作了哪些文件，用于决定是否可删除文件）
    /// </summary>
    public static class LogCacheBLL
    {

        /// <summary>
        /// 缓存数据
        /// </summary>
        private static readonly Dictionary<String, List<String>> cache = new Dictionary<String, List<String>>();

        static LogCacheBLL()
        {
            Refresh();
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="ip">操作者地址</param>
        /// <param name="path">文件或目录路径</param>
        public static void Add(String ip, String path)
        {
            if (!cache.ContainsKey(ip))
                cache[ip] = new List<String>();

            path = path.Replace("\\", "/").TrimStart('/');
            cache[ip].Add(path);
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="ip">操作者地址</param>
        /// <param name="path">文件或目录路径</param>
        public static void Remove(String ip, String path)
        {
            path = path.Replace("\\", "/").TrimStart('/');
            if (cache.ContainsKey(ip))
                cache[ip].Remove(path);
        }

        /// <summary>
        /// 是否存在缓存
        /// </summary>
        /// <param name="ip">操作者地址</param>
        /// <param name="path">文件或目录路径</param>
        /// <returns></returns>
        public static Boolean IsExists(String ip, String path)
        {
            path = path.Replace("\\", "/").TrimStart('/');
            return cache.ContainsKey(ip) && cache[ip].Contains(path);
        }

        /// <summary>
        /// 根据日志重建缓存
        /// </summary>
        public static void Refresh()
        {
            cache.Clear();

            //所有操作的日志文件名
            String logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Common.UploadLogFolder, DateTime.Today.ToString("yyyy-MM-dd") + ".txt");
            if (!File.Exists(logFilePath))
                return;


            String[] lines = File.ReadAllLines(logFilePath);
            foreach (String line in lines)
            {
                String[] strArray = line.Split(new Char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                if (strArray.Length != 4)
                    continue;

                //line结构: time|ip|type|name

                if (!cache.ContainsKey(strArray[1]))
                    cache.Add(strArray[1], new List<String>());

                if (strArray[2] == "Upload")
                    cache[strArray[1]].Add(strArray[3]);
                else if (strArray[2] == "Delete")
                    cache[strArray[1]].Remove(strArray[3]);
            }

        }

    }
}