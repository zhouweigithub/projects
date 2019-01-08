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
    public class ProductSpecificationDetail
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
        /// 分类详情id
        /// </summary>
        [TableField]
        public int specification_detail_id { get; set; }
        /// <summary>
        /// 0正常 1禁用
        /// </summary>
        [TableField]
        public short state { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime crtime { get; set; }
    }
}
