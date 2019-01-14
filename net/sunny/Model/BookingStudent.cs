using Sunny.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model
{
    /// <summary>
    /// 用户下的预订信息
    /// </summary>
    public class BookingStudent
    {
        public int Id { get; set; }
        /// <summary>
        /// course.id
        /// </summary>
        [TableField]
        public int course_id { get; set; }
        /// <summary>
        /// 预订上课开始时间
        /// </summary>
        [TableField]
        public DateTime start_time { get; set; }
        /// <summary>
        /// 预订上课结束时间
        /// </summary>
        [TableField]
        public DateTime end_time { get; set; }
        /// <summary>
        /// 状态0正常 1无效
        /// </summary>
        [TableField]
        public short state { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime crtime { get; set; }
    }
}
