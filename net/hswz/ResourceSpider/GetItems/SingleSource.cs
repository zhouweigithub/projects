using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Hswz.Common;
using Hswz.Model.Urls;

namespace ResourceSpider.GetItems
{
    public class SingleSource
    {
        /// <summary>
        /// 带detail字符的链接
        /// </summary>
        private const String detailLinkRegString = "<a[^>]*?href=[\"'](?<url>.*?\\d{4,}.*?)[\"'][^>]*? title=[\"'](?<title>[^>]*?)[\"'][^>]*?>";

        private static readonly Regex detailReg = new Regex(detailLinkRegString);

        public delegate void StatusNotify();
        /// <summary>
        /// 任务结束时的通知行为
        /// </summary>
        public event StatusNotify CompleteEvent;

        /// <summary>
        /// 获取列表的任务是否已经结束
        /// </summary>
        private Boolean isListTaskOver = false;


        public void SetListTaskStatus(ThreadStatus status)
        {
            isListTaskOver = status == ThreadStatus.Stoped;
        }

        public void Do()
        {
            Int32 threadCount = 5;
            Int32 successCount = 0;   //已结束任务的线程数

            for (Int32 i = 0; i < threadCount; i++)
            {
                try
                {
                    Task.Factory.StartNew(() =>
                    {
                        GetSourceItems();
                        Comm.WriteLog($"详细线程结束", Util.Log.LogType.Fatal);
                        Interlocked.Add(ref successCount, 1);
                    });
                    Comm.WriteLog("详情线程启动成功", Util.Log.LogType.Info);
                }
                catch (Exception e)
                {
                    Comm.WriteLog($"线程出现异常：{e}", Util.Log.LogType.Error);
                }
            }
            while (true)
            {
                if (successCount >= threadCount)
                {
                    Comm.WriteLog("获取详细数据任务结束", Util.Log.LogType.Info);

                    //需要等到获取list的任务结束才能算结束
                    CompleteEvent?.Invoke();

                    break;
                }
                else
                {
                    Thread.Sleep(1000 * 5);
                }
            }

        }

        /// <summary>
        /// 循环获取各页中的数据项
        /// </summary>
        /// <param name="urlFormatString"></param>
        /// <param name="host"></param>
        private void GetSourceItems()
        {
            while (true)
            {
                String link = ItemData.GetPageLink();
                if (!String.IsNullOrEmpty(link))
                {
                    Comm.WriteLog($"开始读取页面数据项：{link}", Util.Log.LogType.Info);
                    GetItemsLoopPage(link);
                }
                else
                {
                    if (isListTaskOver)
                    {
                        break;
                    }
                    else
                    {
                        Thread.Sleep(5 * 1000);
                    }
                }
            }
        }


        private void GetItemsLoopPage(String urlFormateString)
        {
            //记录已抓取过数据的日志
            if (!DbCenter.IsRequestedListExists(DateTime.Today, urlFormateString))
            {
                String host = Comm.GetUrlHost(urlFormateString);

                if (String.IsNullOrEmpty(host))
                {
                    return;
                }

                if (DbCenter.IsForbiddenDomain(host))
                {
                    return;
                }

                //记录获取列表数据失败的次数，超过3次直接退出
                Int32 failCount = 0;
                Int32 maxPage = WebConfigData.GetDetailType == "1" ? 999 : 3;
                //页码
                for (Int32 i = 1; i < maxPage; i++)
                {
                    //如果连续2页都没数据就直接退出
                    if (failCount >= 2)
                    {
                        break;
                    }

                    Console.WriteLine($"读取第 {i} 页");
                    String url = String.Format(urlFormateString, i);
                    String html = Comm.GetHtml(url, 5);


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
                            Comm.WriteLog($"检测到 {details.Count} 条数据", Util.Log.LogType.Debug);
                            Comm.WriteLog($"url: {url}", Util.Log.LogType.Debug);
                            foreach (var item in details)
                            {
                                if (!DbCenter.IsSourceItemExists(item.url))
                                {
                                    DbCenter.SaveSourceItem(item.url, host, item.title);
                                }
                            }
                        }
                        else
                        {
                            failCount++;
                        }
                    }

                    //如果当前页面中找不到下一页的链接，那么就退出
                    String nextPageUrl = String.Format(urlFormateString, i + 1);
                    String relaiveNextPageUrl = nextPageUrl.Replace(host, String.Empty);
                    if (!html.Contains(nextPageUrl) && !html.Contains(relaiveNextPageUrl))
                    {
                        break;
                    }
                }

                DbCenter.SaveRequestedList(DateTime.Today, urlFormateString);
            }
        }

        private List<resource_items> GetDetailInfos(String content, String host)
        {
            List<resource_items> urls = new List<resource_items>();
            var matches = detailReg.Matches(content);
            foreach (Match item in matches)
            {
                String url = item.Groups["url"].Value;
                String title = item.Groups["title"].Value;
                if (Comm.IsUrlValid(url))
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

    }
}
