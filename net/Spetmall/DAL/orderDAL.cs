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
        /// 获取正常订单详情
        /// </summary>
        private static readonly string getOrderListSql = @"
SELECT a.*,b.name memberName,b.phone memberPhone FROM `order` a
LEFT JOIN member b ON a.memberid=b.id
WHERE 1=1 {0} ORDER BY crtime DESC";

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
            this.OrderbyFields = "crtime desc";
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyword">订单号或会员手机号</param>
        /// <param name="day"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public static List<order_detail> GetOrderList(string keyword, string day, string startdate, string enddate, short state)
        {
            try
            {
                string where = GetWhere(keyword, day, startdate, enddate, state);
                using (DBHelper dbHelper = new DBHelper(WebConfigData.DataBaseType, WebConfigData.ConnString))
                {
                    DataTable dt = dbHelper.ExecuteDataTable(string.Format(getOrderListSql, where));
                    if (dt != null && dt.Rows.Count > 0)
                        return dt.ToList<order_detail>();
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "GetOrderList 获取订单信息出错\r\n" + ex);
            }
            return new List<order_detail>();
        }

        /// <summary>
        /// 获取查询条件
        /// </summary>
        /// <param name="keyword">订单号或会员手机号</param>
        /// <param name="day">最近几天</param>
        /// <param name="startdate">起始日期</param>
        /// <param name="enddate">结束日期</param>
        /// <returns></returns>
        private static string GetWhere(string keyword, string day, string startdate, string enddate, short state)
        {
            string where = string.Empty;

            where += $" and a.state={state}";

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                where += $" and (a.id='{keyword}' or b.phone='{keyword}')";
            }

            if (!string.IsNullOrWhiteSpace(day))
            {
                int.TryParse(day, out int days);
                string date = DateTime.Today.AddDays(-days).ToString("yyyy-MM-dd");
                where += $" and a.crdate>='{date}'";
            }

            if (!string.IsNullOrWhiteSpace(startdate))
                where += $" and a.crdate>='{startdate}'";
            if (!string.IsNullOrWhiteSpace(enddate))
                where += $" and a.crdate<='{enddate}'";

            return where;
        }

    }
}
