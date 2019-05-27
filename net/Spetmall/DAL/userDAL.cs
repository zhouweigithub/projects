using Spetmall.Model;
using System;
using System.Data;
using System.Linq;
using Spetmall.Common;
using MySql.Data.MySqlClient;

namespace Spetmall.DAL
{
    public class userDAL : DAL.BaseQuery
    {
        private static readonly userDAL Instance = new userDAL();

        private userDAL()
        {
            this.IsAddIntoCache = false;
            this.TableName = "user";
            this.ItemName = "用户信息";
            this.OrderbyFields = "id asc";
        }

        /// <summary>
        /// 获取唯一实例
        /// </summary>
        /// <returns></returns>
        public static userDAL GetInstance()
        {
            return Instance;
        }

    }
}
