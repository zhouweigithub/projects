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
        public int courseid { get; set; }
        /// <summary>
        /// 课程名称
        /// </summary>
        public string course_name { get; set; }
        /// <summary>
        /// 主图
        /// </summary>
        public string main_img { get; set; }
        /// <summary>
        /// 总学时
        /// </summary>
        public int hour { get; set; }
        /// <summary>
        /// 已完成学时
        /// </summary>
        public int over_hour { get; set; }
        /// <summary>
        /// 上课最大学生人数
        /// </summary>
        public int max_count { get; set; }
        /// <summary>
        /// 场馆名称
        /// </summary>
        public string venue_name { get; set; }
        /// <summary>
        /// 校区名称
        /// </summary>
        public string campus_name { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string state { get; set; }
    }
}
