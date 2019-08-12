using Spetmall.Model;
using System;
using System.Data;
using System.Linq;
using Spetmall.Common;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Spetmall.DAL
{

    /// <summary>
    /// 资金流水
    /// </summary>
    public class financialflowDAL : DAL.BaseQuery
    {
        private static readonly financialflowDAL Instance = new financialflowDAL();

        private static readonly string getDatasSql = "SELECT * FROM financial_flow WHERE 1=1 {0} {1}";
        private static readonly string getDatasTotalSql = "SELECT sum(money)money FROM ({0})t";


        private financialflowDAL()
        {
            this.IsAddIntoCache = false;
            this.TableName = "financial_flow";
            this.ItemName = "资金流水";
            this.OrderbyFields = "id DESC";
        }

        /// <summary>
        /// 获取唯一实例
        /// </summary>
        /// <returns></returns>
        public static financialflowDAL GetInstance()
        {
            return Instance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyWord">备注</param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <param name="type">0无效 -1支出 1收入</param>
        /// <param name="orderBy"></param>
        /// <param name="pageSize"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<financial_flow> GetFinancialFlows(string keyWord, string startdate, string enddate, string type, string orderBy, int pageSize = 20, int page = 1)
        {
            try
            {
                string where = GetWhere(keyWord, startdate, enddate, type);
                string orderbyString = GetOrderBy(orderBy);

                string sql = string.Format(getDatasSql, where, orderbyString);

                using (DBHelper dbHelper = new DBHelper(WebConfigData.DataBaseType, WebConfigData.ConnString))
                {
                    DataTable dt = dbHelper.ExecuteDataTablePage(sql, pageSize, page);
                    if (dt != null && dt.Rows.Count > 0)
                        return dt.ToList<financial_flow>();
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "GetFinancialFlows 获取资金流数据出错\r\n" + ex.Message);
            }
            return new List<financial_flow>();
        }

        public financial_flow GetFinancialFlowsTotal(string keyWord, string startdate, string enddate, string type)
        {
            try
            {
                string where = GetWhere(keyWord, startdate, enddate, type);
                string sql = string.Format(getDatasTotalSql, string.Format(getDatasSql, where, string.Empty));

                using (DBHelper dbHelper = new DBHelper(WebConfigData.DataBaseType, WebConfigData.ConnString))
                {
                    DataTable dt = dbHelper.ExecuteDataTable(sql);
                    if (dt != null && dt.Rows.Count > 0)
                        return dt.ToList<financial_flow>()[0];
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "GetFinancialFlowsTotal 获取汇总数据出错\r\n" + ex.Message);
            }
            return null;
        }


        public int GetFinancialFlowsCount(string keyWord, string startdate, string enddate, string type)
        {
            try
            {
                string where = GetWhere(keyWord, startdate, enddate, type);
                string sqldata = string.Format(getDatasSql, where, string.Empty);

                using (DBHelper dbHelper = new DBHelper(WebConfigData.DataBaseType, WebConfigData.ConnString))
                {
                    string sql = string.Format("select count(1) from ({0})t", sqldata);
                    int count = dbHelper.ExecuteScalarInt(sql);
                    return count;
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "GetFinancialFlowsCount 获取数量出错\r\n" + ex.Message);
            }
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyWord"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <param name="type">0无效 -1支出 1收入</param>
        /// <returns></returns>
        private string GetWhere(string keyWord, string startdate, string enddate, string type)
        {
            string where = string.Empty;
            if (!string.IsNullOrWhiteSpace(keyWord))
                where += $" and remark like'%{keyWord}%'";
            if (!string.IsNullOrWhiteSpace(startdate))
                where += $" and `date`>='{startdate}'";
            if (!string.IsNullOrWhiteSpace(enddate))
                where += $" and `date`<='{enddate}'";
            if (!string.IsNullOrWhiteSpace(type))
                where += $" and `type`={type}";

            return where;
        }

        private string GetOrderBy(string orderBy)
        {
            string orderby = string.Empty;

            if (!string.IsNullOrWhiteSpace(orderBy))
                orderby = $"order by {orderBy} desc";
            else
                orderby = $"order by id desc";

            return orderby;
        }

    }
}
