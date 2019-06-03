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

        private static readonly string getDatasSql = "SELECT * FROM product WHERE 1=1 {0} {1}";

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

        public List<product> GetProducts(string productId, string category, string keyWord, string orderBy, int page, int pageSize)
        {
            try
            {
                string where = GetWhere(productId, category, keyWord);
                string orderby = string.Empty;
                if (!string.IsNullOrWhiteSpace(orderBy))
                    orderBy = $"order by {orderBy}";

                string sql = string.Format(getDatasSql, where, orderBy);

                using (DBHelper dbHelper = new DBHelper(WebConfigData.DataBaseType, WebConfigData.ConnString))
                {
                    DataTable dt = dbHelper.ExecuteDataTablePage(sql, pageSize, page);
                    if (dt != null && dt.Rows.Count > 0)
                        return dt.ToList<Model.product>();
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "GetProducts 获取商品数据出错\r\n" + ex.Message);
            }
            return new List<product>();
        }

        private string GetWhere(string productId, string category, string keyWord)
        {
            string where = string.Empty;
            if (!string.IsNullOrWhiteSpace(productId))
                where += $" and id={productId}";
            if (!string.IsNullOrWhiteSpace(category))
                where += $" and category={category}";
            if (!string.IsNullOrWhiteSpace(keyWord))
                where += $" and (name like'%{keyWord}%' or barcode like'%{keyWord}%')";

            return where;
        }

    }
}
