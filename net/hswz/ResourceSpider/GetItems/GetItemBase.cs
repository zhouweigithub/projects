using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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
        private const String detailLinkRegString = "<a .*?href=[\"'](?<url>.*?detail.*?)[\"'].*?>.*?</a>";
        private static readonly Regex linkReg = new Regex(linkRegString);
        private static readonly Regex numberReg = new Regex(numberRegString);

        public void Do(String url)
        {
            String content = GetHtml(url);
            var urls = GetAllUrls(content);
            (String pageUrl, Int32 pageCharIndex, Int32 pageCharLength) = GetPageUrl(urls);

            if (String.IsNullOrWhiteSpace(pageUrl))
            {
                return;
            }

            String listUrlFormat = GetListUrlFormatString(pageUrl, pageCharIndex, pageCharLength);
            GetItemsLoopPage(listUrlFormat);
        }

        private String GetListUrlFormatString(String pageUrl, Int32 pageCharIndex, Int32 pageNumberCount)
        {
            String pre = pageUrl.Substring(0, pageCharIndex);
            return pre + "{0}" + pageUrl.Substring(pageCharIndex + pageNumberCount - 1);
        }


        /// <summary>
        /// 获取内容中的链接地址
        /// </summary>
        /// <param name="content"></param>
        /// <param name="reg"></param>
        /// <returns></returns>
        private List<String> GetUrls(String content, Regex reg)
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
        /// 获取分页链接信息
        /// </summary>
        /// <param name="urls"></param>
        /// <returns>分页链接完整URL/页码位置/页码所占字符长度</returns>
        public (String, Int32, Int32) GetPageUrl(List<String> urls)
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
        public (String, Int32) GetAllPageNumber(String url, Int32 pageCharIndex)
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
                }
            }

            return (result, minIndex);
        }

        public void GetItemsLoopPage(String urlFormateString)
        {
            //页码
            for (Int32 i = 1; i < 999; i++)
            {
                String html = GetHtml(String.Format(urlFormateString, i));
                if (String.IsNullOrWhiteSpace(html))
                {
                    break;
                }

                //如果取取到的数据项为0也退出
                var items = GetItems(html);
            }
        }

        public List<String> GetAllUrls(String content)
        {
            return GetUrls(content, linkReg);
        }

        private String GetHtml(String url)
        {
            return Hswz.Common.HttpHelper.GetHtml(url, null, "get", null, out _);
        }

        public List<String> GetItems(String content)
        {
            var urls = GetAllUrls(content);
            //移除不包含/符的链接
            urls.RemoveAll(a => !a.Contains("/"));
            //移除所有不包含数字的项
            urls.RemoveAll(a => !numberReg.IsMatch(a));

            return urls.Where(a => a.Contains("detail")).ToList();
        }

        public void SaveItem(Object o)
        {

        }
    }
}
