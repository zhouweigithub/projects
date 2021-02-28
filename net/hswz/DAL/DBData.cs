using System;
using System.Collections.Generic;
using System.Data;
using Hswz.Common;
using Hswz.Model;
using MySql.Data.MySqlClient;

namespace Hswz.DAL
{
    public class DBData
    {

        /// <summary>
        /// 所有表实例的集合
        /// </summary>
        private static readonly Dictionary<String, BaseQuery> InstanceList = new Dictionary<String, BaseQuery>();


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
            String path = System.IO.Path.Combine(Const.RootWebPath, "App_Data\\TableSetting.xml");
            var settings = XmlHelper.XmlDeserializeFromFile<TableList>(path, Const.DefaultEncoding);
            foreach (var item in settings.TableSettings)
            {
                InstanceList[item.TableName] = new BaseQuery(item.TableName, item.KeyField, item.OrderbyFields);
            }
        }

        /// <summary>
        /// 获取目标表的实例
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public static BaseQuery GetInstance(String tableName)
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

        public static List<T> GetDataListBySql<T>(String sql, params MySqlParameter[] paras) where T : class, new()
        {
            using (DBHelper db = new DBHelper())
            {
                var dt = db.ExecuteDataTableParams(sql, paras);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt.ToList<T>();
                }
            }

            return new List<T>();
        }

        public static DataTable GetDataTableBySql(String sql, params MySqlParameter[] paras)
        {
            using (DBHelper db = new DBHelper())
            {
                return db.ExecuteDataTableParams(sql, paras);
            }
        }

        public static Int32 ExecuteScalarIntBySql(String sql, params MySqlParameter[] paras)
        {
            using (DBHelper db = new DBHelper())
            {
                return db.ExecuteScalarIntParams(sql, paras);
            }
        }

        public static Object ExecuteScalarBySql(String sql, params MySqlParameter[] paras)
        {
            using (DBHelper db = new DBHelper())
            {
                return db.ExecuteScalarParams(sql, paras);
            }
        }

        public static Boolean ExecuteNonQueryBySql(String sql, params MySqlParameter[] paras)
        {
            using (DBHelper db = new DBHelper())
            {
                return db.ExecuteNonQueryParams(sql, paras) > 0;
            }
        }
    }
}
