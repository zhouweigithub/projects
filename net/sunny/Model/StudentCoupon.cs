using Sunny.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model
{
    /// <summary>
    /// 学员的优惠券信息
    /// </summary>
    public class StudentCoupon
    {
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
        /// 状态0正常 1禁用
        /// </summary>
        [TableField]
        public short state { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime crtime { get; set; }
    }
}
