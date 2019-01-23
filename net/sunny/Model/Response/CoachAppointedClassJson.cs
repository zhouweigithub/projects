using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model.Response
{
    /// <summary>
    /// 教练已预约成功的记录
    /// </summary>
    public class CoachAppointedClassJson
    {
        /// <summary>
        /// 课程id
        /// </summary>
        public int product_id { get; set; }
        /// <summary>
        /// 教练id
        /// </summary>
        public int coach_id { get; set; }
        /// <summary>
        /// 第几课时
        /// </summary>
        public int hour { get; set; }
        /// <summary>
        /// 场馆id
        /// </summary>
        public int venue_id { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime start_time { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime end_time { get; set; }
        /// <summary>
        /// 是否已预约满
        /// </summary>
        public bool isfull { get; set; }
    }
}
