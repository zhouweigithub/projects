using Sunny.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model
{
    /// <summary>
    /// 学员邀请返现记录
    /// </summary>
    public class CashbackHistory
    {
        public int id { get; set; }
        /// <summary>
        /// 学员id
        /// </summary>
        [TableField]
        public int student_id { get; set; }
        /// <summary>
        /// 触发返现的学员id
        /// </summary>
        [TableField]
        public int from_student_id { get; set; }
        /// <summary>
        /// 返现金额
        /// </summary>
        [TableField]
        public decimal money { get; set; }
        /// <summary>
        /// 获得时间
        /// </summary>
        public DateTime crtime { get; set; }
    }
}
