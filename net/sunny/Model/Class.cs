using Sunny.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model
{
    /// <summary>
    /// 具体每次上课信息
    /// </summary>
    public class Class
    {
        public int id { get; set; }
        /// <summary>
        /// 课程id
        /// </summary>
        [TableField]
        public int product_id { get; set; }
        /// <summary>
        /// 教练id
        /// </summary>
        [TableField]
        public int coach_id { get; set; }
        /// <summary>
        /// 第几课时
        /// </summary>
        [TableField]
        public int hour { get; set; }
        /// <summary>
        /// 场馆id
        /// </summary>
        [TableField]
        public int venue_id { get; set; }
        /// <summary>
        /// 最大教学人数
        /// </summary>
        [TableField]
        public int max_count { get; set; }
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
        /// 教练分成比例
        /// </summary>
        [TableField]
        public float rate { get; set; }
        /// <summary>
        /// 0未上课 1已上课 2教练已评价
        /// </summary>
        [TableField]
        public short state { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime crtime { get; set; }
    }
}
