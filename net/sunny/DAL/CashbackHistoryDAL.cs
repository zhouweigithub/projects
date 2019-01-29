using Sunny.Model.Response;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.DAL
{
    public class CashbackHistoryDAL
    {

        /// <summary>
        /// 返现总金额
        /// </summary>
        private static readonly string getTotalBackMoney = @"SELECT SUM(money)money FROM cashback_history WHERE student_id={0};";
        /// <summary>
        /// 各天邀请返现记录
        /// </summary>
        private static readonly string selectInvitationUsersSql = @"SELECT DATE_FORMAT(crtime,'%Y-%m-%d')crdate,SUM(money)money,COUNT(1)COUNT FROM cashback_history WHERE student_id={0} GROUP BY crdate;";

        /// <summary>
        /// 取邀请返现记录
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public static List<CashbackHistoryItem> GetCashbackHistoryList(int studentId)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    DataTable dt = dbhelper.ExecuteDataTable(string.Format(selectInvitationUsersSql, studentId));

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.ToList<CashbackHistoryItem>();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("GetCashbackHistoryList 出错：" + ex, Util.Log.LogType.Error);
            }

            return new List<CashbackHistoryItem>();
        }

        public static decimal GetTotalBackMoney(int studentId)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    object money = dbhelper.ExecuteScalar(string.Format(getTotalBackMoney, studentId));
                    return Convert.ToDecimal(money);
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("GetTotalBackMoney 出错：" + ex, Util.Log.LogType.Error);
            }

            return 0;
        }

    }
}
