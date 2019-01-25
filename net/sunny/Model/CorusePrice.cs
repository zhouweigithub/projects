using Sunny.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model
{
    /// <summary>
    /// 课程商品的价格
    /// </summary>
    public class CorusePrice
    {
        /// <summary>
        /// 课程商品id
        /// </summary>
        [TableField]
        public string product_id { get; set; }
        /// <summary>
        /// 场馆id
        /// </summary>
        [TableField]
        public short venud_id { get; set; }
        /// <summary>
        /// 上课人数类型course_type.id
        /// </summary>
        [TableField]
        public short type_id { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        [TableField]
        public short price { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime crtime { get; set; }
    }
}
