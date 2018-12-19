using Moqikaka.Tmp.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Moqikaka.Tmp.DAL
{
    /// <summary>
    /// 用户信息BLL
    /// </summary>
    public class MArticalDAL
    {

        private static readonly string getArticalsSql = "SELECT id, title, year, area, type  FROM m_artical WHERE 1=1 {0}";
        private static readonly string getArticalRandomSql = "SELECT id,title,YEAR,AREA FROM m_artical ORDER BY RAND() LIMIT 3";

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="keyWord"></param>
        /// <param name="year"></param>
        /// <param name="area"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static List<MArticalBase> GetArticals(string keyword, int year, string area, int page, int pageSize)
        {
            try
            {
                string where = GetWhereString(keyword, year, area);
                using (DBHelper dbhelper = new DBHelper())
                {
                    MySqlParameter[] commandParameters = new MySqlParameter[] {
                        new MySqlParameter("@keyword",$"%{keyword}%"),
                        new MySqlParameter("@keywordArea",keyword),
                        new MySqlParameter("@year",year),
                        new MySqlParameter("@area",area),
                    };

                    DataTable dt = dbhelper.ExecuteDataTablePageParams(string.Format(getArticalsSql, where), pageSize, page, commandParameters);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.ToList<MArticalBase>();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("GetArticals 出错：" + ex, Util.Log.LogType.Error);
            }

            return new List<MArticalBase>();
        }

        /// <summary>
        /// 获取数据量
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="year"></param>
        /// <param name="area"></param>
        /// <returns></returns>
        public static int GetArticalsCount(string keyword, int year, string area)
        {
            try
            {
                string sql = "SELECT COUNT(1) FROM ( {0} )a";
                string where = GetWhereString(keyword, year, area);
                using (DBHelper dbhelper = new DBHelper())
                {
                    MySqlParameter[] commandParameters = new MySqlParameter[] {
                        new MySqlParameter("@keyword", $"%{keyword}%"),
                        new MySqlParameter("@keywordArea",keyword),
                        new MySqlParameter("@area", area),
                        new MySqlParameter("@year", year),
                    };
                    string dataSql = string.Format(getArticalsSql, where);
                    int count = dbhelper.ExecuteScalarIntParams(string.Format(sql, dataSql), commandParameters);
                    return count;
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("GetArticalsCount 出错：" + ex, Util.Log.LogType.Error);
            }

            return 0;
        }

        /// <summary>
        /// 获取过滤条件
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="year"></param>
        /// <param name="area"></param>
        /// <returns></returns>
        private static string GetWhereString(string keyword, int year, string area)
        {
            string where = string.Empty;
            if (!string.IsNullOrWhiteSpace(keyword))
                where += $" and (title like @keyword or area = @keywordArea)";
            if (!string.IsNullOrWhiteSpace(area))
                where += $" and area = @area";
            if (year != 0)
                where += $" and year = @year";

            return where;
        }

        /// <summary>
        /// 随机取几条数据
        /// </summary>
        /// <returns></returns>
        public static List<MArticalBase> GetArticalsRandom()
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    DataTable dt = dbhelper.ExecuteDataTable(getArticalRandomSql);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.ToList<MArticalBase>();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("GetArticalsRandom 出错：" + ex, Util.Log.LogType.Error);
            }

            return new List<MArticalBase>();
        }


    }
}
