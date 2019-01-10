using Sunny.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model
{
    /// <summary>
    /// 订单商品的规格与价格表
    /// </summary>
    public class OrderProductSpecificationDetail
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
        /// 规格组合id
        /// </summary>
        [TableField]
        public string plan_code { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        [TableField]
        public decimal price { get; set; }
    }
}
