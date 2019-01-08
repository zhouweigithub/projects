using Sunny.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model
{
    /// <summary>
    /// 购物订单信息
    /// </summary>
    public class Order
    {
        /// <summary>
        /// 订单号
        /// </summary>
        [TableField]
        public int order_id { get; set; }
        /// <summary>
        /// 实际支付金额
        /// </summary>
        [TableField]
        public decimal money { get; set; }
        /// <summary>
        /// 商品折扣金额
        /// </summary>
        [TableField]
        public decimal discount_money { get; set; }
        /// <summary>
        /// 优惠券抵扣金额
        /// </summary>
        [TableField]
        public decimal coupon_money { get; set; }
        /// <summary>
        /// 收货人
        /// </summary>
        [TableField]
        public string receiver { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        [TableField]
        public string phone { get; set; }
        /// <summary>
        /// 收货地址
        /// </summary>
        [TableField]
        public string address { get; set; }
        /// <summary>
        /// 买家留言
        /// </summary>
        [TableField]
        public string message { get; set; }
        /// <summary>
        /// 配送方式
        /// </summary>
        [TableField]
        public int deliver_id { get; set; }
        /// <summary>
        /// 运费
        /// </summary>
        [TableField]
        public decimal freight { get; set; }
        /// <summary>
        /// 状态0未支付1已支付2已发货3已收货
        /// </summary>
        [TableField]
        public int state { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        [TableField]
        public DateTime crdate { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime crtime { get; set; }
    }
}
