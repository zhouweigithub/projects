using Sunny.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model
{
    /// <summary>
    /// 教练跟教练队长的关系
    /// </summary>
    public class CoachCaption
    {
        /// <summary>
        /// 教练id
        /// </summary>
        [TableField]
        public int coach_id { get; set; }
        /// <summary>
        /// 教练队长id
        /// </summary>
        [TableField]
        public int caption_id { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime crtime { get; set; }
    }
}
