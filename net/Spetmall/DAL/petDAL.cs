using Spetmall.Model;
using System;
using System.Data;
using System.Linq;
using Spetmall.Common;
using MySql.Data.MySqlClient;

namespace Spetmall.DAL
{
    public class petDAL : DAL.BaseQuery
    {
        private static readonly petDAL Instance = new petDAL();

        private petDAL()
        {
            this.IsAddIntoCache = false;
            this.TableName = "pet";
            this.ItemName = "宠物分类";
            this.OrderbyFields = "id asc";
        }

        /// <summary>
        /// 获取唯一实例
        /// </summary>
        /// <returns></returns>
        public static petDAL GetInstance()
        {
            return Instance;
        }

    }
}
