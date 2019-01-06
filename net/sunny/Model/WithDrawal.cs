using Sunny.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model
{
    /// <summary>
    /// 提现记录
    /// </summary>
    public class WithDrawal
    {
        public int id { get; set; }
        /// <summary>
        /// 教练id
        /// </summary>
        [TableField]
        public int coach_id { get; set; }
        /// <summary>
        /// 提现金额
        /// </summary>
        [TableField]
        public int money { get; set; }
        /// <summary>
        /// 状态0成功 1失败
        /// </summary>
        [TableField]
        public short state { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime crtime { get; set; }
    }
}
