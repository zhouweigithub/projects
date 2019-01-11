using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model.Custom
{
    /// <summary>
    /// 教练可接单的预约信息
    /// </summary>
    public class ClassBookingOfCoach
    {
        /// <summary>
        /// 预约id
        /// </summary>
        public int classid;
        /// <summary>
        /// 预约的学时
        /// </summary>
        public int hour;
        /// <summary>
        /// 最大教学人数
        /// </summary>
        public int max_count;
        /// <summary>
        /// 上课的开始时间
        /// </summary>
        public int start_time;
        /// <summary>
        /// 上课的结束时间
        /// </summary>
        public DateTime end_time;
        /// <summary>
        /// 课程名称
        /// </summary>
        public int course_name;
        /// <summary>
        /// 已预约的学生人数
        /// </summary>
        public int booked_count;
    }
}
