using Sunny.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model
{
    /// <summary>
    /// 教练上课后资金结算
    /// </summary>
    public class Settlement
    {
        /// <summary>
        /// 上课id
        /// </summary>
        [TableField]
        public int class_id { get; set; }
        /// <summary>
        /// 本次课时总费用
        /// </summary>
        [TableField]
        public decimal total_money { get; set; }
        /// <summary>
        /// 教练所得费用
        /// </summary>
        [TableField]
        public decimal coach_money { get; set; }
        /// <summary>
        /// 平台所得费用
        /// </summary>
        [TableField]
        public decimal platform_money { get; set; }
        /// <summary>
        /// 教练分成比例
        /// </summary>
        [TableField]
        public float rate { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime crtime { get; set; }
    }
}
