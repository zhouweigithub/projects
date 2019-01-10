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
        private static readonly string getCourseInfo = @"
SELECT a.id courseid,b.name course_name,b.main_img,a.hour,a.over_hour,a.max_count,c.name venue_name,d.name campus_name,
IF(a.hour=a.over_hour,'已学完',IF(ISNULL(e.course_id),'可预约',IF(e.state=0,'预约中','已预约')))state
FROM course a
INNER JOIN product b ON a.product_id=b.id
INNER JOIN venue c ON a.venue_id=c.id
INNER JOIN campus d ON c.campus_id=d.id
LEFT JOIN appointment e ON e.start_time>NOW() AND e.course_id=a.id
WHERE a.student_id='{0}'
GROUP BY a.id
";

        /// <summary>
        /// 获取预约页面的课程信息
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public static List<AppointmentCourseListJson> GetCourseInfoList(int studentId)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    DataTable dt = dbhelper.ExecuteDataTable(string.Format(getCourseInfo, studentId));

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

    }
}
