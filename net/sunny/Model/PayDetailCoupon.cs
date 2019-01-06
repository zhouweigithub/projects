using Sunny.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model
{
    /// <summary>
    /// 支付时使用的优惠券信息
    /// </summary>
    public class PayDetailCoupon
    {
        /// <summary>
        /// 支付详情id
        /// </summary>
        [TableField]
        public string pay_detail_id { get; set; }
        /// <summary>
        /// 拥有的具体优惠券id
        /// </summary>
        [TableField]
        public int coupon_id { get; set; }
        /// <summary>
        /// 使用的金额
        /// </summary>
        [TableField]
        public int money { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime crtime { get; set; }
    }
}
