using Sunny.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model
{
    /// <summary>
    /// 已购买的课程信息
    /// </summary>
    public class Course
    {
        public int id { get; set; }
        /// <summary>
        /// 学员id
        /// </summary>
        [TableField]
        public int student_id { get; set; }
        /// <summary>
        /// 商品id
        /// </summary>
        [TableField]
        public int product_id { get; set; }
        /// <summary>
        /// 场馆id
        /// </summary>
        [TableField]
        public int venue_id { get; set; }
        /// <summary>
        /// 订单id
        /// </summary>
        [TableField]
        public string order_id { get; set; }
        /// <summary>
        /// 最大教学人数
        /// </summary>
        [TableField]
        public int max_count { get; set; }
        /// <summary>
        /// 总学时
        /// </summary>
        [TableField]
        public int hour { get; set; }
        /// <summary>
        /// 已完成学时
        /// </summary>
        [TableField]
        public int over_hour { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime crtime { get; set; }
    }
}
