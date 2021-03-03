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
        private const String urlRegString = "b_algo.*?<a .*?href=[\"'](?<url>.*?)[\"'].*?>.*?</a>";
        private const String nextUrlRegString = "<a [^>]*?sb_pagN_bp.*?href=\"(?<url>.*?)\".*?>.*?</a>";
        private static readonly Regex urlReg = new Regex(urlRegString);
        private static readonly Regex nextUrlReg = new Regex(nextUrlRegString);
        private const String userAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4324.190 Safari/537.36";

        private const String urlFormat = "https://cn.bing.com/search?q=偷拍+后入+欧美&qs=n&sc=0-8&sk=&cvid=11B6&first={0}";

        private static readonly List<String> requestedUrls = new List<String>();

        /// <summary>
        /// 上次访问的url
        /// </summary>
        private static String lastRequestedUrl;


        static SearchDomainBLL3()
        {
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
            Console.WriteLine("start");

            Do();

            Console.WriteLine("end");
        }

        private static void Do()
        {
            String urlFormat = "/search?q={0}&qs=n&form=QBLH&sp=-1&pq={0}&sc=0-6&sk=&cvid=61EC7E49819341299A065C42F8AC67D0";

            if (String.IsNullOrWhiteSpace(WebConfigData.SearchKeyWords))
            {
                Console.WriteLine("未配置待检索关键字，自动退出");
                return;
            }

            String[] keywordGroup = WebConfigData.SearchKeyWords.Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (String keyword in keywordGroup)
            {
                //找到有效资源的数量
                Int32 okCount = 0;
                String start = $"开始检索关键字：{keyword}";
                Util.Log.LogUtil.Write(start, Util.Log.LogType.Debug);
                Console.WriteLine(start);
                String url = String.Format(urlFormat, keyword);
                do
                {
                    url = url.Replace("&amp;", "&");

                    if (url == lastRequestedUrl)
                    {
                        break;
                    }
                    else
                    {
                        lastRequestedUrl = url;
                    }

                    Console.WriteLine();
                    Console.WriteLine(url);
                    String searchContent = HttpHelper.GetHtml("https://cn.bing.com" + url, null, "get", String.Empty, out String _);

                    if (!String.IsNullOrWhiteSpace(searchContent))
                    {
                        //搜索结果中的链接
                        var urls = GetUrls(searchContent, urlReg);

                        //遍历所有搜索到的链接
                        foreach (String linkItem in urls)
                        {
                            if (requestedUrls.Contains(linkItem))
                            {
                                continue;
                            }

                            if (linkItem.Contains("google.com"))
                            {
                                continue;
                            }

                            requestedUrls.Add(linkItem);

                            //分别获取各个链接的内容
                            var startTtime = DateTime.Now;
                            Console.WriteLine($"connecting: {linkItem}");
                            String linkContent = HttpHelper.GetHtml(linkItem, null, "get", String.Empty, out String _);
                            var endTime = DateTime.Now;

                            if (endTime.Subtract(startTtime).TotalSeconds > 10)
                            {
                                Util.Log.LogUtil.Write($"访问超时 {endTime.Subtract(startTtime).TotalSeconds}秒 {linkItem}", Util.Log.LogType.Fatal);
                            }

                            if (String.IsNullOrWhiteSpace(linkContent))
                            {
                                continue;
                            }

                            if (!ValidPage(linkContent))
                            {
                                continue;
                            }

                            try
                            {
                                okCount++;

                                Console.WriteLine($"找到一个目标链接: {linkItem}");

                                DBData.GetInstance(DBTable.url).Add(new urls()
                                {
                                    url = linkItem,
                                    status = 0,
                                });

                                //查找当前页面中的外链，再检查各个外链是否正常
                            }
                            catch (Exception e)
                            {
                                Util.Log.LogUtil.Write(e.Message, Util.Log.LogType.Error);
                            }
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
                Console.WriteLine(msg);
                Util.Log.LogUtil.Write(msg, Util.Log.LogType.Info);
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

    }
}
