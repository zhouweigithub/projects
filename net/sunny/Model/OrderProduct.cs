using Sunny.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model
{
    /// <summary>
    /// 订单中的商品信息
    /// </summary>
    public class OrderProduct
    {
        /// <summary>
        /// 订单id
        /// </summary>
        [TableField]
        public string order_id { get; set; }
        /// <summary>
        /// 商品id
        /// </summary>
        [TableField]
        public int product_id { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        [TableField]
        public string product_name { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        [TableField]
        public int count { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        [TableField]
        public decimal price { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        [TableField]
        public decimal total_amount { get; set; }
        /// <summary>
        /// 折扣金额
        /// </summary>
        [TableField]
        public decimal discount_amount { get; set; }
        /// <summary>
        /// 优惠券抵扣金额
        /// </summary>
        [TableField]
        public decimal coupon_amount { get; set; }
    }
}
