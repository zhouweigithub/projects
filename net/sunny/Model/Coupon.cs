using Sunny.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model
{
    /// <summary>
    /// 优惠券基本信息
    /// </summary>
    public class Coupon
    {
        public int id { get; set; }
        /// <summary>
        /// 课程id
        /// </summary>
        [TableField]
        public string name { get; set; }
        /// <summary>
        /// 教练id
        /// </summary>
        [TableField]
        public int money { get; set; }
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
        /// 状态0正常1不可用
        /// </summary>
        [TableField]
        public short state { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime crtime { get; set; }
    }
}
