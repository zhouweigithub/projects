using Spetmall.Model;
using System;
using System.Data;
using System.Linq;
using Spetmall.Common;
using MySql.Data.MySqlClient;

namespace Spetmall.DAL
{
    public class rechargeDAL : DAL.BaseQuery
    {
        private static readonly rechargeDAL Instance = new rechargeDAL();

        private rechargeDAL()
        {
            this.IsAddIntoCache = false;
            this.TableName = "recharge";
            this.ItemName = "会员充值记录";
            this.OrderbyFields = "id desc";
        }

        /// <summary>
        /// 获取唯一实例
        /// </summary>
        /// <returns></returns>
        public static rechargeDAL GetInstance()
        {
            return Instance;
        }

    }
}
