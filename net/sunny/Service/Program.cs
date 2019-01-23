using Sunny.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.Log;

namespace Sunny.Service
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //设置日志路径
            LogUtil.SetLogPath(String.Format("{0}/Log", AppDomain.CurrentDomain.BaseDirectory));
            Const.RootWebPath = AppDomain.CurrentDomain.BaseDirectory;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
            LogWebConfigData();

            new AppointeBLL();

            while (true)
            {
                System.Threading.Thread.Sleep(60 * 1000);
            }
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            LogUtil.Write("服务关闭", LogType.Info);
        }

        /// <summary>
        /// 打印服务器配置参数
        /// </summary>
        private static void LogWebConfigData()
        {
            string configString = $@"服务器配置参数为：
DataBaseType: {WebConfigData.DataBaseType}
ConnString: {WebConfigData.ConnString}
";
            LogUtil.Write(configString, LogType.Info);
        }
    }
}
