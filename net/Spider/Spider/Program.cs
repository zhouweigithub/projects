using Spider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider
{
    class Program
    {
        static void Main(string[] args)
        {
            UrlSpider.Start();

            do
            {
                Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} wait {CommonDatas.UrlQueue.Count} over {CommonDatas.DealedUrlQueue.Count}");
                System.Threading.Thread.Sleep(5 * 1000);
            } while (CommonDatas.UrlQueue.Count > 0);

            Console.WriteLine("操作完成，请按任意键退出");
            Console.ReadKey();
        }
    }
}
