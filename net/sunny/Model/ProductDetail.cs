using Sunny.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model
{
    /// <summary>
    /// 商品详细描述
    /// </summary>
    public class ProductDetail
    {
        /// <summary>
        /// 课程id
        /// </summary>
        [TableField]
        public int product_id { get; set; }
        /// <summary>
        /// 商品详情
        /// </summary>
        [TableField]
        public string detail { get; set; }
    }
}
