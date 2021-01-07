using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using HtmlSpider.Model;

namespace HtmlSpider
{
    public class PageBLL
    {
        /// <summary>
        /// 域名与线程id的关系
        /// </summary>
        private static readonly Dictionary<String, Int32> domainDic = new Dictionary<String, Int32>();

        /// <summary>
        /// 已经请求过的URL
        /// </summary>
        private static readonly Dictionary<String, List<String>> requestedUrlDic = new Dictionary<String, List<String>>();

        private static readonly Dictionary<String, List<String>> notRequestedUrlDic = new Dictionary<String, List<String>>();

        private static readonly Object domainLockObject = new Object();

        public static void Reset()
        {
            lock (domainLockObject)
            {
                domainDic.Clear();
            }

            requestedUrlDic.Clear();
            notRequestedUrlDic.Clear();
        }

        public static void LoopGetPageInfo(String url, String domain)
        {
            PageInfo p = GetPageInfo(url, String.Empty);
            while (notRequestedUrlDic.Keys.Count > 0)
            {
                var keys = notRequestedUrlDic.Keys;
                foreach (var item in keys)
                {
                    var urls = notRequestedUrlDic[item];
                    foreach (String urlItem in urls)
                    {
                        PageInfo pi = GetPageInfo(urlItem, item);
                    }
                }
            }
        }

        public static PageInfo GetPageInfo(String url, String domain)
        {
            if (!url.StartsWith("http://") && !url.StartsWith("https://") && !String.IsNullOrEmpty(domain))
            {
                url = "http://" + domain + url;
            }

            Uri uri = new Uri(url);
            lock (domainLockObject)
            {
                if (!domainDic.ContainsKey(uri.Host))
                {
                    domainDic.Add(uri.Host, Thread.CurrentThread.ManagedThreadId);
                }
                else
                {
                    if (domainDic[uri.Host] != Thread.CurrentThread.ManagedThreadId)
                    {
                        return null;
                    }
                }
            }

            if (requestedUrlDic.ContainsKey(uri.Host) && requestedUrlDic[uri.Host].Contains(uri.AbsoluteUri))
            {
                return null;
            }

            (String html, String charset) = Common.GetRemoteHtml(url, Encoding.Default);

            if (String.IsNullOrEmpty(html))
            {
                return null;
            }

            html = html.Replace("\r", String.Empty).Replace("\n", String.Empty).Replace("\t", String.Empty);

            PageInfo result = new PageInfo
            {
                Url = url,
                Domain = uri.Host,
                Title = Common.GetTitle(html),
                H1 = Common.GetH1(html),
                KeyWords = Common.GetKeywords(html),
                Content = Common.GetContent(html),
                Hrefs = Common.GetHrefs(html),
            };

            if (!notRequestedUrlDic.ContainsKey(uri.Host))
            {
                notRequestedUrlDic.Add(uri.Host, new List<String>());
            }

            foreach (var item in result.Hrefs)
            {
                notRequestedUrlDic[uri.Host].Add(item);
            }


            if (!requestedUrlDic.ContainsKey(uri.Host))
            {
                requestedUrlDic.Add(uri.Host, new List<String>());
            }

            requestedUrlDic[uri.Host].Add(uri.AbsoluteUri);

            return result;
        }

    }
}
