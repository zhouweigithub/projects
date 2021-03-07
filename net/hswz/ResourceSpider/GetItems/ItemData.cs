using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

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
            String md5 = Util.Security.MD5Util.MD5(data);
            if (!pageLinkHistory.Contains(md5))
            {
                pageLinkQueue.Enqueue(data);
                pageLinkHistory.Add(md5);
            }
        }
    }
}
