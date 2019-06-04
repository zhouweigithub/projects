using Spetmall.Model;
using System;
using System.Data;
using System.Linq;
using Spetmall.Common;
using MySql.Data.MySqlClient;

namespace Spetmall.DAL
{
    public class orderProductDAL : DAL.BaseQuery
    {
        private static readonly orderProductDAL Instance = new orderProductDAL();

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

    }
}
