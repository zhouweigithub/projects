using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.Common
{
    public class CommonDatas
    {
        /// <summary>
        /// 待处理的URL队列
        /// </summary>
        public static ConcurrentQueue<string> UrlQueue = new ConcurrentQueue<string>();

        /// <summary>
        /// 已经处理过的URL队列
        /// </summary>
        public static ConcurrentQueue<string> DealedUrlQueue = new ConcurrentQueue<string>();

        /// <summary>
        /// URL正则式
        /// </summary>
        public static readonly string RegUrlString = @"(http(s)?):\/\/([\w\-]+(\.[\w\-]+)*\/)*[\w\-]+(\.[\w\-]+)*\/?(\?([\w\-\.,@?^=%&:\/~\+#]*)+)?";

        /// <summary>
        /// 链接正则式
        /// </summary>
        public static readonly string RegLinkString = "<a(.|\\n)+?href=\"(?<url>(.|\\n)*?)\"(.|\\n)*?>(?<text>(.|\\n)*?)</a>";

        /// <summary>
        /// 数字正则式
        /// </summary>
        public static readonly string RegNumber = "^\\d+$";

        /// <summary>
        /// 页面标题正则式
        /// </summary>
        public static readonly string RegTitleString = "<title>(?<Title>(.|\\n)*?)</title>";

        /// <summary>
        /// 需要排除的类型（小写）
        /// </summary>
        public static readonly string[] ExpceptTypes = new string[] { ".css", ".js", ".png", ".gif", "ico",
            ".jpg", ".jpeg", ".doc", ".docs", ".flv", ".mp3", ".mp4", ".rmvb", ".pdf", ".xls", ".xlsx" };

    }
}
