using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance
{
    internal class Program
    {
        public static readonly int userid = int.Parse(Util.AppConfigUtil.GetAppConfig("userid"));
        private static void Main(string[] args)
        {
            Console.Title = "考勤记录导出";

            Console.WriteLine();
            Console.WriteLine("考勤记录导出到EXCEL");
            Console.WriteLine();
            Console.WriteLine("工　　号：" + userid);
            Console.WriteLine("开始日期：" + BLL.startDate);
            Console.WriteLine("结束日期：" + BLL.endDate);
            Console.WriteLine();

            Console.WriteLine("导出所有工号(y)/导出当前工号(任意键)");
            Console.WriteLine();
            Console.Write("请输入：");

            Util.Log.LogUtil.SetLogPath(AppDomain.CurrentDomain.BaseDirectory + "Log");

            string exportFolder = AppDomain.CurrentDomain.BaseDirectory + "export";
            string exportPath = string.Empty;

            if (Console.ReadKey().Key == ConsoleKey.Y)
            {   //导出所有工号
                Console.WriteLine();
                Console.WriteLine();
                for (int i = 1; i <= 330; i++)
                {
                    exportPath = BLL.ToExcel(i, exportFolder);
                    if (string.IsNullOrEmpty(exportPath))
                        Console.WriteLine("导出失败：工号 " + i);
                    else
                        Console.WriteLine("导出成功：工号 " + i);
                }
            }
            else
            {   //导出当前工号
                Console.WriteLine();
                Console.WriteLine();
                exportPath = BLL.ToExcel(userid, exportFolder);
                if (string.IsNullOrEmpty(exportPath))
                    Console.WriteLine("导出失败：工号 " + userid);
                else
                    Console.WriteLine("导出成功：工号 " + userid);
            }

            Console.WriteLine();
            Console.WriteLine("输出目录：" + exportFolder);
            Console.WriteLine();
            Console.Write("任务结束，请按任意键退出...");
            Console.ReadKey();
        }
    }
}
