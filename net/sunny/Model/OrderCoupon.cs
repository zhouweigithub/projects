using Sunny.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model
{
    /// <summary>
    /// 订单优惠券信息
    /// </summary>
    public class OrderCoupon
    {
        /// <summary>
        /// 订单id
        /// </summary>
        [TableField]
        public int order_id { get; set; }
        /// <summary>
        /// 商品id
        /// </summary>
        [TableField]
        public int product_id { get; set; }
        /// <summary>
        /// 优惠券id
        /// </summary>
        [TableField]
        public int coupon_id { get; set; }
        /// <summary>
        /// 优惠券名称
        /// </summary>
        [TableField]
        public string name { get; set; }
        /// <summary>
        /// 优惠券金额
        /// </summary>
        [TableField]
        public decimal price { get; set; }
        /// <summary>
        /// 优惠券数量
        /// </summary>
        [TableField]
        public int count { get; set; }
        /// <summary>
        /// 优惠总金额
        /// </summary>
        [TableField]
        public decimal money { get; set; }
    }
}
