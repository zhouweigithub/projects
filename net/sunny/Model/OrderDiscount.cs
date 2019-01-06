using Sunny.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model
{
    /// <summary>
    /// 订单折扣信息
    /// </summary>
    public class OrderDiscount
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
        /// 折扣id
        /// </summary>
        [TableField]
        public int discount_id { get; set; }
        /// <summary>
        /// 折扣名称
        /// </summary>
        [TableField]
        public string name { get; set; }
        /// <summary>
        /// 立减金额
        /// </summary>
        [TableField]
        public decimal money { get; set; }
    }
}
