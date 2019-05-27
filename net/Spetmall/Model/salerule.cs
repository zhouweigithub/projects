using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spetmall.Model.Common;

namespace Spetmall.Model
{
    /// <summary>
    /// 活动规则(BASE TABLE)
    /// </summary>
    public class salerule
    {

        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 活动id
        /// </summary>
        [TableField]
        public int saleid { get; set; }
        /// <summary>
        /// 0按件折扣、1按价格折扣
        /// </summary>
        [TableField]
        public short type { get; set; }
        /// <summary>
        /// 起始金额或数量
        /// </summary>
        [TableField]
        public double aim { get; set; }
        /// <summary>
        /// 打几折或减多少金额
        /// </summary>
        [TableField]
        public double sale { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime crtime { get; set; }

    }
}
