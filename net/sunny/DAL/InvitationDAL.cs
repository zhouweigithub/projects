using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.DAL
{
    public class InvitationDAL
    {
        /// <summary>
        /// 获取直接邀请人和间接邀请人
        /// </summary>
        private static readonly string selectInvitationUsersSql = @"SELECT a.student_id,a.from_student_id from1,b.from_student_id from2 FROM invitation a
LEFT JOIN invitation b ON a.from_student_id=b.student_id
WHERE a.state=0 AND a.student_id='{0}'";

        /// <summary>
        /// 返现
        /// </summary>
        private static readonly string updateStudentCashSql = @"UPDATE student SET cash=cash+{0} WHERE id='{1}';";
        /// <summary>
        /// 记录返现历史
        /// </summary>
        private static readonly string addCashHistorySql = "INSERT INTO cashback_history(student_id,from_student_id,money)VALUES('{0}','{1}','{2}');";


        /// <summary>
        /// 给邀请人发送现金奖励
        /// </summary>
        /// <param name="studentId">触发人id</param>
        public static void SendCashOfInvitation(int studentId)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    DataTable dt = dbhelper.ExecuteDataTable(string.Format(selectInvitationUsersSql, studentId));
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        int fromStudentId1 = Convert.ToInt32(dt.Rows[0]["from1"]);  //直接邀请人
                        int fromStudentId2 = Convert.ToInt32(dt.Rows[0]["from2"]);  //间接邀请人
                        double money1 = Math.Round(fromStudentId2 == 0 ? Common.WebConfigData.CashbackMoney : Common.WebConfigData.CashbackMoney * 0.9, 2);
                        double money2 = Math.Round(fromStudentId2 == 0 ? 0 : Common.WebConfigData.CashbackMoney * 0.1, 2);
                        if (money1 > 0)
                        {
                            dbhelper.ExecuteNonQuery(string.Format(updateStudentCashSql, money1, fromStudentId1));
                            dbhelper.ExecuteNonQuery(string.Format(addCashHistorySql, fromStudentId1, studentId, money1));
                        }
                        if (money2 > 0)
                        {
                            dbhelper.ExecuteNonQuery(string.Format(updateStudentCashSql, money2, fromStudentId2));
                            dbhelper.ExecuteNonQuery(string.Format(addCashHistorySql, fromStudentId2, studentId, money2));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("SendCashOfInvitation 出错：" + ex, Util.Log.LogType.Error);
            }

        }

    }
}
