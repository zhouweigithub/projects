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
        #region fiels
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
        private static readonly string getLastInsertId = "SELECT LAST_INSERT_ID();";
        private static readonly string insertClassStudent = "INSERT INTO class_student (class_id,student_id,state) VALUES('{0}','{1}','0') ;";

        /// <summary>
        /// 获取教练可接单的预约信息
        /// </summary>
        private static readonly string getBookingListOfCoach = @"
SELECT a.id booking_id,a.start_time,a.end_time,b.over_hour+1 `hour`,b.max_count,b.over_hour,b.product_id,
c.name product_name,c.main_img,k.name venue_name FROM booking_student a
INNER JOIN course b ON a.course_id=b.id
INNER JOIN product c ON b.product_id=c.id
INNER JOIN coachcaption_venue d ON b.venue_id=d.venue_id
INNER JOIN coach_caption e ON d.coach_id=e.caption_id AND b.venue_id=d.venue_id
INNER JOIN venue k ON b.venue_id=k.id
LEFT JOIN booking_coach_queue f ON a.id=f.booking_student_id AND f.end_time>NOW()
INNER JOIN (
	-- 获取上课时间未到，并且上课学员已预约满的时间段
	SELECT start_time,end_time,max_count FROM class p
	INNER JOIN class_student q ON p.id=q.class_id
	WHERE coach_id='{0}' AND start_time>NOW() 
	GROUP BY p.id HAVING max_count=COUNT(1)
)g ON NOT(a.start_time>=g.end_time OR a.end_time<=g.start_time) -- 排除时间交叉同时已预约完成的时间段
INNER JOIN (
	-- 获取上课时间未到，并且上课学员未预约满的数据
	SELECT p.venue_id,p.product_id,p.hour,p.start_time,p.end_time,p.max_count FROM class p
	INNER JOIN class_student q ON p.id=q.class_id
	WHERE coach_id='{0}' AND start_time>NOW() 
	GROUP BY p.id HAVING max_count>COUNT(1)
)h ON a.start_time=h.start_time AND a.end_time=h.end_time 
    AND NOT(b.venue_id=h.venue_id AND b.product_id=h.product_id AND b.over_hour+1=h.hour) -- 排除时间段相同，但内容不同的项

WHERE a.start_time>NOW() AND a.state=0 AND e.coach_id='{0}' AND (ISNULL(f.coach_id) OR f.coach_id=e.coach_id)
";
        #endregion

        /// <summary>
        /// 查询上次上课教练id
        /// </summary>
        private static readonly string getPreCoachOfCourseSql = "SELECT a.coach_id FROM class a INNER JOIN class_student b ON a.id=b.class_id WHERE a.product_id='{0}' ORDER BY a.crtime DESC LIMIT 1;";

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

        /// <summary>
        /// 插入课程信息
        /// </summary>
        /// <param name="studentId">学员id</param>
        /// <param name="data">课程详情</param>
        /// <returns></returns>
        public static bool InsertClassData(int studentId, Class data)
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

                    //插入Class表数据
                    int count = dbhelper.ExecuteNonQueryParams(insertClassSql, paras);
                    //获取刚插入的id
                    int newId = dbhelper.ExecuteScalarInt(getLastInsertId);
                    //向课程对应的学生表里插入数据 
                    int countStudent = dbhelper.ExecuteNonQuery(string.Format(insertClassStudent, newId, studentId));

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

        /// <summary>
        /// 查询上次上课教练id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static int GetPreCoachOfCourse(int productId)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    int coachId = dbhelper.ExecuteScalarInt(string.Format(getPreCoachOfCourseSql, productId));
                    return coachId;
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("GetPreCoachOfCourse 出错：" + ex, Util.Log.LogType.Error);
            }

            return 0;
        }


    }
}
