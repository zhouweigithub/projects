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
            //string url = "https://news.163.com/19/0710/08/EJNA0AJF0001875O.html";
            //string url = "http://www.sohu.com/a/325776574_267106?g=0?code=16c94ea212bd093ba101a88e09c362b8&spm=smpc.home.top-news1.2.1562752238299HdPTTob&_f=index_cpc_1_0";
            string url = "https://baijiahao.baidu.com/s?id=1618166798631963919&wfr=spider&for=pc";


            PageInfo info = BLL.GetPageInfo(url);

            if (info != null)
            {
                Console.WriteLine($"title: {info.Title}");
                Console.WriteLine($"h1: {info.H1}");
                Console.WriteLine($"keywords: {info.KeyWords}");
                Console.WriteLine($"content: {info.Content}");
            }

            Console.ReadKey();
        }
    }
}
