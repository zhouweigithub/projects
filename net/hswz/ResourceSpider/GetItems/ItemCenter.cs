using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Hswz.Common;

namespace ResourceSpider.GetItems
{
    public class ItemCenter
    {
        private PageList plist;
        private SingleSource source;
        /// <summary>
        /// 获取列表数据的线程是否结束
        /// </summary>
        private Boolean isListThreadOver = false;
        /// <summary>
        /// 获取列数据项的线程是否结束
        /// </summary>
        private Boolean isItemThreadOver = false;


        public void Do()
        {
            plist = new PageList();
            source = new SingleSource();
            source.CompleteEvent += Source_CompleteEvent;


            Comm.WriteLog("strt list task ", Util.Log.LogType.Info);

            Task.Factory.StartNew(() => { Init(); });


            Comm.WriteLog("strt item task ", Util.Log.LogType.Info);

            Task.Factory.StartNew(() => { source.Do(); });



            while (!isListThreadOver || !isItemThreadOver)
            {
                Thread.Sleep(5 * 1000);
            }

            Comm.WriteLog("task over", Util.Log.LogType.Info);
        }

        private void Source_CompleteEvent()
        {
            isItemThreadOver = true;
        }

        private void Init()
        {

            if (WebConfigData.UrlFormatFrom == "1")
            {   //从网站上抓取分页链接
                var urlEnteys = DbCenter.GetDistinctDomainHostUrls();

                if (urlEnteys.Count == 0)
                {
                    return;
                }

                Comm.WriteLog("开始从各个网站上抓取分页链接", Util.Log.LogType.Info);

                LoopGetPageList(urlEnteys);
            }
            else
            {   //读取数据库中已有的分页链接
                InitFormatUrlFromDb();

                Comm.WriteLog("已经从数据库中读取所有分页链接", Util.Log.LogType.Info);
            }

            Comm.WriteLog("获取列表任务结束", Util.Log.LogType.Info);

            isListThreadOver = true;

            source.SetListTaskStatus(ThreadStatus.Stoped);
        }

        private void LoopGetPageList(List<String> urlEntrys)
        {
            foreach (String item in urlEntrys)
            {
                plist.SearchPageLink(item, 1);
            }
        }

        /// <summary>
        /// 从数据库加载分页链接
        /// </summary>
        private void InitFormatUrlFromDb()
        {
            var lst = DbCenter.GetFormatList();
            foreach (var item in lst)
            {
                ItemData.AddPageLink(item.url_format);
            }
        }
    }
}
