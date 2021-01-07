using System;

namespace HtmlSpider
{
    internal class Program
    {
        private static void Main(String[] args)
        {
            //GetNjjzw();
            //Console.WriteLine("操作完成，请按任意键退出...");
            //Console.ReadKey();

            Util.Log.LogUtil.SetLogPath(AppDomain.CurrentDomain.BaseDirectory + "Log");

            GetHtmlText();

        }

        private static void GetHtmlText()
        {
            while (true)
            {
                Console.WriteLine("请输入地址：");
                String url = Console.ReadLine();
                if (!String.IsNullOrWhiteSpace(url))
                {
                    //PageInfo info = PageBLL.GetPageInfo(url, String.Empty);

                    //if (info != null)
                    //{
                    //    Console.WriteLine($"title: {info.Title}");
                    //    Console.WriteLine($"h1: {info.H1}");
                    //    Console.WriteLine($"keywords: {info.KeyWords}");
                    //    Console.WriteLine($"content: {info.Content}");
                    //}

                    PageBLL.LoopGetPageInfo(url, String.Empty);
                }
            }
        }

        private static void GetNjjzw()
        {
            NjjzwBLL bll = new NjjzwBLL();

            bll.GetData("http://www.hydcd.com/baike/naojinjizhuanwan.htm");
            bll.GetData("http://www.hydcd.com/baike/naojinjizhuanwan2.htm");
            bll.GetData("http://www.hydcd.com/baike/naojinjizhuanwan3.htm");
            bll.GetData("http://www.hydcd.com/baike/naojinjizhuanwan4.htm");

            bll.SaveDatas();
        }
    }
}
