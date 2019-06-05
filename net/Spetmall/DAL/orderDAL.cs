using Spetmall.Model;
using System;
using System.Data;
using System.Linq;
using Spetmall.Common;
using MySql.Data.MySqlClient;
using Spetmall.Model.Page;
using System.Collections.Generic;

namespace Spetmall.DAL
{
    public class orderDAL : DAL.BaseQuery
    {
        private static readonly orderDAL Instance = new orderDAL();

        /// <summary>
        /// 获取挂单信息
        /// </summary>
        private static readonly string getGuadanInfoSql = @"
SELECT a.id orderid,b.name,a.remark,a.crtime FROM `order` a
LEFT JOIN member b ON a.memberid=b.id
WHERE a.state=1 ORDER BY a.crtime DESC
";
        private orderDAL()
        {
            this.IsAddIntoCache = false;
            this.TableName = "`order`";
            this.ItemName = "订单信息";
            this.OrderbyFields = "id desc";
        }

        /// <summary>
        /// 获取唯一实例
        /// </summary>
        /// <returns></returns>
        public static orderDAL GetInstance()
        {
            return Instance;
        }

        public static List<guadan> GetGuadans()
        {
            try
            {
                using (DBHelper dbHelper = new DBHelper(WebConfigData.DataBaseType, WebConfigData.ConnString))
                {
                    DataTable dt = dbHelper.ExecuteDataTable(getGuadanInfoSql);
                    if (dt != null && dt.Rows.Count > 0)
                        return dt.ToList<guadan>();
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "GetGuadans 获取挂单信息出错\r\n" + ex);
            }
            return new List<guadan>();
        }

    }
}
