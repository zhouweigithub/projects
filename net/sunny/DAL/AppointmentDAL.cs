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
    public class AppointmentDAL
    {
        private static readonly string getAppointmentInfoSql = @"
SELECT a.id courseid,b.name course_name,b.main_img,a.hour,a.over_hour,a.max_count,c.name venue_name,d.name campus_name,
IF(a.hour=a.over_hour,'已学完',IF(ISNULL(e.course_id) OR ISNULL(f.student_id),'可预约',IF(e.state=0,'预约中','已预约')))state
FROM course a
INNER JOIN product b ON a.product_id=b.id
INNER JOIN venue c ON a.venue_id=c.id
INNER JOIN campus d ON c.campus_id=d.id
LEFT JOIN class e ON e.start_time>NOW() AND e.course_id=a.id
LEFT JOIN class_student f ON a.student_id=f.student_id AND e.id=f.class_id
WHERE 1=1 {0}
GROUP BY a.id
";

        private static readonly string getAppointmentedInfoSql = @"
SELECT b.* FROM class_student a
INNER JOIN class b ON a.class_id=b.id 
WHERE a.student_id='1' AND b.start_time>NOW()
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
WHERE DATE_FORMAT(a.end_time,'%Y-%m-%d %H:%i')=DATE_FORMAT(NOW(),'%Y-%m-%d %H:%i')
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
        /// <param name="studentId"></param>
        /// <returns></returns>
        public static List<Class> GetAppointmentedInfo(int studentId)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    DataTable dt = dbhelper.ExecuteDataTable(string.Format(getAppointmentedInfoSql, studentId));

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
        /// 获取1小时后，可以接受之前被限定预约请求的教练信息
        /// </summary>
        /// <returns></returns>
        public static List<Coach> GetCoachInfoCouldRecieveAppointment()
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    DataTable dt = dbhelper.ExecuteDataTable(string.Format(getCoachInfoCouldRecieveAppointmentSql));

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



    }
}
