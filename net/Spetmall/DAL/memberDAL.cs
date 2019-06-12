using Spetmall.Model;
using System;
using System.Data;
using System.Linq;
using Spetmall.Common;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using Spetmall.Model.Page;

namespace Spetmall.DAL
{
    public class memberDAL : DAL.BaseQuery
    {
        private static readonly memberDAL Instance = new memberDAL();

        private static readonly string getDatasSql = @"
SELECT a.*,IFNULL(b.recharge,0)recharge,IFNULL(c.order_count,0)order_count,IFNULL(d.pets,'')pets FROM member a
LEFT JOIN (
	SELECT memberid,IFNULL(SUM(money),0)recharge FROM recharge GROUP BY memberid
)b ON a.id=b.memberid
LEFT JOIN (
	SELECT memberid,COUNT(1)order_count FROM `order` GROUP BY memberid
)c ON a.id=c.memberid 
LEFT JOIN (
	SELECT memberid,GROUP_CONCAT(b.name)pets FROM memberpet a
	INNER JOIN pet b ON a.petid=b.id
	GROUP BY a.memberid
)d ON a.id=d.memberid
WHERE 1=1 {0} {1}";

        private static readonly string getEditSql = @"
SELECT a.*,b.pets FROM member a
LEFT JOIN (
	SELECT memberid,GROUP_CONCAT(petid)pets FROM memberpet GROUP BY memberid
) b ON a.id=b.memberid
WHERE a.id='{0}'
";

        private memberDAL()
        {
            this.IsAddIntoCache = false;
            this.TableName = "member";
            this.ItemName = "会员信息";
            this.OrderbyFields = "id asc";
        }

        /// <summary>
        /// 获取唯一实例
        /// </summary>
        /// <returns></returns>
        public static memberDAL GetInstance()
        {
            return Instance;
        }

        public List<member_show> GetMembers(string keyWord, string orderBy)
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
                        return dt.ToList<member_show>();
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "GetMembers 获取会员数据出错\r\n" + ex.Message);
            }
            return new List<member_show>();
        }

        private string GetWhere(string keyWord)
        {
            string where = string.Empty;
            if (!string.IsNullOrWhiteSpace(keyWord))
                where += $" and (a.name like'%{keyWord}%' or a.phone like'%{keyWord}%')";

            return where;
        }

        private string GetOrderBy(string orderBy)
        {
            string orderby = string.Empty;
            switch (orderBy)
            {
                case "money":
                    orderby = "order by a.money desc";
                    break;
                case "recharge":
                    orderby = "order by b.recharge desc";
                    break;
                default:
                    orderby = "order by a.id desc";
                    break;
            }
            return orderby;
        }

        //edit
        public member_edit GetEditInfo(int id)
        {
            try
            {
                string sql = string.Format(getEditSql, id);

                using (DBHelper dbHelper = new DBHelper(WebConfigData.DataBaseType, WebConfigData.ConnString))
                {
                    DataTable dt = dbHelper.ExecuteDataTable(sql);
                    if (dt != null && dt.Rows.Count > 0)
                        return dt.ToList<member_edit>().First();
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "GetEditInfo 获取会员数据出错\r\n" + ex.Message);
            }

            return null;
        }

    }
}
