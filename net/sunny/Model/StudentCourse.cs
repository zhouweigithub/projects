using Sunny.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model
{
    /// <summary>
    /// 学生预约课程情况
    /// </summary>
    public class StudentCourse
    {
        public int id { get; set; }
        /// <summary>
        /// 学员id
        /// </summary>
        [TableField]
        public int student_id { get; set; }
        /// <summary>
        /// 课程id
        /// </summary>
        [TableField]
        public int course_id { get; set; }
        /// <summary>
        /// 挂靠的场馆id
        /// </summary>
        [TableField]
        public int venue_id { get; set; }
        /// <summary>
        /// 课程金额
        /// </summary>
        [TableField]
        public int money { get; set; }
        /// <summary>
        /// 0未支付 1已支付
        /// </summary>
        [TableField]
        public short state { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime crtime { get; set; }
    }
}
