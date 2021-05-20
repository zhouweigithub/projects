using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Hswz.DAL;
using Hswz.Model.Urls;

namespace ResourceSpider
{
    public class SearchDomainBLL
    {
        private const String urlRegString = "<a .*?href=[\"'](?<url>.*?)[\"'].*?>.*?</a>";
        private static readonly Regex urlreg = new Regex(urlRegString);
        private const String userAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4324.190 Safari/537.36";

        /// <summary>
        /// 已经检索过的域名集
        /// </summary>
        private static readonly List<String> tmp_searched_domains = new List<String>();
        /// <summary>
        /// 已经检索过的域名的主机部分
        /// </summary>
        private static readonly List<String> tmp_searched_domains_host = new List<String>();
        /// <summary>
        /// 所有发现的域名集
        /// </summary>
        private static readonly List<String> tmp_all_domains = new List<String>();
        /// <summary>
        /// 所有发现的域名的链接集
        /// </summary>
        private static readonly List<String> tmp_all_domain_urls = new List<String>();

        private static readonly Object tmp_searched_domains_lockobject = new Object();
        private static readonly Object tmp_searched_domains_host_lockobject = new Object();
        private static readonly Object tmp_all_domains_lockobject = new Object();
        private static readonly Object tmp_all_domain_urls_lockobject = new Object();


        static SearchDomainBLL()
        {
            //从数据库读取所有已经检索过的域名
            var dms = DBData.GetInstance(DBTable.tmp_searched_domains).GetList<tmp_searched_domains>();

            //初始化所有已发现的域名集
            foreach (var item in dms)
            {
                tmp_all_domains.Add(item.domain);
            }

            //初始化所有已检索过的域名
            foreach (var item in dms.Where(a => a.status == 1))
            {
                tmp_searched_domains.Add(item.domain);
                String host = GetHostInfo(item.domain);

                //主机名部分
                if (!tmp_searched_domains_host.Contains(host))
                {
                    tmp_searched_domains_host.Add(host);
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
            Util.Log.LogUtil.Write("start", Util.Log.LogType.Debug);

            //获取所有尚未检索的域名集
            List<String> unSearchedDomains = tmp_all_domains.Except(tmp_searched_domains).ToList();

            if (unSearchedDomains == null || unSearchedDomains.Count == 0)
            {
                String url = "https://www.so.com/s?ie=utf-8&fr=so.com&src=home_so.com&q=%E8%87%AA%E6%8B%8D+%E6%AC%A7%E7%BE%8E";
                Uri curUri = new Uri(url);
                String curDomainName = curUri.Scheme + "://" + curUri.Host;
                Int32 count = DBData.GetInstance(DBTable.tmp_searched_domains).GetCount($"domain='{curDomainName}'");

                if (count == 0)
                {
                    DBData.GetInstance(DBTable.tmp_searched_domains).Add(new tmp_searched_domains() { domain = curDomainName, status = 0 });
                }

                Do(url);
            }
            else
            {
                //启用多个线运行
                Hswz.Common.ThreadHelper.Threading(unSearchedDomains, 5, 10, (item) =>
                {
                    Do(item);
                });
            }

            Util.Log.LogUtil.Write("end", Util.Log.LogType.Debug);
        }

        /// <summary>
        /// 检索主域名
        /// </summary>
        /// <param name="link"></param>
        private static void Do(String link)
        {
            if (!link.StartsWith("http://") && !link.StartsWith("https://"))
            {
                link = "http://" + link;
            }

            String content = GetPageContent(link);
            if (!String.IsNullOrWhiteSpace(content))
            {
                Uri curUri = new Uri(link);
                String curDomainName = curUri.Scheme + "://" + curUri.Host;

                Boolean isValidPage = ValidPage(content);
                if (isValidPage)
                {
                    try
                    {
                        DBData.GetInstance(DBTable.domain).Add(new domains() { domain = curDomainName });
                    }
                    catch (Exception e)
                    {
                        Util.Log.LogUtil.Write(e.Message, Util.Log.LogType.Error);
                    }
                }

                //改变已检索过域名的状态
                if (!tmp_searched_domains.Contains(curDomainName))
                {
                    lock (tmp_searched_domains_lockobject)
                    {
                        tmp_searched_domains.Add(curDomainName);

                        String tmpHost = GetHostInfo(curDomainName);
                        if (!tmp_searched_domains_host.Contains(tmpHost))
                        {
                            tmp_searched_domains_host.Add(tmpHost);
                        }

                        //更改数据库表中状态
                        try
                        {
                            DBData.GetInstance(DBTable.tmp_searched_domains).UpdateByKey(new List<String>() { "status" }, new List<Object>() { 1 }, curDomainName);
                        }
                        catch (Exception e)
                        {
                            Util.Log.LogUtil.Write(e.Message, Util.Log.LogType.Error);
                        }
                    }
                }

                //当前域名中获取到的链接
                var urls = GetUrls(content);
                var domainLinks = GetDomainsLinks(urls);

                //先全部写入数据库
                foreach (String domainLinkItem in domainLinks)
                {
                    Uri uri = new Uri(domainLinkItem);
                    String tmpName = uri.Scheme + "://" + uri.Host;
                    //记录尚未检索过的域名
                    if (!tmp_all_domains.Contains(tmpName))
                    {
                        try
                        {
                            lock (tmp_all_domains_lockobject)
                            {
                                tmp_all_domains.Add(tmpName);
                                DBData.GetInstance(DBTable.tmp_searched_domains).Add(new tmp_searched_domains() { domain = tmpName, status = 0 });
                            }
                        }
                        catch (Exception e)
                        {
                            Util.Log.LogUtil.Write(e.Message, Util.Log.LogType.Error);
                        }
                    }

                }

                //逐个检索
                foreach (String domainItem in domainLinks)
                {
                    if (!String.IsNullOrWhiteSpace(domainItem) && !tmp_searched_domains.Contains(domainItem) && !domainItem.Contains("google.com")
                        && !tmp_searched_domains_host.Any(a => domainItem.Contains(a)))
                    {
                        Do(domainItem);
                    }
                }
            }
        }

        /// <summary>
        /// 获取网页内容
        /// </summary>
        /// <param name="url">网页地址</param>
        /// <returns></returns>
        private static String GetPageContent(String url)
        {
            try
            {
                return Util.Web.WebUtil.GetWebData(url, null, Util.Web.DataCompress.NotCompress, userAgent: userAgent, timeout: 20 * 1000);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write(e.Message, Util.Log.LogType.Error);
            }
            return String.Empty;
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
        /// 获取内容中的所有链接地址
        /// </summary>
        /// <param name="content">内容</param>
        /// <returns></returns>
        private static List<String> GetUrls(String content)
        {
            List<String> urls = new List<String>();
            var matches = urlreg.Matches(content);
            foreach (Match item in matches)
            {
                urls.Add(item.Groups["url"].Value);
            }

            return urls;
        }

        /// <summary>
        /// 获取网页链接，每个域名只获取一个链接
        /// </summary>
        /// <param name="urls">地址列表</param>
        /// <returns></returns>
        private static List<String> GetDomainsLinks(List<String> urls)
        {
            //所有域名列表，用以确保每个域名只有一个链接
            List<String> domains = new List<String>();
            //所有链接列表
            List<String> result = new List<String>();
            foreach (String item in urls)
            {
                if (item.StartsWith("/") || item.StartsWith("#") || item.StartsWith("javascript:") || tmp_all_domain_urls.Contains(item))
                {
                    continue;
                }

                if (Uri.TryCreate(item, UriKind.RelativeOrAbsolute, out var uri))
                {
                    if (uri.IsAbsoluteUri)
                    {
                        String tmpDomainName = uri.Scheme + "://" + uri.Host;
                        if (!domains.Contains(tmpDomainName))
                        {
                            domains.Add(tmpDomainName);
                            result.Add(item);
                            lock (tmp_all_domain_urls_lockobject)
                            {
                                tmp_all_domain_urls.Add(item);
                            }
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 查找链接中的页码链接
        /// </summary>
        /// <param name="urls"></param>
        /// <returns>返回页码链接，和页码数字的位置</returns>
        private static (String, Int32) GetPageUrl(List<String> urls)
        {
            //前提：当页面出现分页链接时，必然会出现多个页码链接，这些页面链接只有页码字符相差1，其余部分完全相同
            //找出链接中的数字然后逐个替换成+1的数字，然后检查所有链接中是否有替换后相同的链接，如果有，则为分页链接

            foreach (String urlItem in urls)
            {
                //找到页码链接的数量，必须找到连续3个以上才行
                for (Int32 i = 0; i < urlItem.Length; i++)
                {
                    Char charItem = urlItem[i];
                    if (charItem >= 48 && charItem <= 57)
                    {   //数字字符
                        //找到的页码数量
                        Int32 mustCount = 3;
                        Int32 successCount = 0;
                        //找到mustCount个连续的才算是页码链接
                        for (Int32 t = 1; t <= mustCount; t++)
                        {
                            Int32 nextChar = charItem + t;
                            //数字替换成+1后的字符串
                            String newString = urlItem.Substring(0, i) + (Char)nextChar + urlItem.Substring(i + 1);

                            if (!urls.Any(a => a == newString))
                            {
                                break;
                            }
                            else
                            {
                                successCount++;
                            }
                        }

                        if (successCount == mustCount)
                        {
                            //找到了页码链接
                            return (urlItem, i);
                        }
                    }
                }
            }

            return (null, -1);
        }

        private static String GetHostInfo(String domain)
        {
            return domain.Replace("http://", String.Empty).Replace("http://", String.Empty).Replace("www", String.Empty);
        }
    }
}
