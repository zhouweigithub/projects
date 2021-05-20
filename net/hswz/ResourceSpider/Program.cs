using System;
using Hswz.Common;
using ResourceSpider.GetItems;
using Util.Log;

namespace ResourceSpider
{
    internal class Program
    {
        private static void Main(String[] args)
        {
            //日志路径
            LogUtil.SetLogPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log"));
            Const.RootWebPath = AppDomain.CurrentDomain.BaseDirectory;

            GetItems();

            Console.WriteLine("over");

            Console.ReadKey();
        }

        private static void GetItems()
        {
            new ItemCenter().Do();
        }

        private static void Search()
        {
            Console.WriteLine("请输入搜索方式：");
            Console.WriteLine("1、bing");
            Console.WriteLine("2、google");

            var key = Console.ReadKey();
            Console.WriteLine();

            if (key.Key == ConsoleKey.D1)
            {
                new SearchUrlWithDbBLL().Start();
            }
            else if (key.Key == ConsoleKey.D2)
            {
                new SearchGoogleWithDbBLL().Start();
            }
            else
            {
                Console.WriteLine("输入错误，任务结束");
            }
        }
    }
}
