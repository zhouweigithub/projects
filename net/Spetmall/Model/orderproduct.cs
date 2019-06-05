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
        /// 原始价格
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
        /// <summary>
        /// 条码
        /// </summary>
        [TableField]
        public string barcode { get; set; }
        /// <summary>
        /// 分类
        /// </summary>
        [TableField]
        public int category { get; set; }
        /// <summary>
        /// 购买时是否启用了会员折扣
        /// </summary>
        [TableField]
        public short ismemberdiscount { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [TableField]
        public string name { get; set; }
        /// <summary>
        /// 缩略图
        /// </summary>
        [TableField]
        public string thumbnail { get; set; }

    }
}
