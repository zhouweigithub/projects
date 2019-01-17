using Sunny.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model
{
    /// <summary>
    /// 邀请关系
    /// </summary>
    public class Invitation
    {
        /// <summary>
        /// 用户id
        /// </summary>
        [TableField]
        public int student_id { get; set; }
        /// <summary>
        /// 邀请人id
        /// </summary>
        [TableField]
        public int from_student_id { get; set; }
        /// <summary>
        /// 0未发放奖励 1已发放奖励
        /// </summary>
        [TableField]
        public short state { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime crtime { get; set; }
    }
}
