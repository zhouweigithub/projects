using System;
using Hswz.Common;
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

            SearchDomainBLL2.Start();
        }
    }
}
