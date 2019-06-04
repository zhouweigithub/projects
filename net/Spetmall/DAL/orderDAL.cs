using Spetmall.Model;
using System;
using System.Data;
using System.Linq;
using Spetmall.Common;
using MySql.Data.MySqlClient;

namespace Spetmall.DAL
{
    public class orderDAL : DAL.BaseQuery
    {
        private static readonly orderDAL Instance = new orderDAL();

        private orderDAL()
        {
            this.IsAddIntoCache = false;
            this.TableName = "`order`";
            this.ItemName = "订单信息";
            this.OrderbyFields = "id desc";
        }

        /// <summary>
        /// 获取唯一实例
        /// </summary>
        /// <returns></returns>
        public static orderDAL GetInstance()
        {
            return Instance;
        }

    }
}
