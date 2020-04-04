using Spetmall.Model;
using System;
using System.Data;
using System.Linq;
using Spetmall.Common;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using Spetmall.Model.Page;

namespace Spetmall.DAL
{
    public class receiptDAL : DAL.BaseQuery
    {

        /// <summary>
        /// 限时折扣
        /// </summary>
        private static readonly string getDiscountSql = @"
SELECT a.*,b.type ruleType,b.aim,b.sale,c.productid FROM discount a
INNER JOIN salerule b ON a.id=b.saleid
LEFT JOIN saleproduct c ON a.id=c.saleid AND c.type=0
WHERE a.state=1 AND a.starttime<NOW() AND a.endtime>NOW() {0}
ORDER BY a.id DESC
";

        /// <summary>
        /// 满就减
        /// </summary>
        private static readonly string getFullSendSql = @"
SELECT a.*,b.type ruleType,b.aim,b.sale,c.productid FROM fullsend a
INNER JOIN salerule b ON a.id=b.saleid
LEFT JOIN saleproduct c ON a.id=c.saleid AND c.type=1
WHERE a.state=1 AND a.starttime<NOW() AND a.endtime>NOW() {0}
ORDER BY a.id DESC
";

        /// <summary>
        /// 查找商品参与的限时折扣活动信息
        /// </summary>
        /// <param name="aimId">商品或分类的id</param>
        /// <param name="type">-1所有 0按店铺 1按分类 2按商品</param>
        /// <returns></returns>
        public static List<receipt_discount> GetDiscount(int aimId, short type)
        {
            try
            {
                string where = GetWhere(aimId, type);
                string sql = string.Format(getDiscountSql, where);
                using (DBHelper dbHelper = new DBHelper(WebConfigData.DataBaseType, WebConfigData.ConnString))
                {
                    DataTable dt = dbHelper.ExecuteDataTable(sql);
                    if (dt != null && dt.Rows.Count > 0)
                        return dt.ToList<receipt_discount>();
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "GetDiscount 结算时获取折扣信息出错\r\n" + ex.Message);
            }

            return new List<receipt_discount>();
        }

        /// <summary>
        /// 查找商品参与的满就送活动信息
        /// </summary>
        /// <param name="aimId">商品或分类的id</param>
        /// <param name="type">-1所有 0按店铺 1按分类 2按商品</param>
        /// <returns></returns>
        public static List<receipt_fullsend> GetFullSend(int aimId, short type)
        {
            try
            {
                string where = GetWhere(aimId, type);
                string sql = string.Format(getFullSendSql, where);
                using (DBHelper dbHelper = new DBHelper(WebConfigData.DataBaseType, WebConfigData.ConnString))
                {
                    DataTable dt = dbHelper.ExecuteDataTable(sql);
                    if (dt != null && dt.Rows.Count > 0)
                        return dt.ToList<receipt_fullsend>();
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "GetFullSend 结算时获取满就减信息出错\r\n" + ex.Message);
            }

            return new List<receipt_fullsend>();
        }

        /// <summary>
        /// 获取查询条件
        /// </summary>
        /// <param name="aimId">商品或分类的id</param>
        /// <param name="type">-1所有 0按店铺 1按分类 2按商品</param>
        /// <returns></returns>
        private static string GetWhere(int aimId, short type)
        {
            string where = string.Empty;

            if (type != -1)
                where += $" and a.type={type}";    //店铺级

            if (type != 0 && aimId != 0)
                where += $" and c.productid={aimId}";

            return where;
        }

    }
}
