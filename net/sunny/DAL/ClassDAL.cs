using MySql.Data.MySqlClient;
using Sunny.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.DAL
{
    public class ClassDAL
    {
        /// <summary>
        /// 获取教练看到的上课信息
        /// </summary>
        private static readonly string getClassListOfCoachSql = @"SELECT a.*,b.name courseName,b.summary,COUNT(1)studentCount FROM class a
INNER JOIN course b ON a.course_id=b.id
INNER JOIN class_student d ON a.id=d.class_id
WHERE a.coach_id='{0}' and a.state='{1}'";

        private static readonly string getClassListOfStudentSql = @"SELECT a.*,b.name courseName,b.summary,c.name coachName,d.studentCount FROM class a
INNER JOIN course b ON a.course_id=b.id
INNER JOIN coach c ON a.coach_id=c.id
INNER JOIN (
	SELECT class_id,COUNT(1)studentCount FROM class_student WHERE student_id='{0}' and state='{1}' GROUP BY class_id
) d ON a.id=d.class_id";

        /// <summary>
        /// 根据教练id获取课程列表
        /// </summary>
        /// <param name="coachId">教练id</param>
        /// <param name="state">课程状态</param>
        /// <returns></returns>
        public static List<Class> GetClassByCoachId(int coachId, short state)
        {
            try
            {
                //string where = GetWhereString(studentId, coachId, state);
                using (DBHelper dbhelper = new DBHelper())
                {
                    //MySqlParameter[] commandParameters = new MySqlParameter[] {
                    //    new MySqlParameter("@studentId", studentId),
                    //    new MySqlParameter("@coachId", coachId),
                    //    new MySqlParameter("@state", state),
                    //};

                    DataTable dt = dbhelper.ExecuteDataTable(string.Format(getClassListOfCoachSql, coachId, state));

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.ToList<Class>();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("GetClassByCoachId 出错：" + ex, Util.Log.LogType.Error);
            }

            return new List<Class>();
        }

        /// <summary>
        /// 根据学员id获取课程列表
        /// </summary>
        /// <param name="studentId">学员id</param>
        /// <param name="state">课程状态</param>
        /// <returns></returns>
        public static List<Class> GetClassByStudentId(int studentId, short state)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    DataTable dt = dbhelper.ExecuteDataTable(string.Format(getClassListOfStudentSql, studentId, state));

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.ToList<Class>();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("GetClassByStudentId 出错：" + ex, Util.Log.LogType.Error);
            }

            return new List<Class>();
        }

    }
}
