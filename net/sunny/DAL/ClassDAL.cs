using MySql.Data.MySqlClient;
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
    /// <summary>
    /// 上课信息操作类
    /// </summary>
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
        /// 获取上课后教练的评论
        /// </summary>
        private static readonly string getClassCommentListSql = @"SELECT b.name,b.headimg,c.comment,GROUP_CONCAT(d.url)urls,a.crtime FROM class a
INNER JOIN coach b ON a.coach_id=b.id
INNER JOIN class_comment c ON a.id=c.class_id
INNER JOIN class_comment_url d ON a.id=d.class_id
WHERE 1=1 {0}
GROUP BY a.id
ORDER BY a.crtime DESC
";


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

        /// <summary>
        /// 获取某次上课后教练给的评论
        /// </summary>
        /// <param name="classId">上课id</param>
        /// <param name="courseId">课程id</param>
        /// <returns></returns>
        public static List<ClassCommentJson> GetClassCommentList(int classId, int courseId)
        {
            try
            {
                string where = string.Empty;
                if (classId != 0)
                {
                    where += " and a.id = @classId";
                }
                if (courseId != 0)
                {
                    where += " and a.course_id = @courseId";
                }

                using (DBHelper dbhelper = new DBHelper())
                {
                    MySqlParameter[] commandParameters = new MySqlParameter[] {
                        new MySqlParameter("@classId", classId),
                        new MySqlParameter("@courseId", courseId),
                    };

                    DataTable dt = dbhelper.ExecuteDataTableParams(string.Format(getClassCommentListSql, where), commandParameters);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.ToList<ClassCommentJson>();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("GetClassCommentList 出错：" + ex, Util.Log.LogType.Error);
            }

            return new List<ClassCommentJson>();
        }

    }
}
