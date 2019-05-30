using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spetmall.Model.Page
{
    /// <summary>
    /// 确认订单的信息
    /// </summary>
    public class receipt_confirm_products
    {
        /// <summary>
        /// 商品id
        /// </summary>
        public int productId;
        /// <summary>
        /// 数量
        /// </summary>
        public int count;
        /// <summary>
        /// 单价
        /// </summary>
        public decimal price;
        /// <summary>
        /// 折扣前金额
        /// </summary>
        public decimal money;
        /// <summary>
        /// 折扣
        /// </summary>
        public double discount;
        /// <summary>
        /// 折扣金额
        /// </summary>
        public decimal discount_money;
        /// <summary>
        /// 缩略图
        /// </summary>
        public string thumbnail;
        /// <summary>
        /// 商品名称
        /// </summary>
        public string productName;
        /// <summary>
        /// 是否启用会员折扣
        /// </summary>
        public bool isDiscounted;
    }
}
