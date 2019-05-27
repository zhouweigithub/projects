using Spetmall.Model;
using System;
using System.Data;
using System.Linq;
using Spetmall.Common;
using MySql.Data.MySqlClient;

namespace Spetmall.DAL
{
    public class categoryDAL : DAL.BaseQuery
    {
        private static readonly categoryDAL Instance = new categoryDAL();

        private categoryDAL()
        {
            this.IsAddIntoCache = false;
            this.TableName = "category";
            this.ItemName = "商品分类";
            this.OrderbyFields = "id asc";
        }

        /// <summary>
        /// 获取唯一实例
        /// </summary>
        /// <returns></returns>
        public static categoryDAL GetInstance()
        {
            return Instance;
        }

    }
}
