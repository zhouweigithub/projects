using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ResourceSpider.GetItems
{
    public class ItemData
    {
        /// <summary>
        /// 待处理的分页链接
        /// </summary>
        private static readonly ConcurrentQueue<String> pageLinkQueue;
        /// <summary>
        /// 分页链接历史记录
        /// </summary>
        private static readonly List<String> pageLinkHistory;


        static ItemData()
        {
            pageLinkQueue = new ConcurrentQueue<String>();
            pageLinkHistory = new List<String>();
        }

        /// <summary>
        /// 获取下一个分页链接
        /// </summary>
        /// <returns></returns>
        public static String GetPageLink()
        {
            if (pageLinkQueue.TryDequeue(out String data))
            {
                return data;
            }
            else
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// 添加记录，自动去重
        /// </summary>
        /// <param name="data"></param>
        public static void AddPageLink(String data)
        {
            if (!pageLinkHistory.Contains(data))
            {
                pageLinkQueue.Enqueue(data);
                pageLinkHistory.Add(data);
            }
        }

        /// <summary>
        /// 检测是否已经存在以其为开头的链接
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Boolean IsPartofSoMeOne(String url)
        {
            return pageLinkHistory.Any(a => a.StartsWith(url));
        }
    }
}
