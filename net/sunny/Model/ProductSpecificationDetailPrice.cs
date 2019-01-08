using Sunny.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model
{
    /// <summary>
    /// 商品各规格与价格表
    /// </summary>
    public class ProductSpecificationDetailPrice
    {
        /// <summary>
        /// 商品id
        /// </summary>
        [TableField]
        public int product_id { get; set; }
        /// <summary>
        /// 规格组合方案代码，用来区分不同种规格组合
        /// </summary>
        [TableField]
        public string plan_code { get; set; }
        /// <summary>
        /// 该规格组合对应的价格
        /// </summary>
        [TableField]
        public decimal price { get; set; }
    }
}
