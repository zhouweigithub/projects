using Spetmall.Model;
using System;
using System.Data;
using System.Linq;
using Spetmall.Common;
using MySql.Data.MySqlClient;
using Spetmall.Model.Page;

namespace Spetmall.DAL
{
    public class fullsendDAL : DAL.BaseQuery
    {
        private static readonly fullsendDAL Instance = new fullsendDAL();

        private static readonly string insertDiscountSql = @"INSERT INTO spetmall.fullsend (`name`,`type`,starttime,endtime,state
) VALUES('{0}','{1}','{2}','{3}','{4}');";
        private static readonly string insertProductOrCategorySql = @"INSERT INTO spetmall.saleproduct (`type`,ptype,saleid,productid
) VALUES('{0}','{1}','{2}','{3}');";
        private static readonly string insertRuleSql = @"INSERT INTO spetmall.salerule ( saleid, aim, sale
) VALUES('{0}','{1}','{2}','{3}');";
        private static readonly string getLastInsertIdSql = "SELECT LAST_INSERT_ID()";


        private fullsendDAL()
        {
            this.IsAddIntoCache = false;
            this.TableName = "fullsend";
            this.ItemName = "满就送活动";
            this.OrderbyFields = "id desc";
        }

        /// <summary>
        /// 获取唯一实例
        /// </summary>
        /// <returns></returns>
        public static fullsendDAL GetInstance()
        {
            return Instance;
        }

        public static bool InsertData(fullsend_post data)
        {
            try
            {
                using (DBHelper dbHelper = new DBHelper(WebConfigData.DataBaseType, WebConfigData.ConnString))
                {
                    string sql = string.Format(insertDiscountSql, data.name, data.type,
                        data.starttime.ToString("yyyy-MM-dd"), data.endtime.ToString("yyyy-MM-dd"), data.state);
                    dbHelper.ExecuteNonQuery(sql);

                    //获取折扣自增id
                    int saleId = dbHelper.ExecuteScalarInt(getLastInsertIdSql);

                    InsertExtraDatas(data, saleId);

                    return true;
                }
            }
            catch (Exception e)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "fullsendDAL.InsertData 插入限时折扣数据失败\r\n" + e);
            }

            return false;
        }

        /// <summary>
        /// 添加折扣活动相关的商品和规则信息
        /// </summary>
        /// <param name="data"></param>
        /// <param name="saleId"></param>
        public static void InsertExtraDatas(fullsend_post data, int saleId)
        {
            using (DBHelper dbHelper = new DBHelper(WebConfigData.DataBaseType, WebConfigData.ConnString))
            {
                //添加折扣的分类或商品信息
                if (data.type == 1 || data.type == 2)
                {
                    string[] tmpIds = data.categoryOrProducts.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string tmpid in tmpIds)
                    {
                        string sql = string.Format(insertProductOrCategorySql, 1, data.type == 1 ? 1 : 0, saleId, tmpid);
                        dbHelper.ExecuteNonQuery(sql);
                    }
                }

                //添加折扣的规则信息
                string[] rules = data.rules.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string rule in rules)
                {
                    string[] para = rule.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    if (para.Length == 2)
                    {
                        string sql = string.Format(insertRuleSql, saleId, para[0], para[1]);
                        dbHelper.ExecuteNonQuery(sql);
                    }
                    else
                    {
                        Util.Log.LogUtil.Write("折扣规则参数不正确：" + rule, Util.Log.LogType.Error);
                    }
                }
            }
        }

    }
}
