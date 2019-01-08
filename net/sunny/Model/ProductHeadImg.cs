using Sunny.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model
{
    /// <summary>
    /// 商品的头部图片
    /// </summary>
    public class ProductHeadImg
    {
        public int id { get; set; }
        /// <summary>
        /// 商品id
        /// </summary>
        [TableField]
        public int product_id { get; set; }
        /// <summary>
        /// 头部图片地址
        /// </summary>
        [TableField]
        public string headimg_url { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime crtime { get; set; }
    }
}
