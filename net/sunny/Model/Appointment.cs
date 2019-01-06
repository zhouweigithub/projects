using Sunny.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model
{
    /// <summary>
    /// 预约请求
    /// </summary>
    public class Appointment
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
        /// 场馆id
        /// </summary>
        [TableField]
        public int venue_id { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        [TableField]
        public DateTime start_time { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        [TableField]
        public DateTime end_time { get; set; }
        /// <summary>
        /// 状态0无教练接单 1有教练接单
        /// </summary>
        [TableField]
        public short state { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime crtime { get; set; }
    }
}
