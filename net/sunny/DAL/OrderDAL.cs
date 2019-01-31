using Sunny.Model.Custom;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.DAL
{
    public class OrderDAL
    {
        /// <summary>
        /// 取订单和商品信息
        /// </summary>
        private static readonly string getOrderProductList = @"
SELECT a.order_id,a.money,a.state,a.coupon_money,a.discount_money,a.crtime,b.product_id,b.product_name,b.count,
b.price,b.orig_price,b.total_amount,c.name venue_name,d.name campus_name,e.main_img FROM `order` a
INNER JOIN order_product b ON a.order_id=b.order_id
LEFT JOIN venue c ON b.venueid=c.id
LEFT JOIN campus d ON c.campus_id=d.id
LEFT JOIN product e ON b.product_id=e.id
WHERE 1=1 {0}
";
        /// <summary>
        /// 取订单中商品的规格信息
        /// </summary>
        private static readonly string getOrderProductSpecificationList = @"
SELECT a.order_id,a.product_id,GROUP_CONCAT(c.name)specifications FROM order_product a
LEFT JOIN product_specification_detail b ON a.product_id=b.product_id AND a.plan_code=b.plan_code
LEFT JOIN specification_detail c ON b.specification_detail_id=c.id
WHERE a.order_id IN({0})
GROUP BY a.order_id,a.product_id
";
        /// <summary>
        /// 取订单中的优惠券信息
        /// </summary>
        private static readonly string getOrderCouponList = "SELECT order_id,`name`,`count`,money FROM order_coupon WHERE order_id IN({0})";

        /// <summary>
        /// 更新订单状态为已支付，同时写入支付历史记录
        /// </summary>
        private static readonly string orderPaySuccess = @"
UPDATE `order` SET state=1 WHERE orderid='{0}';
INSERT INTO pay_record(order_id,money) VALUES('{0}','{1}');";


        /// <summary>
        /// 取订单和商品信息，有订单号就按订单号查询 ，没有再按其他两个条件查询
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <param name="state">状态0未支付1已支付2已发货3已收货4已评价999所有订单</param>
        /// <returns></returns>
        public static List<CustOrderProduct> GetOrderProductList(int userid, int state, string orderId)
        {
            try
            {
                //有订单号就按订单号查询 ，没有再按其他两个条件查询

                string where = string.Empty;

                if (!string.IsNullOrWhiteSpace(orderId))
                {
                    where += $" and a.order_id='{orderId}'";
                }
                else
                {
                    if (userid != 0)
                    {
                        where += $" and a.userid='{userid}'";
                    }
                    if (state != 999)
                    {
                        where += $" and a.state='{state}'";
                    }
                }

                using (DBHelper dbhelper = new DBHelper())
                {
                    DataTable dt = dbhelper.ExecuteDataTable(string.Format(getOrderProductList, where));

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.ToList<CustOrderProduct>();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("GetOrderProductList 出错：" + ex, Util.Log.LogType.Error);
            }

            return new List<CustOrderProduct>();
        }

        /// <summary>
        /// 取订单中商品的规格信息
        /// </summary>
        /// <param name="orderIds"></param>
        /// <returns></returns>
        public static List<CustOrderProductSpecification> GetOrderProductSpecificationList(string[] orderIds)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    string orderIdsString = "'" + string.Join("','", orderIds) + "'";
                    DataTable dt = dbhelper.ExecuteDataTable(string.Format(getOrderProductSpecificationList, orderIdsString));

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.ToList<CustOrderProductSpecification>();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("GetOrderProductSpecificationList 出错：" + ex, Util.Log.LogType.Error);
            }

            return new List<CustOrderProductSpecification>();
        }

        /// <summary>
        /// 取订单中的优惠券信息
        /// </summary>
        /// <param name="orderIds"></param>
        /// <returns></returns>
        public static List<CustOrderCoupon> GetOrderCouponList(string[] orderIds)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    string orderIdsString = "'" + string.Join("','", orderIds) + "'";
                    DataTable dt = dbhelper.ExecuteDataTable(string.Format(getOrderCouponList, orderIdsString));

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.ToList<CustOrderCoupon>();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("GetOrderCouponList 出错：" + ex, Util.Log.LogType.Error);
            }

            return new List<CustOrderCoupon>();
        }

        /// <summary>
        /// 更新订单状态为已支付，同时写入支付历史记录
        /// </summary>
        /// <param name="orderId">订单id</param>
        /// <param name="money">订单金额</param>
        /// <returns></returns>
        public static bool OrderPaySuccess(string orderId, decimal money)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    int count = dbhelper.ExecuteNonQuery(string.Format(orderPaySuccess, orderId, money));
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("OrderPaySuccess 出错：" + ex, Util.Log.LogType.Error);
            }

            return false;
        }


    }
}
