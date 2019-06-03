using Spetmall.Model;
using System;
using System.Data;
using System.Linq;
using Spetmall.Common;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Spetmall.DAL
{
    public class saleProductDAL : DAL.BaseQuery
    {
        private static readonly saleProductDAL Instance = new saleProductDAL();

        private static readonly string getDatasSql = @"SELECT a.*,IF(a.ptype=0,b.name,c.name)productName FROM saleproduct a
LEFT JOIN product b ON a.productid=b.id
LEFT JOIN category c ON a.productid=c.id
WHERE a.saleid={0}";


        private saleProductDAL()
        {
            this.IsAddIntoCache = false;
            this.TableName = "saleProduct";
            this.ItemName = "做活动的商品";
            this.OrderbyFields = "id asc";
        }

        /// <summary>
        /// 获取唯一实例
        /// </summary>
        /// <returns></returns>
        public static saleProductDAL GetInstance()
        {
            return Instance;
        }

        public static List<saleproductObject> GetEditDatas(int saleId)
        {
            try
            {
                string sql = string.Format(getDatasSql, saleId);

                using (DBHelper dbHelper = new DBHelper(WebConfigData.DataBaseType, WebConfigData.ConnString))
                {
                    DataTable dt = dbHelper.ExecuteDataTable(sql);
                    if (dt != null && dt.Rows.Count > 0)
                        return dt.ToList<saleproductObject>();
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "GetEditDatas 获取折扣商品或分类数据出错\r\n" + ex.Message);
            }
            return new List<saleproductObject>();
        }
    }
}
