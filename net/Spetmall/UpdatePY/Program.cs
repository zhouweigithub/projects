using Spetmall.Common;
using Spetmall.DAL;
using Spetmall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdatePY
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("开始更新简拼");

            Update();

            Console.WriteLine("更新完成");
            Console.WriteLine("请按任意键退出");
            Console.ReadKey();
        }

        private static void Update()
        {
            var list = memberDAL.GetInstance().GetList<member>();
            foreach (member item in list)
            {
                item.py = ChineseSpell.GetChineseSpell(item.name);
                memberDAL.GetInstance().UpdateByKey(item, item.id);
            }

            var list2 = productDAL.GetInstance().GetList<product>();
            foreach (product item in list2)
            {
                item.py = ChineseSpell.GetChineseSpell(item.name);
                productDAL.GetInstance().UpdateByKey(item, item.id);
            }

            var list3 = railcardDAL.GetInstance().GetList<railcard>();
            foreach (railcard item in list3)
            {
                item.py = ChineseSpell.GetChineseSpell(item.petname);
                railcardDAL.GetInstance().UpdateByKey(item, item.id);
            }
        }
    }
}
