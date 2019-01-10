using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model.Custom
{
    /// <summary>
    /// 预约页面的课程列表信息
    /// </summary>
    public class AppointmentCourseListJson
    {
        /// <summary>
        /// 课程id
        /// </summary>
        public int courseid;
        /// <summary>
        /// 课程名称
        /// </summary>
        public string course_name;
        /// <summary>
        /// 主图
        /// </summary>
        public string main_img;
        /// <summary>
        /// 总学时
        /// </summary>
        public string hour;
        /// <summary>
        /// 已完成学时
        /// </summary>
        public string over_hour;
        /// <summary>
        /// 上课最大学生人数
        /// </summary>
        public string max_count;
        /// <summary>
        /// 场馆名称
        /// </summary>
        public string venue_name;
        /// <summary>
        /// 校区名称
        /// </summary>
        public string campus_name;
        /// <summary>
        /// 状态
        /// </summary>
        public string state;
    }
}
