using Spetmall.Model;
using System;
using System.Data;
using System.Linq;
using Spetmall.Common;
using MySql.Data.MySqlClient;
using Spetmall.Model.Page;
using System.Collections.Generic;

namespace Spetmall.DAL
{
    public class fullsendDAL : DAL.BaseQuery
    {
        private static readonly fullsendDAL Instance = new fullsendDAL();

        private static readonly string insertFullsendSql = @"INSERT INTO fullsend (`name`,`type`,starttime,endtime,state
) VALUES('{0}','{1}','{2}','{3}','{4}');";
        private static readonly string insertProductOrCategorySql = @"INSERT INTO saleproduct (`type`,ptype,saleid,productid
) VALUES('{0}','{1}','{2}','{3}');";
        private static readonly string insertRuleSql = @"INSERT INTO salerule ( saleid, aim, sale
) VALUES('{0}','{1}','{2}');";
        private static readonly string getLastInsertIdSql = "SELECT LAST_INSERT_ID()";
        /// <summary>
        /// 查询店铺级活动是否存在时间 交叉
        /// </summary>
        private static readonly string getIntersectDateShopSql = "SELECT MAX(endtime)endtime FROM fullsend WHERE `type`=0 AND starttime<='{0}' AND endtime>='{1}' {2}";
        /// <summary>
        /// 查询商品或分类活动是否存在时间交叉
        /// </summary>
        private static readonly string getIntersectDateNotShopSql = @"
SELECT b.productid,MAX(a.endtime)endtime FROM fullsend a
INNER JOIN saleproduct b ON a.id=b.saleid
WHERE a.`type`={0} AND b.productid IN({1}) AND a.starttime<='{2}' AND a.endtime>='{3}' {4}
GROUP BY b.productid
";


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
                    string sql = string.Format(insertFullsendSql, data.name, data.type,
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

        /// <summary>
        /// 获取店铺活动时间交叉点
        /// </summary>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public static string GetIntersectDateShop(string startdate, string enddate, int currentActiveid)
        {
            try
            {
                using (DBHelper dbHelper = new DBHelper(WebConfigData.DataBaseType, WebConfigData.ConnString))
                {
                    string where = currentActiveid > 0 ? $" AND id<>{currentActiveid}" : "";
                    object endtime = dbHelper.ExecuteScalar(string.Format(getIntersectDateShopSql, enddate, startdate, where));
                    if (endtime != null)
                        return Convert.ToDateTime(endtime).ToString("yyyy-MM-dd");
                    else
                        return null;
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "GetIntersectDateShop 获取店铺活动时间交叉点出错\r\n" + ex);
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取非店铺活动时间交叉点
        /// </summary>
        /// <param name="type">1按分类 2按商品</param>
        /// <param name="productids"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public static List<active_date> GetIntersectDateNotShop(short type, string productids, string startdate, string enddate, int currentActiveid)
        {
            try
            {
                using (DBHelper dbHelper = new DBHelper(WebConfigData.DataBaseType, WebConfigData.ConnString))
                {
                    string where = currentActiveid > 0 ? $" AND a.id<>{currentActiveid}" : "";
                    DataTable dt = dbHelper.ExecuteDataTable(string.Format(getIntersectDateNotShopSql, type, productids, enddate, startdate, where));
                    if (dt.Rows.Count > 0)
                    {
                        return dt.ToList<active_date>();
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "GetIntersectDateNotShop 获取非店铺活动时间交叉点出错\r\n" + ex);
            }
            return new List<active_date>();
        }


    }
}
