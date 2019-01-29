using Sunny.Common;
using Sunny.Model;
using Sunny.Model.Custom;
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
        /// 取该用户直接和间接邀请的总人数
        /// </summary>
        private static readonly string selectInvitatedCountSql = @"SELECT COUNT(1)COUNT FROM invitation a
LEFT JOIN invitation b ON a.from_student_id=b.student_id
WHERE a.from_student_id={0} OR b.from_student_id={0}";

        /// <summary>
        /// 获取未发放奖励的直接邀请人和间接邀请人
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public static CustInvationUserInfo GetInvitationUsers(int studentId)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    DataTable dt = dbhelper.ExecuteDataTable(string.Format(selectInvitationUsersSql, studentId));

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.ToList<CustInvationUserInfo>().First();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("GetInvitationUsers 出错：" + ex, Util.Log.LogType.Error);
            }

            return null;
        }

        /// <summary>
        /// 取该用户直接和间接邀请的总人数
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public static int GetInvitatedCount(int studentId)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    int dt = dbhelper.ExecuteScalarInt(string.Format(selectInvitatedCountSql, studentId));
                    return dt;
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("GetInvitatedCount 出错：" + ex, Util.Log.LogType.Error);
            }

            return 0;
        }
    }
}
