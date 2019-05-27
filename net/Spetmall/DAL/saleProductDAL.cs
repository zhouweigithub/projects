using Spetmall.Model;
using System;
using System.Data;
using System.Linq;
using Spetmall.Common;
using MySql.Data.MySqlClient;

namespace Spetmall.DAL
{
    public class saleProductDAL : DAL.BaseQuery
    {
        private static readonly saleProductDAL Instance = new saleProductDAL();

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

    }
}
