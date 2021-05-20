using System;
using System.Collections.Generic;
using System.Data;
using CreateDBmodels.Models;
using Util.Log;

namespace CreateDBmodels.DAL
{
    /// <summary>
    /// 查询表的基本信息
    /// </summary>
    public class TableInfoDAL : BaseQuery
    {

        private static readonly String dbType = "mysql";
        private static readonly String dbName = Common.Config.GetConfigToString("DbName");
        private static readonly String conn = System.Configuration.ConfigurationManager.ConnectionStrings["conn"].ConnectionString;


        /// <summary>
        /// 读取数据库各表的字段信息
        /// </summary>
        /// <returns></returns>
        public static List<ColumeModel> GetTableColumeInfo()
        {
            String sql = $"SELECT TABLE_NAME, column_name, DATA_TYPE, column_comment FROM  information_schema.COLUMNS WHERE TABLE_SCHEMA = '{dbName}';";
            return GetTableInfo<ColumeModel>(sql);
        }


        /// <summary>
        /// 读取数据库各表的基本信息
        /// </summary>
        /// <returns></returns>
        public static List<TableModel> GetTableInfo()
        {
            String sql = $"SELECT table_name,table_type,table_comment FROM INFORMATION_SCHEMA.TABLES WHERE table_schema='{dbName}';";
            return GetTableInfo<TableModel>(sql);
        }


        /// <summary>
        /// 读取数据库各表的主键信息
        /// </summary>
        /// <returns></returns>
        public static List<PrimaryKeyModel> GetPrimaryKeyInfo()
        {
            String sql = $"SELECT table_name,column_name FROM INFORMATION_SCHEMA.`KEY_COLUMN_USAGE` WHERE table_schema='{dbName}' AND constraint_name='PRIMARY';";
            return GetTableInfo<PrimaryKeyModel>(sql);
        }

        /// <summary>
        /// 执行SQL查询
        /// </summary>
        /// <typeparam name="T">返回值模型</typeparam>
        /// <param name="sql">SQL查询语句</param>
        /// <returns></returns>
        public static List<T> GetTableInfo<T>(String sql) where T : class, new()
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper(dbType, conn))
                {
                    DataTable dt = dbhelper.ExecuteDataTable(sql);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.ToList<T>();
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.Write("执行SQL查询时出错：\r\n" + ex.ToString(), LogType.Error);
            }

            return new List<T>();
        }
    }
}
