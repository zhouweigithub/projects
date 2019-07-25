using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spetmall.Model.Common;

namespace Spetmall.Model
{
    /// <summary>
    /// 洗澡卡使用记录(BASE TABLE)
    /// </summary>
    public class railcard_record
    {

        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 洗澡卡id
        /// </summary>
        [TableField]
        public int railcardid { get; set; }
        /// <summary>
        /// 使用次数
        /// </summary>
        [TableField]
        public int times { get; set; }
        /// <summary>
        /// 剩余次数
        /// </summary>
        [TableField]
        public int lefttimes { get; set; }
        /// <summary>
        /// 使用备注
        /// </summary>
        [TableField]
        public string remark { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime crtime { get; set; }

    }
}
