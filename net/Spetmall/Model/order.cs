using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spetmall.Model.Common;

namespace Spetmall.Model
{
    /// <summary>
    /// 订单(BASE TABLE)
    /// </summary>
    public class order
    {

        /// <summary>
        /// 订单号
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 商品总金额
        /// </summary>
        [TableField]
        public decimal productMoney { get; set; }
        /// <summary>
        /// 实际支付总金额
        /// </summary>
        [TableField]
        public decimal payMoney { get; set; }
        /// <summary>
        /// 优惠金额
        /// </summary>
        [TableField]
        public decimal discountMoney { get; set; }
        /// <summary>
        /// 高价金额
        /// </summary>
        [TableField]
        public decimal adjustMomey { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        [TableField]
        public short payType { get; set; }
        /// <summary>
        /// 会员id
        /// </summary>
        [TableField]
        public int memberid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime crtime { get; set; }

    }
}
