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
    public class Hours
    {
        /// <summary>
        /// 课程id
        /// </summary>
        [TableField]
        public int product_id { get; set; }
        /// <summary>
        /// 课时
        /// </summary>
        [TableField]
        public int hour { get; set; }
    }
}
