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
    public class AppointmentDAL
    {
        private static readonly string getAppointmentInfoSql = @"
SELECT a.id courseid,b.name course_name,b.main_img,a.hour,a.over_hour,a.max_count,c.name venue_name,
d.name campus_name,g.name coach_name,g.phone coach_phone,e.start_time,e.end_time,
IF(a.hour=a.over_hour,'已学完',IF(ISNULL(f.student_id) AND ISNULL(h.course_id),'可预约',IF(h.course_id IS NOT NULL,'预约中',IF(f.student_id IS NOT NULL,'已预约','未知'))))state
FROM course a
INNER JOIN product b ON a.product_id=b.id
INNER JOIN venue c ON a.venue_id=c.id
INNER JOIN campus d ON c.campus_id=d.id
LEFT JOIN class e ON e.state=0 AND e.product_id=a.product_id
LEFT JOIN class_student f ON a.student_id=f.student_id AND e.id=f.class_id
LEFT JOIN coach g ON e.coach_id=g.id
LEFT JOIN booking_student h ON a.id=h.course_id AND h.state=0
WHERE 1=1 {0}
GROUP BY a.id
";

        /// <summary>
        /// 教练相关的所有预约请求
        /// </summary>
        private static readonly string getBookingListOfCoach = @"
SELECT a.id booking_id,a.start_time,a.end_time,b.over_hour+1 `hour`,b.max_count,b.over_hour,b.product_id,
c.name product_name,c.main_img,k.name venue_name,f.coach_id,k.id venue_id FROM booking_student a
INNER JOIN course b ON a.course_id=b.id
INNER JOIN product c ON b.product_id=c.id
INNER JOIN coachcaption_venue d ON b.venue_id=d.venue_id
INNER JOIN coach_caption e ON d.coach_id=e.caption_id AND b.venue_id=d.venue_id
INNER JOIN venue k ON b.venue_id=k.id
LEFT JOIN booking_coach_queue f ON b.id=f.course_id AND f.end_time>NOW()
WHERE a.start_time>NOW() AND a.state=0 AND e.coach_id='{0}' AND (ISNULL(f.coach_id) OR f.coach_id=e.coach_id) 
";

        /// <summary>
        /// 取学员已预约过的课程信息
        /// </summary>
        private static readonly string getStudentAppointmentedClassInfoSql = @"
SELECT b.* FROM class_student a
INNER JOIN class b ON a.class_id=b.id 
WHERE a.student_id='{0}' AND b.start_time>NOW()
";

        /// <summary>
        /// 取教练已预约过的课程信息
        /// </summary>
        private static readonly string getCoachAppointmentedClassInfoSql = @"
SELECT a.*,IF(a.max_count>COUNT(1),FALSE,TRUE)isfull FROM class a
INNER JOIN class_student b ON a.id=b.class_id
WHERE a.coach_id='{0}' AND a.start_time>NOW()
GROUP BY a.id
";

        /// <summary>
        /// 获取1小时后，可以接受之前被限定预约请求的教练信息
        /// </summary>
        private static readonly string getCoachInfoCouldRecieveAppointmentSql = @"
SELECT g.id,g.phone FROM booking_coach_queue a
INNER JOIN course b ON a.course_id=b.id
INNER JOIN coachcaption_venue c ON b.venue_id=c.venue_id
INNER JOIN coach_caption d ON c.coach_id=d.caption_id
INNER JOIN booking_student f ON a.course_id=f.course_id AND f.state=0 AND f.start_time>NOW()
INNER JOIN class e ON !(d.coach_id=e.coach_id AND b.venue_id=e.venue_id AND e.start_time=f.start_time)
INNER JOIN coach g ON d.coach_id=g.id
WHERE DATE_FORMAT(a.end_time,'%Y-%m-%d %H:%i')='{0}'
GROUP BY g.id
";

        /// <summary>
        /// 获取预约页面的课程信息（两个参数同时>0时，能取到具体某一预约详情）
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static List<AppointmentCourseListJson> GetCourseInfoList(int studentId, int productId)
        {
            try
            {
                string where = string.Empty;
                if (studentId != 0)
                    where += $" and a.student_id='{studentId}'";
                if (productId != 0)
                    where += $" and a.product_id='{productId}'";

                using (DBHelper dbhelper = new DBHelper())
                {
                    DataTable dt = dbhelper.ExecuteDataTable(string.Format(getAppointmentInfoSql, where));

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.ToList<AppointmentCourseListJson>();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("GetCourseInfoList 出错：" + ex, Util.Log.LogType.Error);
            }

            return new List<AppointmentCourseListJson>();
        }

        /// <summary>
        /// 获取时间还没到的上课记录
        /// </summary>
        /// <param name="studentId">学生id</param>
        /// <returns></returns>
        public static List<Class> GetStudentAppointmentedInfo(int studentId)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    DataTable dt = dbhelper.ExecuteDataTable(string.Format(getStudentAppointmentedClassInfoSql, studentId));

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.ToList<Class>();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("GetAppointmentedInfo 出错：" + ex, Util.Log.LogType.Error);
            }

            return new List<Class>();
        }

        /// <summary>
        /// 取教练已预约好的课程信息
        /// </summary>
        /// <param name="coachId">教练id</param>
        /// <returns></returns>
        public static List<CoachAppointedClassJson> GetCoachAppointmentedClassInfo(int coachId)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    DataTable dt = dbhelper.ExecuteDataTable(string.Format(getCoachAppointmentedClassInfoSql, coachId));

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.ToList<CoachAppointedClassJson>();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("GetAppointmentedInfo 出错：" + ex, Util.Log.LogType.Error);
            }

            return new List<CoachAppointedClassJson>();
        }


        /// <summary>
        /// 获取截止时间后，可以接受之前被限定预约请求的教练信息
        /// </summary>
        /// <returns></returns>
        public static List<Coach> GetCoachInfoCouldRecieveAppointment(string time)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    DataTable dt = dbhelper.ExecuteDataTable(string.Format(getCoachInfoCouldRecieveAppointmentSql, time));

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.ToList<Coach>();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("GetCoachInfoCouldRecieveAppointment 出错：" + ex, Util.Log.LogType.Error);
            }

            return new List<Coach>();
        }

        /// <summary>
        /// 获取教练可接单的预约信息
        /// </summary>
        /// <param name="coachId">教练id</param>
        /// <returns></returns>
        public static List<ClassBookingOfCoachJson> GetBookingListOfCoach(int coachId)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    DataTable dt = dbhelper.ExecuteDataTableParams(string.Format(getBookingListOfCoach, coachId));

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.ToList<ClassBookingOfCoachJson>();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("GetBookingListOfCoach 出错：" + ex, Util.Log.LogType.Error);
            }

            return new List<ClassBookingOfCoachJson>();
        }


    }
}
