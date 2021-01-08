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
            try
            {
                using (DBHelper dbhelper = new DBHelper(dbType, conn))
                {
                    String sql = $"SELECT TABLE_NAME, column_name, DATA_TYPE, column_comment FROM  information_schema.COLUMNS WHERE TABLE_SCHEMA = '{dbName}' ;";
                    DataTable dt = dbhelper.ExecuteDataTable(sql);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.ToList<ColumeModel>();
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.Write("读取数据库各表的字段信息时出错：" + ex.ToString(), LogType.Error);
            }

            return new List<ColumeModel>();
        }

        /// <summary>
        /// 读取数据库各表的基本信息
        /// </summary>
        /// <returns></returns>
        public static List<TableModel> GetTableInfo()
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper(dbType, conn))
                {
                    String sql = $"SELECT table_name,table_type,table_comment FROM INFORMATION_SCHEMA.TABLES WHERE table_schema='{dbName}' ;";
                    DataTable dt = dbhelper.ExecuteDataTable(sql);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.ToList<TableModel>();
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.Write("读取数据库各表的基本信息时出错：" + ex.ToString(), LogType.Error);
            }

            return new List<TableModel>();
        }

    }
}
