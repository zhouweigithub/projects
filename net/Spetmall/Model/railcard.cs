using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spetmall.Model.Common;

namespace Spetmall.Model
{
    /// <summary>
    /// 优惠券(BASE TABLE)
    /// </summary>
    public class railcard
    {

        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        [TableField]
        public string phone { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        [TableField]
        public string name { get; set; }
        /// <summary>
        /// 最大可使用次数
        /// </summary>
        [TableField]
        public int times { get; set; }
        /// <summary>
        /// 剩余使用次数
        /// </summary>
        [TableField]
        public int lefttimes { get; set; }
        /// <summary>
        /// 最大可使用金额
        /// </summary>
        [TableField]
        public decimal money { get; set; }
        /// <summary>
        /// 剩余金额
        /// </summary>
        [TableField]
        public decimal leftmoney { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        [TableField]
        public DateTime starttime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        [TableField]
        public DateTime endtime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime crtime { get; set; }

    }
}
