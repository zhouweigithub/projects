using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spetmall.Model.Common;

namespace Spetmall.Model
{
    /// <summary>
    /// (BASE TABLE)
    /// </summary>
    public class orderproduct
    {

        /// <summary>
        /// 订单号
        /// </summary>
        [TableField]
        public string orderid { get; set; }
        /// <summary>
        /// 商品id
        /// </summary>
        [TableField]
        public int productid { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        [TableField]
        public decimal price { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        [TableField]
        public int count { get; set; }
        /// <summary>
        /// 商品总金额
        /// </summary>
        [TableField]
        public decimal money { get; set; }
        /// <summary>
        /// 优惠总金额
        /// </summary>
        [TableField]
        public decimal discountMoney { get; set; }
        /// <summary>
        /// 实际支付总金额
        /// </summary>
        [TableField]
        public decimal payMoney { get; set; }

    }
}
