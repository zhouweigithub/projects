using Spetmall.Model;
using System;
using System.Data;
using System.Linq;
using Spetmall.Common;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Spetmall.DAL
{
    public class productDAL : DAL.BaseQuery
    {
        private static readonly productDAL Instance = new productDAL();

        private static readonly string getDatasSql = "SELECT a.*,b.name categoryName FROM product a INNER JOIN category b ON a.category=b.id WHERE 1=1 {0} {1}";
        private static readonly string updateStroeSalesSql = "UPDATE product SET store=store-{0},sales=sales+{0} WHERE id={1} ;";

        private productDAL()
        {
            this.IsAddIntoCache = false;
            this.TableName = "product";
            this.ItemName = "商品信息";
            this.OrderbyFields = "id asc";
        }

        /// <summary>
        /// 获取唯一实例
        /// </summary>
        /// <returns></returns>
        public static productDAL GetInstance()
        {
            return Instance;
        }

        public List<product_show> GetProducts(string productId, string category, string keyWord, string orderBy, int page, int pageSize)
        {
            try
            {
                string where = GetWhere(productId, category, keyWord);
                string orderByString = GetOrderBy(orderBy);
                string sql = string.Format(getDatasSql, where, orderByString);

                using (DBHelper dbHelper = new DBHelper(WebConfigData.DataBaseType, WebConfigData.ConnString))
                {
                    DataTable dt = dbHelper.ExecuteDataTablePage(sql, pageSize, page);
                    if (dt != null && dt.Rows.Count > 0)
                        return dt.ToList<Model.product_show>();
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "GetProducts 获取商品数据出错\r\n" + ex.Message);
            }
            return new List<product_show>();
        }

        private string GetWhere(string productId, string category, string keyWord)
        {
            string where = string.Empty;
            if (!string.IsNullOrWhiteSpace(productId))
                where += $" and a.id={productId}";
            if (!string.IsNullOrWhiteSpace(category))
                where += $" and a.category={category}";
            if (!string.IsNullOrWhiteSpace(keyWord))
                where += $" and (a.name like'%{keyWord}%' or a.barcode like'%{keyWord}%' or a.py like'%{keyWord}%')";

            return where;
        }

        private string GetOrderBy(string orderBy)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(orderBy))
            {
                result += " order by ";
                if (orderBy == "store")
                    result += orderBy + " asc";
                else
                    result += orderBy + " desc";
            }

            return result;
        }

        /// <summary>
        /// 更新商品的库存和销量
        /// </summary>
        /// <param name="productId">商品id</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public bool ReduceStoreAndSales(int productId, int count)
        {
            try
            {
                using (DBHelper dbHelper = new DBHelper(WebConfigData.DataBaseType, WebConfigData.ConnString))
                {
                    int successCount = dbHelper.ExecuteNonQuery(string.Format(updateStroeSalesSql, count, productId));
                    return successCount > 0;
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "ReduceStoreAndSales 更新商品的库存和销量出错\r\n" + ex);
            }
            return false;
        }

    }
}
