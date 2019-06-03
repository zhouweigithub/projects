using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spetmall.Model.Common;

namespace Spetmall.Model
{
    /// <summary>
    /// 做活动的商品(BASE TABLE)
    /// </summary>
    public class saleproduct
    {

        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 0折扣 1满就送
        /// </summary>
        [TableField]
        public short type { get; set; }
        /// <summary>
        /// 0商品 1分类
        /// </summary>
        [TableField]
        public short ptype { get; set; }
        /// <summary>
        /// 活动id
        /// </summary>
        [TableField]
        public int saleid { get; set; }
        /// <summary>
        /// 商品或分类 id
        /// </summary>
        [TableField]
        public int productid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime crtime { get; set; }

    }

    public class saleproductObject : saleproduct
    {
        public string productName { get; set; }
    }
}
