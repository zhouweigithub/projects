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
            //100页
            String nextUrl = "https://cn.bing.com/search?q=%E5%81%B7%E6%8B%8D+%E5%90%8E%E5%85%A5+%E6%AC%A7%E7%BE%8E&go=%E6%90%9C%E7%B4%A2&qs=ds&form=QBRE";

            do
            {
                //Console.WriteLine($"开始检索第 {i} 页");

                //Int32 page = i == 1 ? 1 : i == 2 ? 3 : 10 * (i - 2) + 3;
                //String searchUrl = String.Format(urlFormat, page);
                //nextUrl= HttpUtility.UrlDecode(nextUrl);

                if (!nextUrl.StartsWith("http"))
                {
                    nextUrl = "https://cn.bing.com" + nextUrl;
                }

                nextUrl = nextUrl.Replace("amp;", String.Empty);
                Console.WriteLine();
                Console.WriteLine(nextUrl);
                String searchContent = HttpHelper.GetHtml(nextUrl, null, "get", String.Empty, out String _);

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
                            Console.WriteLine($"找到一个目标链接: {linkItem}");

                            DBData.GetInstance(DBTable.tmp_searched_domains).Add(new tmp_searched_domains()
                            {
                                domain = linkItem,
                                status = 0
                            });
                        }
                        catch (Exception e)
                        {
                            Util.Log.LogUtil.Write(e.Message, Util.Log.LogType.Error);
                        }
                    }

                }

                nextUrl = String.Empty;

                //检索下一页地址
                var tmpUrls = GetUrls(searchContent, nextUrlReg);
                if (tmpUrls.Count > 0)
                {
                    nextUrl = tmpUrls[0];
                }

            } while (!String.IsNullOrWhiteSpace(nextUrl));
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
