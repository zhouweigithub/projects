using Sunny.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model
{
    /// <summary>
    /// 学生已完成课时信息
    /// </summary>
    public class CourseHourStudent
    {
        /// <summary>
        /// 课程id
        /// </summary>
        [TableField]
        public int course_id { get; set; }
        /// <summary>
        /// 学员id
        /// </summary>
        [TableField]
        public int student_id { get; set; }
        /// <summary>
        /// 课时
        /// </summary>
        [TableField]
        public int hour { get; set; }
    }
}
