using System;
using System.Collections.Generic;
using System.Linq;

namespace ResourceSpider.GetItems
{
    public class PageList
    {
        private readonly List<String> historyList = new List<String>();

        /// <summary>
        /// 一个站点，请求的最大次数，超过这个次数，仍没找到数据就退出
        /// </summary>
        private const Int32 maxRequestTimesPerSite = 40;

        /// <summary>
        /// 没取到任何数据的话，超过此次数就退出
        /// </summary>
        private static Int32 maxRequestTimesPerSiteWithoutData = 15;

        /// <summary>
        /// URL中包含以下字符时，直接退出
        /// </summary>
        private static readonly List<String> expectDomains = new List<String>(){"163.com","douban.com","baidu.com","zhihu.com","so.com","google.com","bing.com","sohu.com","sina.com","china","edu","org","qq.com","bbs","github.com","taobao.com","1688.com","jd.com","tmall.com","58.com","12306.cn","tianya.cn","cctv","alipay.com","gov","youdao.com","sogou.com","detail"
        };

        /// <summary>
        /// 对url进行拆分时用到的各个字符
        /// </summary>
        private static readonly Char[] urlSplitChars = new Char[] { '/', '-', '_', '.', '?', '=', '*' };

        public String SearchPageLink(String url, Int32 deep)
        {
            if (deep == 1)
            {
                maxRequestTimesPerSiteWithoutData = 0;
            }

            //如果URL中包含以上任意字符，则不再继续
            if (expectDomains.Any(a => url.Contains(a)))
            {
                return String.Empty;
            }

            if (!url.StartsWith("http"))
            {
                url = "http://" + url;
            }

            String md5 = Util.Security.MD5Util.MD5(url);
            if (historyList.Contains(md5))
            {
                return String.Empty;
            }
            else
            {
                historyList.Add(md5);
            }

            String host = Comm.GetUrlHost(url);

            //如果是被禁掉的无效站点，就不用再继续
            if (DbCenter.IsForbiddenDomain(host))
            {
                return String.Empty;
            }

            //如果已有该站点的列表数据，就不再继续
            //if (DbCenter.IsListFormatExists(host))
            //{
            //    return 0;
            //}

            Comm.WriteLog($"connecting: {url}", Util.Log.LogType.Info);
            String content = Comm.GetHtml(url, 5);
            if (String.IsNullOrWhiteSpace(content))
            {
                Comm.WriteLog("读取页面内容失败", Util.Log.LogType.Info);
                DbCenter.AddToForbiddenDomain(host);
                return String.Empty;
            }

            var urls = Comm.GetUrls(content, host);

            if (deep == 1)
            {   //首页不存在分页链接，所以直接再取各个子页面进行分析

                //当前页面上的URL，检测过的数量
                Int32 didCount = 0;
                Int32 okCount = 0;
                //检测过的无效链接
                List<String> hisFaileds = new List<String>();
                //检测过的有效链接
                List<String> hisSuccesss = new List<String>();
                //找到分页链接的地址和具体分页链接的对应关系
                Dictionary<String, String> successUrlFormatDic = new Dictionary<String, String>();
                foreach (String item in urls)
                {
                    //如果是同一个站点，则继续检测其他页面
                    String tmpHost = Comm.GetUrlHost(item);
                    if (tmpHost == host)
                    {
                        //如果已存在类似的无效链接，则不再检测
                        (String tt, _, _, _) = GetSameAsSomeUrls(item, hisFaileds);
                        if (!String.IsNullOrWhiteSpace(tt))
                        {
                            continue;
                        }


                        //如果已存在类似的有效链接，则直接添加，不用再检测
                        (String sucItem, Int32 startIndex, Int32 length, String endChar) = GetSameAsSomeUrls(item, hisSuccesss);
                        if (!String.IsNullOrWhiteSpace(sucItem))
                        {
                            if (successUrlFormatDic.ContainsKey(sucItem))
                            {
                                String formatUrl = successUrlFormatDic[sucItem];
                                //检测分页链接中是否包含了完整的分类的URL地址数据
                                String oldSstartChars = sucItem.Substring(0, startIndex + length);
                                if (formatUrl.Contains(oldSstartChars))
                                {
                                    //下面的3为分页url中的不同处的长度，一般为3
                                    Int32 newCharEndIndex = item.IndexOf(endChar, startIndex, length + 1, StringComparison.OrdinalIgnoreCase);
                                    if (newCharEndIndex != -1)
                                    {
                                        String newStartChars = item.Substring(0, newCharEndIndex);
                                        String newFormatUrl = formatUrl.Replace(oldSstartChars, newStartChars);

                                        SuccessHandler(host, ref okCount, hisSuccesss, successUrlFormatDic, item, newFormatUrl);
                                        continue;
                                    }
                                    else
                                    {

                                    }
                                }

                            }
                        }

                        String okFormatString = SearchPageLink(item, 2);

                        //失败时，加入失败队列
                        if (String.IsNullOrWhiteSpace(okFormatString))
                        {
                            hisFaileds.Add(item);
                        }
                        else
                        {
                            SuccessHandler(host, ref okCount, hisSuccesss, successUrlFormatDic, item, okFormatString);
                        }
                    }

                    didCount++;

                    //如果一直没检测到数据，则超过一定次数就退出
                    if (okCount == 0 && didCount > maxRequestTimesPerSiteWithoutData)
                    {
                        break;
                    }

                    //每个站点最多只检测次数
                    //if (didCount > maxRequestTimesPerSite)
                    //{
                    //    break;
                    //}
                }

                //没找到数据列表页面，则记录该站点，下次不用再检测了
                if (okCount == 0)
                {
                    DbCenter.AddToForbiddenDomain(host);
                }

                return String.Empty;
            }
            else
            {   //非首页再进行列表页面检查
                //倒序，防止前面的列表链接孔存在分页链接类似的结构
                urls.Reverse();
                (String pageUrl, Int32 pageCharIndex, Int32 pageCharLength) = GetPageUrl(urls);

                if (String.IsNullOrWhiteSpace(pageUrl))
                {
                    Comm.WriteLog("未检测到数据分页链接", Util.Log.LogType.Info);

                    return String.Empty;
                }

                String listUrlFormat = GetListUrlFormatString(pageUrl, pageCharIndex, pageCharLength);

                //如果已存在包含当前链接的前部分的数据，那么这个就不要了
                if (!IsFormatUrlOk(listUrlFormat, pageCharIndex))
                {
                    return String.Empty;
                }

                return listUrlFormat;
            }

        }

        private void SuccessHandler(String host, ref Int32 okCount, List<String> hisSuccesss, Dictionary<String, String> successUrlFormatDic, String item, String okFormatString)
        {
            //成功找到分页链接
            okCount++;
            hisSuccesss.Add(item);
            successUrlFormatDic.Add(item, okFormatString);

            Comm.WriteLog($"检测到分页链接：{okFormatString}", Util.Log.LogType.Info);

            //写入缓存
            ItemData.AddPageLink(okFormatString);

            //写入数据库
            DbCenter.SaveListFormat(host, okFormatString);
        }

        /// <summary>
        /// 检测目标URL是否和列表中的某个URL存在类似关系
        /// </summary>
        /// <param name="url">目标URL</param>
        /// <param name="urls">等检测列表</param>
        /// <returns>存在类似关系的URL，两个URL不同处的起始位置，不同处字符长度，两个URL不同处的结束位置的下一个字符</returns>
        private (String, Int32, Int32, String) GetSameAsSomeUrls(String url, List<String> urls)
        {
            //逆序检测，因为后进入的可能会长些
            for (Int32 i = urls.Count - 1; i >= 0; i--)
            {
                String item = urls[i];
                (Int32 startIndex, Int32 length, String nextChar) = IsUrlSameAs(item, url);
                if (startIndex != -1)
                {
                    return (item, startIndex, length, nextChar);
                }
            }

            return (String.Empty, -1, -1, "");
        }


        /// <summary>
        /// 比较两个url地址是否很相似（只有一个地方不一样）
        /// </summary>
        /// <param name="url1"></param>
        /// <param name="url2"></param>
        /// <returns>（返回值中所有值都为每一个字符串为基础）不同处的起始位置，不同处字符长度，不同处后面的第一个字符</returns>
        private (Int32 startIndex, Int32 length, String nextChar) IsUrlSameAs(String url1, String url2)
        {
            String[] a1 = url1.Split(urlSplitChars);
            String[] a2 = url2.Split(urlSplitChars);

            if (a1.Length != a2.Length)
            {
                return (-1, 0, "");
            }

            //不相同地方的数量
            Int32 notSameCount = 0;
            //不同处的组索引
            Int32 index = 0;

            for (Int32 i = 0; i < a1.Length; i++)
            {
                if (a1[i] != a2[i])
                {
                    index = i;
                    notSameCount++;
                }

                if (notSameCount > 1)
                {
                    return (-1, 0, "");
                }
            }
            //不同处前面的字符数量
            Int32 preCharCount = 0;
            for (Int32 i = 0; i < index; i++)
            {
                preCharCount += a1[i].Length;
            }

            //字符中不同处的起始位置
            Int32 startIndex = index + preCharCount;
            //不同处的结束位置
            Int32 endIndex = startIndex + a1[index].Length;

            return (startIndex, a1[index].Length, url1[endIndex].ToString());
        }

        private Boolean IsFormatUrlOk(String listUrlFormat, Int32 pageCharIndex)
        {
            if (listUrlFormat.Contains("detail"))
            {
                return false;
            }

            //如果已存在包含当前链接的前部分的数据，那么这个就不要了
            String tmpStart = listUrlFormat.Substring(0, pageCharIndex);

            if (ItemData.IsPartofSoMeOne(tmpStart))
            {
                return false;
            }

            //如果链接中包含中文则排队，防止搜索页面出现
            String de = System.Web.HttpUtility.UrlDecode(listUrlFormat);
            if (!Comm.IsBaseChars(de))
            {
                return false;
            }

            return true;
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

                            //页码必须为2才算
                            if (allPgeNumber == "2")
                            {
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
