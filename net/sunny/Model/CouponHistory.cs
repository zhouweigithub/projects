using Sunny.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model
{
    /// <summary>
    /// 优惠券获得历史记录
    /// </summary>
    public class CouponHistory
    {
        public int id { get; set; }
        /// <summary>
        /// 学员id
        /// </summary>
        [TableField]
        public int student_id { get; set; }
        /// <summary>
        /// 优惠券id
        /// </summary>
        [TableField]
        public int coupon_id { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        [TableField]
        public int count { get; set; }
        /// <summary>
        /// 获得时间
        /// </summary>
        public DateTime crtime { get; set; }
    }
}
