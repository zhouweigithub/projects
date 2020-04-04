using Spetmall.Model;
using System;
using System.Data;
using System.Linq;
using Spetmall.Common;
using MySql.Data.MySqlClient;

namespace Spetmall.DAL
{
    public class memberPetDAL : DAL.BaseQuery
    {
        private static readonly memberPetDAL Instance = new memberPetDAL();

        private memberPetDAL()
        {
            this.IsAddIntoCache = false;
            this.TableName = "memberpet";
            this.ItemName = "会员喜爱的宠物";
            this.OrderbyFields = "id asc";
        }

        /// <summary>
        /// 获取唯一实例
        /// </summary>
        /// <returns></returns>
        public static memberPetDAL GetInstance()
        {
            return Instance;
        }

    }
}
