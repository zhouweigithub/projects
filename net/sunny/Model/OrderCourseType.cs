using Sunny.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model
{
    /// <summary>
    /// 课时信息
    /// </summary>
    public class OrderCourseType
    {
        /// <summary>
        /// 订单id
        /// </summary>
        [TableField]
        public string order_id { get; set; }
        /// <summary>
        /// 商品id
        /// </summary>
        [TableField]
        public int product_id { get; set; }
        /// <summary>
        /// 最大上课人数
        /// </summary>
        [TableField]
        public int max_people { get; set; }
    }
}
