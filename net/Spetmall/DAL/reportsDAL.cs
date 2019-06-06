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
IFNULL(SUM(costMoney),0)costMoney,COUNT(1)payCount 
FROM `order` WHERE state=0 {0} 
GROUP BY crdate;
";

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
        public static List<payInfo> GetPayInfos(string startdate, string enddate)
        {
            try
            {
                string where = string.Empty;
                if (!string.IsNullOrEmpty(startdate))
                    where += $" and crdate>='{startdate}'";
                if (!string.IsNullOrEmpty(enddate))
                    where += $" and crdate<='{enddate}'";

                string sql = string.Format(getPayInfoSql, where);

                using (DBHelper dbHelper = new DBHelper(WebConfigData.DataBaseType, WebConfigData.ConnString))
                {
                    DataTable dt = dbHelper.ExecuteDataTable(sql);
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
