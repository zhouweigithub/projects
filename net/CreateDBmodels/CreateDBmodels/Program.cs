using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateDBmodels
{
    public class Program
    {
        private static void Main(string[] args)
        {
            string dbName = Common.Config.GetConfigToString("DbName");
            string nameSpace = Common.Config.GetConfigToString("NameSpace");
            Console.WriteLine();
            Console.WriteLine("功能：根据数据库生成所有表的模型文件");
            Console.WriteLine();
            Console.WriteLine($"模型命名空间：{nameSpace}");
            Console.WriteLine($"目标数据库名：{dbName}");
            Console.WriteLine();
            Console.WriteLine("是否继续？(y/n)");

            ConsoleKeyInfo key = Console.ReadKey();

            if (key.Key == ConsoleKey.Y)
            {
                BLL.BLL.CreateTableModelFiles();
                Console.WriteLine("\n任务完成，请按任意键退出");
            }
            else
            {
                Console.WriteLine("\n任务取消，请按任意键退出");
            }

            Console.ReadKey();
        }
    }
}
