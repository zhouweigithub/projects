using Sunny.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model
{
    /// <summary>
    /// 商品折扣信息
    /// </summary>
    public class ProductDiscount
    {
        public int id { get; set; }
        /// <summary>
        /// 商品id
        /// </summary>
        [TableField]
        public int product_id { get; set; }
        /// <summary>
        /// 折扣id
        /// </summary>
        [TableField]
        public int discount_id { get; set; }
        /// <summary>
        /// 折扣开始时间
        /// </summary>
        [TableField]
        public DateTime start_time { get; set; }
        /// <summary>
        /// 折扣结束时间
        /// </summary>
        [TableField]
        public DateTime end_time { get; set; }
        /// <summary>
        /// 状态0正常 1禁用
        /// </summary>
        [TableField]
        public short state { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime crttime { get; set; }
    }
}
