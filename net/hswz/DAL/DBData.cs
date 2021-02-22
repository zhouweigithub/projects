using Hswz.Common;
using Hswz.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hswz.DAL
{
    public class DBData
    {

        /// <summary>
        /// 所有表实例的集合
        /// </summary>
        private static Dictionary<string, BaseQuery> InstanceList = new Dictionary<string, BaseQuery>();


        private DBData()
        {

        }

        static DBData()
        {
            CreateInstanceList();
        }

        /// <summary>
        /// 创建所有表的实例
        /// </summary>
        private static void CreateInstanceList()
        {
            string path = System.IO.Path.Combine(Common.Const.RootWebPath, "App_Data\\TableSetting.xml");
            List<Model.TableSetting> settings = XmlHelper.XmlDeserializeFromFile<List<Model.TableSetting>>(path, Encoding.UTF8);
            foreach (Model.TableSetting item in settings)
            {
                InstanceList[item.TableName] = new BaseQuery(item.TableName, item.KeyField, item.OrderbyFields, item.IsAddIntoCache);
            }
        }

        /// <summary>
        /// 获取目标表的实例
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public static BaseQuery GetInstance(string tableName)
        {
            if (InstanceList.ContainsKey(tableName))
            {
                return InstanceList[tableName];
            }
            else
            {
                return null;
            }
        }

    }
}
