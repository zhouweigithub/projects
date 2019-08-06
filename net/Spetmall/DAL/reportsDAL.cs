using Spetmall.Common;
using Spetmall.Model.Page;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spetmall.DAL
{
    /// <summary>
    /// 报表处理
    /// </summary>
    public class reportsDAL
    {

        private static readonly string getPayInfoSql = @"
SELECT crdate,IFNULL(SUM(payMoney),0)payMoney,IFNULL(SUM(discountMoney),0)discountMoney,
IFNULL(SUM(adjustMomey),0)adjustMomey,IFNULL(SUM(profitMoney),0)profitMoney,
IFNULL(SUM(costMoney),0)costMoney,COUNT(1)payCount ,0 rechargeMoney,0 railCardMoney
FROM `order` WHERE state=0 {0}
GROUP BY crdate

UNION

SELECT DATE(crtime)crdate,0 payMoney,0 discountMoney,0 adjustMomey,0 profitMoney,
0 costMoney,0 payCount,SUM(paymoney)rechargeMoney,0 railCardMoney FROM recharge WHERE 1=1 {1}
GROUP BY crdate

UNION

SELECT DATE(crtime)crdate,0 payMoney,0 discountMoney,0 adjustMomey,0 profitMoney,
0 costMoney,0 payCount,0 rechargeMoney,SUM(money)railCardMoney FROM railcard  WHERE 1=1 {1}
GROUP BY crdate

ORDER BY crdate DESC
";

        private static readonly string getPayInfoTotalSql = @"select sum(profitMoney)profitMoney,sum(payMoney)payMoney, 
sum(discountMoney)discountMoney,sum(adjustMomey)adjustMomey,sum(costMoney)costMoney,sum(payCount)payCount from ({0})t";

        private static readonly string getProductInfoSql = @"
SELECT b.crdate,SUM(a.count)`count` FROM orderproduct a
INNER JOIN `order` b ON a.orderid=b.id
WHERE b.state=0 and a.productid='{0}' {1} GROUP BY b.crdate
";

        private static readonly string getMemberInfoSql = @"SELECT crdate,COUNT(DISTINCT memberid)`count` FROM `order` WHERE state=0 AND memberid>0 {0} GROUP BY crdate";


        /// <summary>
        /// 获取整体交易流水
        /// </summary>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public static List<payInfo> GetPayInfos(string startdate, string enddate, int pageSize, int page)
        {
            try
            {
                string where1 = GetWhereString(startdate, enddate);
                string where2 = where1.Replace("crdate", "DATE(crtime)");

                string sql = string.Format(getPayInfoSql, where1, where2);

                using (DBHelper dbHelper = new DBHelper(WebConfigData.DataBaseType, WebConfigData.ConnString))
                {
                    DataTable dt = dbHelper.ExecuteDataTablePage(sql, pageSize, page);
                    if (dt != null && dt.Rows.Count > 0)
                        return dt.ToList<payInfo>();
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "GetPayInfos 获取交易流水报表出错：\r\n" + ex.Message);
            }

            return new List<payInfo>();
        }

        private static string GetWhereString(string startdate, string enddate)
        {
            string where1 = string.Empty;
            if (!string.IsNullOrEmpty(startdate))
                where1 += $" and crdate>='{startdate}'";
            if (!string.IsNullOrEmpty(enddate))
                where1 += $" and crdate<='{enddate}'";
            return where1;
        }

        public static int GetPayInfosCount(string startdate, string enddate)
        {
            try
            {
                string where = GetWhereString(startdate, enddate);
                string sqldata = string.Format(getPayInfoSql, where, string.Empty);

                using (DBHelper dbHelper = new DBHelper(WebConfigData.DataBaseType, WebConfigData.ConnString))
                {
                    string sql = string.Format("select count(1) from ({0})t", sqldata);
                    int count = dbHelper.ExecuteScalarInt(sql);
                    return count;
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "GetPayInfosCount 获取数量出错\r\n" + ex.Message);
            }
            return 0;
        }

        public static payInfo GetPayInfosTotal(string startdate, string enddate)
        {
            try
            {
                string where = GetWhereString(startdate, enddate);
                string sqldata = string.Format(getPayInfoSql, where, string.Empty);

                using (DBHelper dbHelper = new DBHelper(WebConfigData.DataBaseType, WebConfigData.ConnString))
                {
                    DataTable dt = dbHelper.ExecuteDataTable(string.Format(getPayInfoTotalSql, sqldata));
                    if (dt != null && dt.Rows.Count > 0)
                        return dt.ToList<payInfo>()[0];
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "GetPayInfosTotal 获取汇总数据出错\r\n" + ex.Message);
            }
            return null;
        }


        /// <summary>
        /// 获取商品交易数量流水
        /// </summary>
        /// <param name="productid"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public static List<countPayInfo> GetProductInfos(int productid, string startdate, string enddate)
        {
            try
            {
                string where = string.Empty;
                if (!string.IsNullOrEmpty(startdate))
                    where += $" and b.crdate>='{startdate}'";
                if (!string.IsNullOrEmpty(enddate))
                    where += $" and b.crdate<='{enddate}'";

                string sql = string.Format(getProductInfoSql, productid, where);

                using (DBHelper dbHelper = new DBHelper(WebConfigData.DataBaseType, WebConfigData.ConnString))
                {
                    DataTable dt = dbHelper.ExecuteDataTable(sql);
                    if (dt != null && dt.Rows.Count > 0)
                        return dt.ToList<countPayInfo>();
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "GetProductInfos 获取商品交易数量流水报表出错：\r\n" + ex.Message);
            }

            return new List<countPayInfo>();
        }

        public static List<countPayInfo> GetMemberInfos(string startdate, string enddate)
        {
            try
            {
                string where = string.Empty;
                if (!string.IsNullOrEmpty(startdate))
                    where += $" and crdate>='{startdate}'";
                if (!string.IsNullOrEmpty(enddate))
                    where += $" and crdate<='{enddate}'";

                string sql = string.Format(getMemberInfoSql, where);

                using (DBHelper dbHelper = new DBHelper(WebConfigData.DataBaseType, WebConfigData.ConnString))
                {
                    DataTable dt = dbHelper.ExecuteDataTable(sql);
                    if (dt != null && dt.Rows.Count > 0)
                        return dt.ToList<countPayInfo>();
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "GetMemberInfos 获取会员交易数量流水报表出错：\r\n" + ex.Message);
            }

            return new List<countPayInfo>();
        }


    }
}
