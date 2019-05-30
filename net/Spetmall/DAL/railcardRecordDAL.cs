using Spetmall.Model;
using System;
using System.Data;
using System.Linq;
using Spetmall.Common;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Spetmall.DAL
{
    public class railcardRecordDAL : DAL.BaseQuery
    {
        private static readonly railcardRecordDAL Instance = new railcardRecordDAL();

        private static readonly string getRecordSql = @"
SELECT a.* FROM railcard_record a
INNER JOIN railcard_record b ON a.railcardid=b.id
WHERE 1=1 {0}
ORDER BY a.id DESC
";

        private railcardRecordDAL()
        {
            this.IsAddIntoCache = false;
            this.TableName = "railcard_record";
            this.ItemName = "优惠卡使用记录";
            this.OrderbyFields = "id desc";
        }

        /// <summary>
        /// 获取唯一实例
        /// </summary>
        /// <returns></returns>
        public static railcardRecordDAL GetInstance()
        {
            return Instance;
        }

        public List<railcard_record> GetRailcardRecords(int railcardid, string keyWord)
        {
            try
            {
                string where = GetWhere(railcardid, keyWord);
                string sql = string.Format(getRecordSql, where);
                using (DBHelper dbHelper = new DBHelper(WebConfigData.DataBaseType, WebConfigData.ConnString))
                {
                    DataTable dt = dbHelper.ExecuteDataTable(sql);
                    if (dt != null && dt.Rows.Count > 0)
                        return dt.ToList<railcard_record>();
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "GetRailcardRecords 获取优惠卡使用记录出错\r\n" + ex.Message);
            }
            return new List<railcard_record>();
        }

        private string GetWhere(int railcard, string keyWord)
        {
            string where = string.Empty;
            if (railcard != 0)
                where += $" and a.railcardid={railcard}";
            else if (!string.IsNullOrWhiteSpace(keyWord))
                where += $" b.name LIKE '%{keyWord}%' OR b.phone LIKE '%{keyWord}%'";

            return where;
        }

    }
}
