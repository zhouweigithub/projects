using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateImportExportMysqlDataScript
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine();
            Console.WriteLine("创建导入导出批处理文件");
            Console.WriteLine();
            Console.WriteLine("功能选项");
            Console.WriteLine("1    根据配置文件创建批处理文件");
            Console.WriteLine("3915 在程序根目录生成默认配置文件");
            Console.WriteLine();
            Console.Write("请选择 ");

            try
            {
                string key = Console.ReadLine();
                if (key == "1")
                {
                    new BLL().CreateScriptFiles();
                    Console.WriteLine();
                    Console.WriteLine("操作完成");
                }
                else if (key == "3915")
                {
                    new BLL().CreateConfigFile();
                    Console.WriteLine();
                    Console.WriteLine("操作完成");
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("无效选择");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine();
            Console.Write("请按任意键退出...");
            Console.ReadKey();
        }
    }
}
