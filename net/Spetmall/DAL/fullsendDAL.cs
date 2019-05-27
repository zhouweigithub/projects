using Spetmall.Model;
using System;
using System.Data;
using System.Linq;
using Spetmall.Common;
using MySql.Data.MySqlClient;

namespace Spetmall.DAL
{
    public class fullsendDAL : DAL.BaseQuery
    {
        private static readonly fullsendDAL Instance = new fullsendDAL();

        private fullsendDAL()
        {
            this.IsAddIntoCache = false;
            this.TableName = "fullsend";
            this.ItemName = "满就送活动";
            this.OrderbyFields = "id asc";
        }

        /// <summary>
        /// 获取唯一实例
        /// </summary>
        /// <returns></returns>
        public static fullsendDAL GetInstance()
        {
            return Instance;
        }

    }
}
