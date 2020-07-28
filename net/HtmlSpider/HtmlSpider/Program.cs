using HtmlSpider.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlSpider
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //GetNjjzw();
            //Console.WriteLine("操作完成，请按任意键退出...");
            //Console.ReadKey();

            GetHtmlText();
        }

        private static void GetHtmlText()
        {
            while (true)
            {
                Console.WriteLine("请输入地址：");
                string url = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(url))
                {
                    PageInfo info = BLL.GetPageInfo(url);

                    if (info != null)
                    {
                        Console.WriteLine($"title: {info.Title}");
                        Console.WriteLine($"h1: {info.H1}");
                        Console.WriteLine($"keywords: {info.KeyWords}");
                        Console.WriteLine($"content: {info.Content}");
                    }
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
