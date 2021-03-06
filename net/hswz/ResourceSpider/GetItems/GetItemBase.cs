using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Hswz.DAL;
using Hswz.Model.Urls;

namespace ResourceSpider.GetItems
{
    public class GetItemBase
    {
        /// <summary>
        /// 数字
        /// </summary>
        private const String numberRegString = "\\d+";
        /// <summary>
        /// 所有链接
        /// </summary>
        private const String linkRegString = "<a .*?href=[\"'](?<url>.*?)[\"'].*?>.*?</a>";
        /// <summary>
        /// 带detail字符的链接
        /// </summary>
        private const String detailLinkRegString = "<a .*?href=[\"'](?<url>.*?\\d{4,}.*?)[\"'].*? title=[\"'](?<title>.*?)[\"'].*?>.*?</a>";

        private static readonly Regex linkReg = new Regex(linkRegString);
        private static readonly Regex numberReg = new Regex(numberRegString);
        private static readonly Regex detailReg = new Regex(detailLinkRegString);

        public void Do()
        {
            var urls = GetDbUrls();
            if (urls != null)
            {
                foreach (var item in urls)
                {
                    Do(item.url);
                }
            }
        }

        private void Do(String url)
        {
            if (!url.StartsWith("http"))
            {
                url = "http://" + url;
            }

            Uri uri = new Uri(url);
            String host = uri.Scheme + "://" + uri.Host;

            WriteLog($"connecting: {host}", Util.Log.LogType.Info);
            String content = GetHtml(host);
            if (String.IsNullOrWhiteSpace(content))
            {
                WriteLog("读取页面内容失败", Util.Log.LogType.Info);
                return;
            }

            var urls = GetAllUrls(content, linkReg, host);
            (String pageUrl, Int32 pageCharIndex, Int32 pageCharLength) = GetPageUrl(urls);

            if (String.IsNullOrWhiteSpace(pageUrl))
            {
                WriteLog("未检测到数据分页链接", Util.Log.LogType.Info);
                return;
            }

            String listUrlFormat = GetListUrlFormatString(pageUrl, pageCharIndex, pageCharLength);
            GetItemsLoopPage(listUrlFormat, host);
        }

        private IList<urls> GetDbUrls()
        {
            return DBData.GetInstance(DBTable.url).GetList<urls>();
        }

        private String GetListUrlFormatString(String pageUrl, Int32 pageCharIndex, Int32 pageNumberCount)
        {
            String pre = pageUrl.Substring(0, pageCharIndex);
            String next = pageCharIndex == pageUrl.Length - 1 ? String.Empty : pageUrl.Substring(pageCharIndex + 1);
            return pre + "{0}" + next;
        }


        /// <summary>
        /// 获取内容中的链接地址
        /// </summary>
        /// <param name="content"></param>
        /// <param name="reg"></param>
        /// <returns></returns>
        private List<String> GetUrls(String content, Regex reg, String host)
        {
            List<String> urls = new List<String>();
            var matches = reg.Matches(content);
            foreach (Match item in matches)
            {
                String url = item.Groups["url"].Value;
                if (IsUrlValid(url))
                {
                    if (!url.StartsWith("http"))
                    {
                        url = host + url;
                    }

                    urls.Add(url);
                }
            }

            return urls;
        }


        private List<resource_items> GetDetailInfos(String content, String host)
        {
            List<resource_items> urls = new List<resource_items>();
            var matches = detailReg.Matches(content);
            foreach (Match item in matches)
            {
                String url = item.Groups["url"].Value;
                String title = item.Groups["title"].Value;
                if (IsUrlValid(url))
                {
                    if (!url.StartsWith("http"))
                    {
                        url = host + url;
                    }

                    urls.Add(new resource_items()
                    {
                        url = url,
                        title = title,
                        domain = host
                    });
                }
            }

            return urls;
        }


        private Boolean IsUrlValid(String url)
        {
            return !url.StartsWith("javascript:") && !url.StartsWith("#") && url != "/";
        }

        /// <summary>
        /// 获取分页链接信息
        /// </summary>
        /// <param name="urls"></param>
        /// <returns>分页链接完整URL/页码位置/页码所占字符长度</returns>
        private (String, Int32, Int32) GetPageUrl(List<String> urls)
        {
            //分页链接应该有以下特征
            //1 同时存在只有一个数字不同的，同时该数字部分连续的多个连续url地址
            //2 页码数字不应该太长，并且不与其他字母靠在一起

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

                            //与页码挨在一起的数字串
                            (String allPgeNumber, Int32 startIndex) = GetAllPageNumber(urlItem, i);
                            if (allPgeNumber.Length <= 3)
                            {   //页码数字小于等于3个数字才算是真正的页码
                                return (urlItem, startIndex, allPgeNumber.Length);
                            }
                        }
                    }
                }
            }

            return (null, -1, 0);
        }

        /// <summary>
        /// 获取页码及附近并列的所有数字字符串
        /// </summary>
        /// <param name="url"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        private (String, Int32) GetAllPageNumber(String url, Int32 pageCharIndex)
        {
            //查找该页码数字紧邻的所有数字

            //数字位置的起始位置
            Int32 minIndex = pageCharIndex;

            //向前找
            String result = url[pageCharIndex].ToString();
            if (pageCharIndex > 0)
            {
                for (Int32 i = pageCharIndex - 1; i >= 0; i--)
                {
                    Char charItem = url[i];
                    if (charItem >= 48 && charItem <= 57)
                    {
                        //如果前面的是数字，就补到前面
                        result = charItem + result;
                        minIndex = i;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            //向后找
            if (pageCharIndex < url.Length - 1)
            {
                for (Int32 i = pageCharIndex + 1; i < url.Length; i++)
                {
                    Char charItem = url[i];
                    if (charItem >= 48 && charItem <= 57)
                    {
                        //如果前面的是数字，就补到前面
                        result += charItem;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return (result, minIndex);
        }

        private void GetItemsLoopPage(String urlFormateString, String host)
        {
            //记录获取列表数据失败的次数，超过3次直接退出
            Int32 failCount = 0;
            //页码
            for (Int32 i = 1; i < 999; i++)
            {
                if (failCount >= 3)
                {
                    break;
                }

                Console.WriteLine($"读取第 {i} 页");
                String url = String.Format(urlFormateString, i);
                String html = GetHtml(url);


                if (String.IsNullOrWhiteSpace(html))
                {
                    failCount++;
                }
                else
                {
                    //如果取取到的数据项为0也退出
                    var details = GetDetailInfos(html, host);

                    if (details.Count > 0)
                    {
                        failCount = 0;
                        foreach (var item in details)
                        {
                            SaveItem(item.url, host, item.title);
                        }
                    }
                    else
                    {
                        failCount++;
                    }
                }
            }
        }

        public List<String> GetAllUrls(String content, Regex regx, String host)
        {
            return GetUrls(content, regx, host);
        }

        private String GetHtml(String url)
        {
            return Hswz.Common.HttpHelper.GetHtml(url, null, "get", null, out _, 5);
        }

        private List<String> GetItems(String content, String host)
        {
            var urls = GetAllUrls(content, detailReg, host);
            //移除不包含/符的链接
            urls.RemoveAll(a => !a.Contains("/"));
            //移除所有不包含数字的项
            urls.RemoveAll(a => !numberReg.IsMatch(a));

            return urls.Where(a => a.Contains("detail")).ToList();
        }

        private void SaveItem(String url, String domain, String title)
        {
            DBData.GetInstance(DBTable.resource_items).Add(new resource_items()
            {
                url = url,
                domain = domain,
                title = title
            });
        }

        private void WriteLog(String msg, Util.Log.LogType logType)
        {
            Console.WriteLine(msg);
            Util.Log.LogUtil.Write(msg, logType);
        }

    }
}
