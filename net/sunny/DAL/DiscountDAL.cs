using Sunny.Model;
using Sunny.Model.Custom;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.DAL
{
    public class DiscountDAL
    {

        private static readonly string getDiscountSql = @"
SELECT a.id productid,SUM(c.money)money FROM product a
LEFT JOIN product_discount b ON a.id=b.product_id AND b.start_time<NOW() AND b.end_time>NOW()
LEFT JOIN discount c ON b.discount_id=c.id
WHERE b.state=0 AND c.state=0 AND a.id IN({0})
GROUP BY a.id
";

        private static readonly string getDisscountByProductIdSql = @"
SELECT a.product_id productid,b.money FROM product_discount a
INNER JOIN discount b ON a.discount_id=b.id
WHERE b.state=0 AND a.start_time<NOW() AND a.end_time>NOW() AND a.product_id='{0}'";


        public static List<CustDisscount> GetDiscountList(string productIds)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    DataTable dt = dbhelper.ExecuteDataTable(string.Format(getDiscountSql, productIds));

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.ToList<CustDisscount>();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("GetDiscountList 出错：" + ex, Util.Log.LogType.Error);
            }

            return new List<CustDisscount>();
        }

        /// <summary>
        /// 根据商品id获取当前折扣金额
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static CustDisscount GetDisscountByProductId(int productId)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    DataTable dt = dbhelper.ExecuteDataTable(string.Format(getDisscountByProductIdSql, productId));

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.ToList<CustDisscount>().First();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("GetDisscountByProductId 出错：" + ex, Util.Log.LogType.Error);
            }

            return null;
        }
    }

}
