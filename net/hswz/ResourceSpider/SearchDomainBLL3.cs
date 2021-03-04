using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Hswz.Common;
using Hswz.DAL;
using Hswz.Model.Urls;

namespace ResourceSpider
{
    public class SearchDomainBLL3
    {
        /// <summary>
        /// 必应搜索结果项
        /// </summary>
        private const String urlRegString = "b_algo.*?<a .*?href=[\"'](?<url>.*?)[\"'].*?>.*?</a>";
        /// <summary>
        /// 必应下一页
        /// </summary>
        private const String nextUrlRegString = "<a [^>]*?sb_pagN_bp.*?href=\"(?<url>.*?)\".*?>.*?</a>";
        /// <summary>
        /// 普通链接
        /// </summary>
        private const String linkRegString = "<a .*?href=[\"'](?<url>.*?)[\"'].*?>.*?</a>";

        private static readonly Regex urlReg = new Regex(urlRegString);
        private static readonly Regex nextUrlReg = new Regex(nextUrlRegString);
        private static readonly Regex linkReg = new Regex(linkRegString);

        private const String userAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4324.190 Safari/537.36";

        private static readonly List<String> httpTypes = new List<String>() { "http://", "https://" };

        /// <summary>
        /// 已经请求过的链接集
        /// </summary>
        private static readonly List<String> requestedUrls = new List<String>();

        /// <summary>
        /// 数据库已经存在的链接
        /// </summary>
        private static readonly List<String> existsUrls = new List<String>();

        /// <summary>
        /// 已经访问过的域名
        /// </summary>
        private static readonly List<String> existedDomains = new List<String>();

        /// <summary>
        /// 已经访问过的搜索链接md5值，防止重复搜索
        /// </summary>
        private static readonly List<String> requestedSearchUrl = new List<String>();


        static SearchDomainBLL3()
        {
            var datas = DBData.GetInstance(DBTable.url).GetList<urls>();
            foreach (var item in datas)
            {
                String url = item.url;
                if (!url.StartsWith("http"))
                {
                    url = "http://" + url;
                }

                String urlMd5 = Util.Security.MD5Util.MD5(url);
                existsUrls.Add(urlMd5);

                if (Uri.TryCreate(url, UriKind.Absolute, out var uri))
                {
                    if (uri.IsAbsoluteUri)
                    {
                        existedDomains.Add(uri.Host);
                    }
                }
            }
        }

        public static void Test()
        {
            //String url = "http://www.duwenzhang.com/wenzhang/aiqingwenzhang";
            //String content = GetPageContent(url);

            //Boolean isValidPage = ValidPage(content);
            //var urls = GetUrls(content);
            //var domains = GetDomains(urls);
            //(String pageUrl, Int32 pageIndex) = GetPageUrl(urls);
        }

        public static void Start()
        {
            WriteLog("start", Util.Log.LogType.Info);

            Do();

            WriteLog("end", Util.Log.LogType.Info);
            Console.ReadKey();
        }

        private static void Do()
        {
            String urlFormat = "/search?q={0}&qs=n&form=QBLH&sp=-1&pq={0}&sc=0-6&sk=&cvid=61EC7E49819341299A065C42F8AC67D0";

            if (String.IsNullOrWhiteSpace(WebConfigData.SearchKeyWords))
            {
                WriteLog("未配置待检索关键字，自动退出", Util.Log.LogType.Info);
                return;
            }

            String[] keywordGroup = WebConfigData.SearchKeyWords.Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (String keyword in keywordGroup)
            {
                //找到有效资源的数量
                Int32 okCount = 0;
                String start = $"开始检索关键字：{keyword}";
                WriteLog(start, Util.Log.LogType.Info);
                String url = String.Format(urlFormat, keyword);
                do
                {
                    url = url.Replace("&amp;", "&");

                    String urlMd5 = Util.Security.MD5Util.MD5(url);
                    if (requestedSearchUrl.Contains(urlMd5))
                    {
                        break;
                    }
                    else
                    {
                        requestedSearchUrl.Add(urlMd5);
                    }

                    Console.WriteLine();
                    WriteLog(url, Util.Log.LogType.Info);
                    String searchContent = HttpHelper.GetHtml("https://cn.bing.com" + url, null, "get", String.Empty, out String _);

                    if (!String.IsNullOrWhiteSpace(searchContent))
                    {
                        //搜索结果中的链接
                        var urls = GetUrls(searchContent, urlReg);

                        //遍历所有搜索到的链接
                        foreach (String linkItem in urls)
                        {
                            DealLink(linkItem, ref okCount, 1);
                        }

                    }

                    url = String.Empty;

                    //检索下一页地址
                    var tmpUrls = GetUrls(searchContent, nextUrlReg);
                    if (tmpUrls.Count > 0)
                    {
                        url = tmpUrls[0];
                    }

                } while (!String.IsNullOrWhiteSpace(url));

                String msg = $"关键字【{keyword}】成功发现有效资源【{okCount}】个";
                WriteLog(msg, Util.Log.LogType.Info);
            }
        }

        /// <summary>
        /// 对链接做处理。
        /// </summary>
        /// <param name="linkItem">链接地址</param>
        /// <param name="okCount">成功数量</param>
        /// <param name="deep">链接深度，如果为1，则继续检测页面里的链接，为2则不再检测页面上的链接</param>
        private static void DealLink(String linkItem, ref Int32 okCount, Int32 deep)
        {
            linkItem = linkItem.Replace("&amp;", "&");

            if (linkItem.Contains("google.com"))
            {
                return;
            }

            String urlMd5 = Util.Security.MD5Util.MD5(linkItem);

            if (existsUrls.Contains(urlMd5))
            {
                return;
            }

            if (requestedUrls.Contains(urlMd5))
            {
                return;
            }

            requestedUrls.Add(urlMd5);

            if (Uri.TryCreate(linkItem, UriKind.Absolute, out var tmpUri))
            {
                if (!tmpUri.IsAbsoluteUri)
                {
                    return;
                }
                else if (existedDomains.Contains(tmpUri.Host))
                {
                    return;
                }
                else
                {
                    existedDomains.Add(tmpUri.Host);
                }
            }

            //分别获取各个链接的内容
            var startTtime = DateTime.Now;
            WriteLog($"connecting: {linkItem}", Util.Log.LogType.Info);
            String linkContent = HttpHelper.GetHtml(linkItem, null, "get", String.Empty, out String _);
            var endTime = DateTime.Now;

            if (endTime.Subtract(startTtime).TotalSeconds > 10)
            {
                Util.Log.LogUtil.Write($"访问超时 {endTime.Subtract(startTtime).TotalSeconds}秒 {linkItem}", Util.Log.LogType.Fatal);
            }

            if (String.IsNullOrWhiteSpace(linkContent))
            {
                return;
            }

            if (!ValidPage(linkContent))
            {
                return;
            }

            try
            {
                okCount++;
                WriteLog($"找到一个目标链接: {linkItem}", Util.Log.LogType.Info);

                DBData.GetInstance(DBTable.url).Add(new urls()
                {
                    url = linkItem,
                    status = 0,
                });

                //深度为1就继续查找当前页面中的外链，再检查各个外链是否满足条件
                if (deep == 1)
                {
                    //搜索结果中的链接
                    var urls = GetUrls(linkContent, linkReg);

                    Uri baseUri = new Uri(linkItem);
                    //遍历所有搜索到的链接
                    foreach (String url in urls)
                    {
                        if (IsUrlValid(url))
                        {
                            if (Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out var newUri))
                            {
                                if (newUri.IsAbsoluteUri && baseUri.Host != newUri.Host)
                                {   //不是当前页面的外链
                                    DealLink(url, ref okCount, 2);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                WriteLog(e.Message, Util.Log.LogType.Error);
            }

        }

        /// <summary>
        /// 检测内容是否符合要求
        /// </summary>
        /// <param name="content">页面内容</param>
        /// <returns></returns>
        private static Boolean ValidPage(String content)
        {
            List<String> keywords = new List<String>()
            {
                "自拍","无码","欧美","自拍","無碼","歐美"
            };

            //必须包含至少以上3个关键字

            Int32 count = 0;
            foreach (String item in keywords)
            {
                if (content.Contains(item))
                {
                    count++;
                }
            }

            return count >= 3;
        }

        /// <summary>
        /// 获取内容中的链接地址
        /// </summary>
        /// <param name="content"></param>
        /// <param name="reg"></param>
        /// <returns></returns>
        private static List<String> GetUrls(String content, Regex reg)
        {
            List<String> urls = new List<String>();
            var matches = reg.Matches(content);
            foreach (Match item in matches)
            {
                urls.Add(item.Groups["url"].Value);
            }

            return urls;
        }

        /// <summary>
        /// 是否为有效的常规链接
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        private static Boolean IsUrlValid(String link)
        {
            //Int32 httpIndex = link.IndexOf("//");
            //String tmpType = httpIndex >= 0 ? link.Substring(0, httpIndex) + "//" : String.Empty;
            //if ((!String.IsNullOrWhiteSpace(tmpType) && !httpTypes.Any(a => a == tmpType))
            //    || link.StartsWith("/") || link.StartsWith("#") || link.StartsWith("javascript:") || link.StartsWith("tell:") || link.StartsWith("mailto:"))
            //{
            //    return false;
            //}
            //else
            //{
            //    return true;
            //}

            return link.StartsWith("http");
        }

        private static void WriteLog(String msg, Util.Log.LogType logType)
        {
            Console.WriteLine(msg);
            Util.Log.LogUtil.Write(msg, logType);
        }
    }
}
