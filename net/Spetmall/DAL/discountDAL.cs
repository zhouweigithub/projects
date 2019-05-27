using Spetmall.Model;
using System;
using System.Data;
using System.Linq;
using Spetmall.Common;
using MySql.Data.MySqlClient;

namespace Spetmall.DAL
{
    public class discountDAL : DAL.BaseQuery
    {
        private static readonly discountDAL Instance = new discountDAL();

        private discountDAL()
        {
            this.IsAddIntoCache = false;
            this.TableName = "discount";
            this.ItemName = "折扣信息";
            this.OrderbyFields = "id asc";
        }

        /// <summary>
        /// 获取唯一实例
        /// </summary>
        /// <returns></returns>
        public static discountDAL GetInstance()
        {
            return Instance;
        }

    }
}
