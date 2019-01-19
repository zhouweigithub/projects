using Sunny.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model
{
    /// <summary>
    /// 热门课程
    /// </summary>
    public class HotCourse
    {
        /// <summary>
        /// 商品id
        /// </summary>
        [TableField]
        public int product_id { get; set; }
        /// <summary>
        /// 类型0热门课程1精品课程
        /// </summary>
        [TableField]
        public short type { get; set; }
        /// <summary>
        /// 排列序号
        /// </summary>
        public int sort_order { get; set; }
    }
}
