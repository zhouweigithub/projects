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
    public class orderProductDAL : DAL.BaseQuery
    {
        private static readonly orderProductDAL Instance = new orderProductDAL();

        private static readonly string getProductInfoByOrderId = @"
SELECT a.productid id,a.count number,a.discountMoney discount_goods_total_price,
a.money goods_total_price,a.payMoney goods_activity_total_price,a.price,b.name,
b.thumbnail mapthumimg,b.store,b.warn alarm FROM orderproduct a
INNER JOIN product b ON a.productid=b.id
WHERE a.orderid='{0}'";

        private orderProductDAL()
        {
            this.IsAddIntoCache = false;
            this.TableName = "orderproduct";
            this.ItemName = "订单中的商品信息";
            this.OrderbyFields = "id desc";
        }

        /// <summary>
        /// 获取唯一实例
        /// </summary>
        /// <returns></returns>
        public static orderProductDAL GetInstance()
        {
            return Instance;
        }

        public static List<ReceiptOrderProductInfo> GetProductByOrderId(string orderid)
        {
            try
            {
                string sql = string.Format(getProductInfoByOrderId, orderid);

                using (DBHelper dbHelper = new DBHelper(WebConfigData.DataBaseType, WebConfigData.ConnString))
                {
                    DataTable dt = dbHelper.ExecuteDataTable(sql);
                    if (dt != null && dt.Rows.Count > 0)
                        return dt.ToList<ReceiptOrderProductInfo>();
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "GetProductByOrderId 获取订单的商品数据出错\r\n" + ex.Message);
            }

            return new List<ReceiptOrderProductInfo>();
        }

    }
}
