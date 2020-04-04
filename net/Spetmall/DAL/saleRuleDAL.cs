using Spetmall.Model;
using System;
using System.Data;
using System.Linq;
using Spetmall.Common;
using MySql.Data.MySqlClient;

namespace Spetmall.DAL
{
    public class saleRuleDAL : DAL.BaseQuery
    {
        private static readonly saleRuleDAL Instance = new saleRuleDAL();

        private saleRuleDAL()
        {
            this.IsAddIntoCache = false;
            this.TableName = "saleRule";
            this.ItemName = "活动规则";
            this.OrderbyFields = "id asc";
        }

        /// <summary>
        /// 获取唯一实例
        /// </summary>
        /// <returns></returns>
        public static saleRuleDAL GetInstance()
        {
            return Instance;
        }

    }
}
