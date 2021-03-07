using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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

            var urlEnteys = DbCenter.GetDistinctDomainHostUrls();

            if (urlEnteys.Count == 0)
            {
                return;
            }


            Comm.WriteLog("strt list task ", Util.Log.LogType.Info);

            Task.Factory.StartNew(() => { LoopGetPageList(urlEnteys); });


            Comm.WriteLog("strt item task ", Util.Log.LogType.Info);

            Task.Factory.StartNew(() => { source.GetSourceItems(); });



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

        public void LoopGetPageList(List<String> urlEntrys)
        {
            foreach (String item in urlEntrys)
            {
                plist.SearchPageLink(item, 1);
            }

            Comm.WriteLog("获取列表任务结束", Util.Log.LogType.Info);

            isListThreadOver = true;
        }
    }
}
