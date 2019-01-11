using Sunny.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model
{
    /// <summary>
    /// 接单时教练限定
    /// </summary>
    public class BookingCoachQueue
    {
        /// <summary>
        /// 预订id
        /// </summary>
        [TableField]
        public int class_id { get; set; }
        /// <summary>
        /// 教练id
        /// </summary>
        [TableField]
        public int coach_id { get; set; }
        /// <summary>
        /// 结束时间（此时间前，仅限coach_id能接class_id这一单）
        /// </summary>
        public DateTime end_time { get; set; }
    }
}
