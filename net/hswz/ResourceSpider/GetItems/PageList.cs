using System;
using System.Collections.Generic;
using System.Linq;

namespace ResourceSpider.GetItems
{
    public class PageList
    {
        private readonly List<String> historyList = new List<String>();

        /// <summary>
        /// URL中包含以下字符时，直接退出
        /// </summary>
        private static readonly List<String> expectDomains = new List<String>(){"163.com","douban.com","baidu.com","zhihu.com","so.com","google.com","bing.com","sohu.com","sina.com","china","edu","org","qq.com","bbs","github.com","taobao.com","1688.com","jd.com","tmall.com","58.com","12306.cn","tianya.cn","cctv","alipay.com","gov","youdao.com","sogou.com"
        };

        public void SearchPageLink(String url, Int32 deep)
        {
            //如果URL中包含以上任意字符，则不再继续
            if (expectDomains.Any(a => url.Contains(a)))
            {
                return;
            }

            if (!url.StartsWith("http"))
            {
                url = "http://" + url;
            }

            String md5 = Util.Security.MD5Util.MD5(url);
            if (historyList.Contains(md5))
            {
                return;
            }
            else
            {
                historyList.Add(md5);
            }

            Comm.WriteLog($"connecting: {url}", Util.Log.LogType.Info);
            String content = Comm.GetHtml(url, 5);
            if (String.IsNullOrWhiteSpace(content))
            {
                Comm.WriteLog("读取页面内容失败", Util.Log.LogType.Info);
                return;
            }

            String host = Comm.GetUrlHost(url);
            var urls = Comm.GetUrls(content, host);

            if (deep == 1)
            {   //首页不存在分页链接，所以直接再取各个子页面进行分析
                foreach (String item in urls)
                {
                    //如果是同一个站点，则继续检测其他页面
                    String tmpHost = Comm.GetUrlHost(item);
                    if (tmpHost == host)
                    {
                        SearchPageLink(item, 2);
                    }
                }
            }
            else
            {   //非首页再进行列表页面检查
                //倒序，防止前面的列表链接孔存在分页链接类似的结构
                urls.Reverse();
                (String pageUrl, Int32 pageCharIndex, Int32 pageCharLength) = GetPageUrl(urls);

                if (String.IsNullOrWhiteSpace(pageUrl))
                {
                    Comm.WriteLog("未检测到数据分页链接", Util.Log.LogType.Info);

                    return;
                }

                String listUrlFormat = GetListUrlFormatString(pageUrl, pageCharIndex, pageCharLength);
                Comm.WriteLog($"检测到分页链接：{listUrlFormat}", Util.Log.LogType.Info);

                //写入缓存
                ItemData.AddPageLink(listUrlFormat);

                //写入数据库
                DbCenter.SaveListFormat(host, listUrlFormat);
            }

        }

        /// <summary>
        /// 提取分页链接信息
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

        private String GetListUrlFormatString(String pageUrl, Int32 pageCharIndex, Int32 pageNumberCount)
        {
            String pre = pageUrl.Substring(0, pageCharIndex);
            String next = pageCharIndex == pageUrl.Length - 1 ? String.Empty : pageUrl.Substring(pageCharIndex + pageNumberCount);
            return pre + "{0}" + next;
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

    }
}
