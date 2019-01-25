using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model.Custom
{

    /// <summary>
    /// 商品价格信息
    /// </summary>
    public class CustProductPriceInfoJson
    {
        /// <summary>
        /// 商品id
        /// </summary>
        public int product_id { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 原价
        /// </summary>
        public decimal price { get; set; }
        /// <summary>
        /// 折扣金额
        /// </summary>
        public decimal discount_money { get; set; }
        /// <summary>
        /// 折扣id
        /// </summary>
        public int discount_id { get; set; }
        /// <summary>
        /// 折扣名称
        /// </summary>
        public string discount_name { get; set; }

    }
}
