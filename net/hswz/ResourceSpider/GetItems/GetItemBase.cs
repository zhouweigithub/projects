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
        /// 带detail字符的链接
        /// </summary>
        private const String detailLinkRegString = "<a .*?href=[\"'](?<url>.*?\\d{4,}.*?)[\"'].*? title=[\"'](?<title>.*?)[\"'].*?>.*?</a>";

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
            String host = Comm.GetUrlHost(url);
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





        //private List<String> GetItems(String content, String host)
        //{
        //    var urls = Comm.GetUrls(content, detailReg, host);
        //    //移除不包含/符的链接
        //    urls.RemoveAll(a => !a.Contains("/"));
        //    //移除所有不包含数字的项
        //    urls.RemoveAll(a => !numberReg.IsMatch(a));

        //    return urls.Where(a => a.Contains("detail")).ToList();
        //}



    }
}
