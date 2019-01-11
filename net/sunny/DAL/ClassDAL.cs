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
INNER JOIN product b ON a.product_id=b.id
INNER JOIN class_student d ON a.id=d.class_id
WHERE a.coach_id='{0}' and a.state='{1}'";

        private static readonly string getClassListOfStudentSql = @"SELECT a.*,b.name courseName,b.summary,c.name coachName,d.studentCount FROM class a
INNER JOIN product b ON a.product_id=b.id
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
GROUP BY a.id desc
ORDER BY a.crtime DESC
limit {1}
";
        private static readonly string insertClassSql = @"INSERT IGNORE INTO class (product_id,coach_id,venue_id,hour,max_count,start_time,end_time,state,rate)
VALUES(@product_id,@coach_id,@venue_id,@hour,@max_count,@start_time,@end_time,0,0);";

        /// <summary>
        /// 获取教练可接单的预约信息
        /// </summary>
        private static readonly string getBookingListOfCoach = @"
SELECT a.id classid,a.hour,a.max_count,a.start_time,a.end_time,b.name course_name,COUNT(c.student_id)booked_count FROM class a
INNER JOIN product b ON a.product_id=b.id
INNER JOIN coachcaption_venue d ON a.venue_id=d.venue_id
INNER JOIN coach_caption e ON d.coach_id=e.caption_id
LEFT JOIN class_student c ON a.id=c.class_id 
WHERE a.start_time>NOW() AND a.coach_id=0 AND (ISNULL(c.state) OR c.state=0) AND e.coach_id=1
GROUP BY classid
HAVING a.max_count>COUNT(c.student_id)

SELECT a.id classid,a.hour,a.max_count,a.start_time,a.end_time,b.name course_name,f.student_id FROM class a
INNER JOIN product b ON a.product_id=b.id
INNER JOIN coachcaption_venue d ON a.venue_id=d.venue_id
INNER JOIN coach_caption e ON d.coach_id=e.caption_id
INNER JOIN class_student f ON a.id=f.class_id AND f.state=0 
INNER JOIN course g ON f.student_id=g.student_id AND a.product_id=g.product_id  AND d.venue_id=g.venue_id
WHERE a.start_time>NOW() AND (a.coach_id=0 OR a.coach_id=1) AND e.coach_id=1

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
        public static List<ClassCommentJson> GetClassCommentList(int classId, int courseId, int limitCount)
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
                    where += " and a.product_id = @courseId";
                }

                using (DBHelper dbhelper = new DBHelper())
                {
                    MySqlParameter[] commandParameters = new MySqlParameter[] {
                        new MySqlParameter("@classId", classId),
                        new MySqlParameter("@courseId", courseId),
                    };

                    DataTable dt = dbhelper.ExecuteDataTableParams(string.Format(getClassCommentListSql, where, limitCount), commandParameters);

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

        public static bool InsertClassData(Class data)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    MySqlParameter[] paras = new MySqlParameter[] {
                        new MySqlParameter("@product_id",data.product_id),
                        new MySqlParameter("@coach_id",data.coach_id),
                        new MySqlParameter("@venue_id",data.venue_id),
                        new MySqlParameter("@hour",data.hour),
                        new MySqlParameter("@max_count",data.max_count),
                        new MySqlParameter("@start_time",data.start_time),
                        new MySqlParameter("@end_time",data.end_time),
                    };

                    int count = dbhelper.ExecuteNonQueryParams(insertClassSql, paras);
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("InsertClassData 出错：" + ex, Util.Log.LogType.Error);
            }

            return false;
        }

        /// <summary>
        /// 获取教练可接单的预约信息
        /// </summary>
        /// <param name="coachId">教练id</param>
        /// <returns></returns>
        public static List<ClassBookingOfCoach> GetBookingListOfCoach(int coachId)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    DataTable dt = dbhelper.ExecuteDataTableParams(string.Format(getBookingListOfCoach, coachId));

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.ToList<ClassBookingOfCoach>();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("GetBookingListOfCoach 出错：" + ex, Util.Log.LogType.Error);
            }

            return new List<ClassBookingOfCoach>();
        }



    }
}
