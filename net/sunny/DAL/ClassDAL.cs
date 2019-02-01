using MySql.Data.MySqlClient;
using Sunny.Model;
using Sunny.Model.Custom;
using Sunny.Model.Response;
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
        private static readonly string getClassListOfCoachSql = @"
SELECT a.*,b.name course_name,b.main_img,b.summary,e.name venue_name,f.name campus_name,d.student_count FROM class a
INNER JOIN product b ON a.product_id=b.id
INNER JOIN (
	SELECT class_id,COUNT(1)student_count FROM class_student GROUP BY class_id
) d ON a.id=d.class_id
INNER JOIN venue e ON a.venue_id=e.id
INNER JOIN campus f ON e.campus_id=f.id
WHERE a.coach_id='{0}'
";

        private static readonly string getClassListOfStudentSql = @"
SELECT a.*,b.name course_name,b.main_img,b.summary,c.name coach_name,e.name venue_name,f.name campus_name,d.student_count FROM class a
INNER JOIN product b ON a.product_id=b.id
INNER JOIN coach c ON a.coach_id=c.id
INNER JOIN (
	SELECT class_id,COUNT(1)student_count FROM class_student WHERE student_id='{0}' GROUP BY class_id
) d ON a.id=d.class_id
INNER JOIN venue e ON a.venue_id=e.id
INNER JOIN campus f ON e.campus_id=f.id
";

        /// <summary>
        /// 获取上课后教练的评论
        /// </summary>
        private static readonly string getClassCommentListSql = @"
SELECT b.name,b.headimg,c.comment,d.images,d.videos,a.crtime FROM class a
INNER JOIN coach b ON a.coach_id=b.id
INNER JOIN class_comment c ON a.id=c.class_id
INNER JOIN (
	SELECT class_id,GROUP_CONCAT(IF(TYPE=0,url,NULL))images,GROUP_CONCAT(IF(TYPE=1,url,NULL))videos FROM class_comment_url GROUP BY class_id
) d ON a.id=d.class_id
WHERE 1=1 {0}
ORDER BY a.id DESC
limit {1}
";
        private static readonly string insertClassSql = @"INSERT IGNORE INTO class (product_id,coach_id,venue_id,hour,max_count,start_time,end_time,state,rate)
VALUES(@product_id,@coach_id,@venue_id,@hour,@max_count,@start_time,@end_time,0,0);";
        private static readonly string insertClassStudent = "INSERT INTO class_student (class_id,student_id,state) VALUES('{0}','{1}','0') ;";
        private static readonly string getClassIdOfCoachTime = "SELECT id FROM class WHERE coach_id={0} AND start_time='{1}'";


        /// <summary>
        /// 已预约满的时间段
        /// </summary>
        private static readonly string getBookingFullTimesListOfCoach = @"
SELECT start_time,end_time,max_count FROM class p
INNER JOIN class_student q ON p.id=q.class_id
WHERE coach_id='{0}' AND start_time>NOW() 
GROUP BY p.id HAVING max_count=COUNT(1)
";
        /// <summary>
        /// 没预约满的信息
        /// </summary>
        private static readonly string getBookingNotFullTimesListOfCoach = @"
SELECT p.venue_id,p.product_id,p.hour,p.start_time,p.end_time,p.max_count FROM class p
INNER JOIN class_student q ON p.id=q.class_id
WHERE coach_id='{0}' AND start_time>NOW() 
GROUP BY p.id HAVING max_count>COUNT(1)
";

        /// <summary>
        /// 学生上课历史
        /// </summary>
        private static readonly string getStudentClassHistorySql = @"
SELECT a.id class_id,a.coach_id,a.product_id,a.hour,a.start_time,a.end_time,a.state class_state,b.state student_state,c.hour total_hour,
d.name product_name,d.main_img,e.name venue_name,f.name campus_name,g.name coach_name,g.phone coach_phone FROM class a 
INNER JOIN class_student b ON a.id=b.class_id
INNER JOIN course c ON a.product_id=c.product_id AND b.student_id=c.student_id
INNER JOIN product d ON a.product_id=d.id
INNER JOIN venue e ON a.venue_id=e.id
INNER JOIN campus f ON e.campus_id=f.id
INNER JOIN coach g ON a.coach_id=g.id
WHERE b.student_id='{0}' ORDER BY a.crtime DESC";

        /// <summary>
        /// 教练上课历史
        /// </summary>
        private static readonly string getCoachClassHistorySql = @"
SELECT a.id class_id,a.coach_id,a.product_id,a.hour,h.hour total_hour,a.start_time,a.end_time,a.state class_state,
d.name product_name,d.main_img,e.name venue_name,f.name campus_name,g.name coach_name,g.phone coach_phone FROM class a 
LEFT JOIN product d ON a.product_id=d.id
LEFT JOIN venue e ON a.venue_id=e.id
LEFT JOIN campus f ON e.campus_id=f.id
LEFT JOIN coach g ON a.coach_id=g.id
LEFT JOIN hours h ON a.product_id=h.product_id
WHERE g.id='{0}' ORDER BY a.crtime DESC";
        #endregion

        /// <summary>
        /// 查询上次上课教练id
        /// </summary>
        private static readonly string getPreCoachOfCourseSql = "SELECT a.coach_id FROM class a INNER JOIN class_student b ON a.id=b.class_id WHERE a.product_id='{0}' ORDER BY a.crtime DESC LIMIT 1;";

        /// <summary>
        /// 获取上课的学生信息
        /// </summary>
        private static readonly string getStudentInfoByCoachIdSql = @"SELECT b.*,a.class_id FROM class_student a 
INNER JOIN student b ON a.student_id=b.id 
INNER JOIN class c ON a.class_id=c.id
WHERE c.id='{0}'
";


        /// <summary>
        /// 根据教练id获取课程列表
        /// </summary>
        /// <param name="coachId">教练id</param>
        /// <param name="state">课程状态</param>
        /// <returns></returns>
        public static List<ClassCoachJson> GetClassByCoachId(int coachId)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    DataTable dt = dbhelper.ExecuteDataTable(string.Format(getClassListOfCoachSql, coachId));

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.ToList<ClassCoachJson>();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("GetClassByCoachId 出错：" + ex, Util.Log.LogType.Error);
            }

            return new List<ClassCoachJson>();
        }

        /// <summary>
        /// 根据学员id获取课程列表
        /// </summary>
        /// <param name="studentId">学员id</param>
        /// <param name="state">课程状态</param>
        /// <returns></returns>
        public static List<ClassStudentJson> GetClassByStudentId(int studentId)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    DataTable dt = dbhelper.ExecuteDataTable(string.Format(getClassListOfStudentSql, studentId));

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.ToList<ClassStudentJson>();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("GetClassByStudentId 出错：" + ex, Util.Log.LogType.Error);
            }

            return new List<ClassStudentJson>();
        }

        /// <summary>
        /// 获取某次上课后教练给的评论
        /// </summary>
        /// <param name="classId">上课id</param>
        /// <param name="productId">课程id</param>
        /// <returns></returns>
        public static List<ClassCommentJson> GetClassCommentList(int classId, int productId, int limitCount)
        {
            try
            {
                string where = string.Empty;
                if (classId != 0)
                {
                    where += " and a.id = @classId";
                }
                if (productId != 0)
                {
                    where += " and a.product_id = @productId";
                }

                using (DBHelper dbhelper = new DBHelper())
                {
                    MySqlParameter[] commandParameters = new MySqlParameter[] {
                        new MySqlParameter("@classId", classId),
                        new MySqlParameter("@productId", productId),
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
        /// 插入课程和相应学生信息
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
                    int newId = dbhelper.ExecuteScalarInt(string.Format(getClassIdOfCoachTime, data.coach_id, data.start_time.ToString("yyyy-MM-dd HH")));
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

        /// <summary>
        /// 获取学生的上课历史
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public static List<StudentClassHistoryJson> GetStudentClassHistoryList(int studentId)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    DataTable dt = dbhelper.ExecuteDataTableParams(string.Format(getStudentClassHistorySql, studentId));

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.ToList<StudentClassHistoryJson>();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("GetStudentClassHistoryList 出错：" + ex, Util.Log.LogType.Error);
            }

            return new List<StudentClassHistoryJson>();
        }

        /// <summary>
        /// 获取教练的上课历史
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public static List<CoachClassHistoryJson> GetCoachClassHistoryList(int studentId)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    DataTable dt = dbhelper.ExecuteDataTableParams(string.Format(getCoachClassHistorySql, studentId));

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.ToList<CoachClassHistoryJson>();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("GetCoachClassHistoryList 出错：" + ex, Util.Log.LogType.Error);
            }

            return new List<CoachClassHistoryJson>();
        }

        /// <summary>
        /// 已预约满的时点点
        /// </summary>
        /// <param name="coachid">教练id</param>
        /// <returns></returns>
        public static List<CustBookingFullTimes> GetBookingFullTimesListOfCoach(int coachid)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    DataTable dt = dbhelper.ExecuteDataTable(string.Format(getBookingFullTimesListOfCoach, coachid));

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.ToList<CustBookingFullTimes>().Distinct().ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("GetBookingFullTimesListOfCoach 出错：" + ex, Util.Log.LogType.Error);
            }

            return new List<CustBookingFullTimes>();
        }

        /// <summary>
        /// 未预约满的信息
        /// </summary>
        /// <param name="coachid">教练id</param>
        /// <returns></returns>
        public static List<CustBookingNotFullTimesInfo> GetBookingNotFullTimesListOfCoach(int coachid)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    DataTable dt = dbhelper.ExecuteDataTable(string.Format(getBookingNotFullTimesListOfCoach, coachid));

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.ToList<CustBookingNotFullTimesInfo>();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("GetBookingNotFullTimesListOfCoach 出错：" + ex, Util.Log.LogType.Error);
            }

            return new List<CustBookingNotFullTimesInfo>();
        }

        /// <summary>
        /// 获取上课的学生信息
        /// </summary>
        /// <param name="coachId"></param>
        /// <returns></returns>
        public static List<CoachClassStudentJson> GetStudentInfoByCoachId(int coachId)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    DataTable dt = dbhelper.ExecuteDataTable(string.Format(getStudentInfoByCoachIdSql, coachId));

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.ToList<CoachClassStudentJson>();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("GetStudentInfoByCoachId 出错：" + ex, Util.Log.LogType.Error);
            }

            return new List<CoachClassStudentJson>();
        }



    }
}
