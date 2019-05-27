using Spetmall.Model;
using System;
using System.Data;
using System.Linq;
using Spetmall.Common;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Spetmall.DAL
{
    public class railcardDAL : DAL.BaseQuery
    {
        private static readonly railcardDAL Instance = new railcardDAL();

        private static readonly string getDatasSql = @"SELECT * FROM railcard WHERE 1=1 {0} {1}";


        private railcardDAL()
        {
            this.IsAddIntoCache = false;
            this.TableName = "railcard";
            this.ItemName = "优惠卡";
            this.OrderbyFields = "id desc";
        }

        /// <summary>
        /// 获取唯一实例
        /// </summary>
        /// <returns></returns>
        public static railcardDAL GetInstance()
        {
            return Instance;
        }

        public List<railcard> GetRailcards(string keyWord, string orderBy)
        {
            try
            {
                string where = GetWhere(keyWord);
                string orderbyString = GetOrderBy(orderBy);

                string sql = string.Format(getDatasSql, where, orderbyString);

                using (DBHelper dbHelper = new DBHelper(WebConfigData.DataBaseType, WebConfigData.ConnString))
                {
                    DataTable dt = dbHelper.ExecuteDataTable(sql);
                    if (dt != null && dt.Rows.Count > 0)
                        return dt.ToList<railcard>();
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "Getrailcards 获取优惠卡数据出错\r\n" + ex.Message);
            }
            return new List<railcard>();
        }

        private string GetWhere(string keyWord)
        {
            string where = string.Empty;
            if (!string.IsNullOrWhiteSpace(keyWord))
                where += $" and (name like'%{keyWord}%' or phone like'%{keyWord}%')";

            return where;
        }

        private string GetOrderBy(string orderBy)
        {
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                string orderby = string.Empty;
                switch (orderBy)
                {
                    case "money":
                        orderby = "order by money desc";
                        break;
                    case "lefttimes":
                        orderby = "order by lefttimes desc";
                        break;
                    default:
                        break;
                }
                return orderby;
            }
            return string.Empty;
        }

    }
}
