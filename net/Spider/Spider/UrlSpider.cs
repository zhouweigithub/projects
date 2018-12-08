using Spider.Common;
using Spider.DAL;
using Spider.RegexContent;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Spider
{
    public class UrlSpider
    {
        private static readonly Regex urlRegex = new Regex(CommonDatas.RegUrlString);
        private static readonly Regex titleRegex = new Regex(CommonDatas.RegTitleString);
        private static readonly Regex linkRegex = new Regex(CommonDatas.RegLinkString);
        private static readonly Regex numberRegex = new Regex(CommonDatas.RegNumber);


        /// <summary>
        /// 链接关键字，包含才检索该链接
        /// </summary>
        private static readonly string[] linkKeyWordsList;

        /// <summary>
        /// 页面标题关键字，包含则视为有效内容，可获取文章标题和内容
        /// </summary>
        private static readonly string[] titleKeyWordsList;

        /// <summary>
        /// 页面标题中不能出现的关键字，包含则视为无效内容
        /// </summary>
        private static readonly string[] exceptKeyWordsList;

        /// <summary>
        /// 起用线程数
        /// </summary>
        private static readonly int threadCount;

        /// <summary>
        /// 是否只在站内检索
        /// </summary>
        private static readonly bool isOnlyThisSite;

        /// <summary>
        /// 站点字符集
        /// </summary>
        private static readonly ConcurrentDictionary<string, string> charsetDic = new ConcurrentDictionary<string, string>();


        public static void Start()
        {
            Go();
        }

        static UrlSpider()
        {
            isOnlyThisSite = WebConfigData.IsOnlyThisSite;
            threadCount = WebConfigData.ThreadCount > 0 ? WebConfigData.ThreadCount : 1;
            linkKeyWordsList = WebConfigData.LinkKeyWordsList.Split(new string[] { ",", "，" }, StringSplitOptions.RemoveEmptyEntries);
            titleKeyWordsList = WebConfigData.TitleKeyWordsList.Split(new string[] { ",", "，" }, StringSplitOptions.RemoveEmptyEntries);
            exceptKeyWordsList = WebConfigData.ExceptKeyWordsList.Split(new string[] { ",", "，" }, StringSplitOptions.RemoveEmptyEntries);
        }

        private static void Go()
        {
            GetUrls("https://www.diyifanwen.com/zuowen/gaozhongzuowen/", isOnlyThisSite);
            GetUrls("http://www.zuowen.com/gaokaozw/lngkmf/", isOnlyThisSite);
            GetUrls("http://www.gaokao.com/gkzw/gkmfzw/", isOnlyThisSite);

            //GetUrls("https://www.baidu.com/s?ie=utf-8&f=8&rsv_bp=1&rsv_idx=1&tn=baidu&wd=%E9%AB%98%E8%80%83%E6%BB%A1%E5%88%86%E4%BD%9C%E6%96%87", isOnlyThisSite);

            for (int i = 0; i < threadCount; i++)
            {
                Task.Factory.StartNew(() => GetContents(isOnlyThisSite));
            }
        }

        private static string GetCharset(string domain)
        {
            string result = string.Empty;
            if (charsetDic.ContainsKey(domain))
                result = charsetDic[domain];
            else
            {
                result = HtmlUtil.GetEncoding(domain);
                charsetDic[domain] = result;
            }

            return result;
        }

        /// <summary>
        /// 读取网页内容
        /// </summary>
        /// <param name="url">起始地址</param>
        /// <param name="isOnlyThisSite">是否只在本站内检索</param>
        private static void GetUrls(string url, bool isOnlyThisSite)
        {
            CommonDatas.DealedUrlQueue.Enqueue(url);

            Uri uri = ConvertToUri(url);
            if (uri == null)
                return;

            string host = uri.Host;
            string charset = GetCharset(uri.Scheme + "://" + host);
            string html = HtmlUtil.GetHtml(url, Encoding.GetEncoding(charset));

            if (string.IsNullOrWhiteSpace(html))
                return;

            Match titleMatch = titleRegex.Match(html);
            if (titleMatch.Success)
            {   //网页标题
                string title = titleMatch.Groups["Title"].Value;
                if (titleKeyWordsList.Length > 0)
                {
                    if (titleKeyWordsList.Count(a => title.Contains(a)) > 0 &&
                        (exceptKeyWordsList.Length == 0 || exceptKeyWordsList.Count(a => title.Contains(a)) == 0))      //不能包含被排除的关键字
                        GetContent(host, url, html);
                }
                else
                {
                    if ((exceptKeyWordsList.Length == 0 || exceptKeyWordsList.Count(a => title.Contains(a)) == 0))      //不能包含被排除的关键字)
                        GetContent(host, url, html);
                }
            }

            foreach (Match item in linkRegex.Matches(html))
            {
                string linkUrl = item.Groups["url"].Value;
                string linkContent = item.Groups["text"].Value;

                linkUrl = HtmlUtil.ValidUrl(linkUrl, uri.Scheme);
                if (string.IsNullOrWhiteSpace(linkUrl))     //如果为非有效URL，则跳过
                    continue;

                Uri mtpUri = ConvertToUri(linkUrl);
                if (mtpUri == null)     //如果为非有效URL，则跳过
                    continue;

                if (!isOnlyThisSite || (isOnlyThisSite && host == mtpUri.Host))
                {
                    if (((linkKeyWordsList.Length > 0 && linkKeyWordsList.Count(a => linkContent.Contains(a)) > 0) || numberRegex.IsMatch(linkContent)) &&
                        !CommonDatas.DealedUrlQueue.Contains(linkUrl) && !CommonDatas.UrlQueue.Contains(linkUrl))
                    {
                        CommonDatas.UrlQueue.Enqueue(linkUrl);
                    }
                }
            }
        }

        private static void GetContent(string domain, string url, string html)
        {
            string title = string.Empty;
            string content = string.Empty;

            RegexBase reg = WebRegex.GetSiteRegex(domain);

            if (reg != null)
            {
                //进入已定好规则的站点
                Regex regTitle = new Regex(reg.TitleRegex);
                Match titleMatch = regTitle.Match(html);

                if (titleMatch.Success)
                {
                    //文章标题
                    title = titleMatch.Groups["title"].Value;

                    Regex regContent = new Regex(reg.ContentRegex);
                    Match contentMatch = regContent.Match(html);
                    if (contentMatch.Success)
                    {
                        content = contentMatch.Groups["content"].Value;
                        ArticalDAL.InsertData(title.Trim(), content.Trim(), domain);
                    }
                }
            }
            else
            {
                //进入了未定规则的网站
                Match titleMatch = titleRegex.Match(html);
                if (titleMatch.Success)
                {   //网页标题
                    title = titleMatch.Groups["Title"].Value;
                    content = url;
                    ArticalDAL.InsertData(title.Trim(), content.Trim(), domain);
                }

            }
        }

        /// <summary>
        /// 从队列中获取地址，然后读取内容
        /// </summary>
        /// <param name="isOnlyThisSite"></param>
        private static void GetContents(bool isOnlyThisSite)
        {
            Console.WriteLine(string.Format("线程 {0} 启动", System.Threading.Thread.CurrentThread.ManagedThreadId));
            while (CommonDatas.UrlQueue.Count > 0)
            {
                if (CommonDatas.UrlQueue.TryDequeue(out string url))
                {
                    if (!CommonDatas.DealedUrlQueue.Contains(url))
                    {
                        string newUrl = HtmlUtil.ValidUrl(url);
                        if (!string.IsNullOrWhiteSpace(newUrl))
                            GetUrls(newUrl, isOnlyThisSite);
                    }
                }
            }
            Console.WriteLine(string.Format("线程 {0} 退出", System.Threading.Thread.CurrentThread.ManagedThreadId));
        }

        /// <summary>
        /// 判定该URL是否指向特定的文件类型
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static bool IsValidUrl(string url)
        {
            bool result = true;

            try
            {
                url = url.Trim().ToLower();     //url直接转换为小写
                string type = url.Substring(url.LastIndexOf("."));
                result = !CommonDatas.ExpceptTypes.Contains(type);

                if (!result)
                    return result;

                foreach (var item in CommonDatas.ExpceptTypes)
                {
                    result = result && !url.Contains(item + "?");
                    if (!result)
                        break;
                }
            }
            catch
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// 将url字符串转换为Uri对象（若url为非有效地址，则返回null）
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static Uri ConvertToUri(string url)
        {
            try
            {
                return new Uri(url);
            }
            catch
            {
                LogUtil.Write($"转换Uri失败：{url}", LogType.Error);
                return null;
            }
        }

    }
}
