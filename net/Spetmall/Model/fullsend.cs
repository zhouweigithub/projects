using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spetmall.Model.Common;

namespace Spetmall.Model
{
    /// <summary>
    /// 满就送(BASE TABLE)
    /// </summary>
    public class fullsend
    {

        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [TableField]
        public string name { get; set; }
        /// <summary>
        /// 类型 按分类、按商品
        /// </summary>
        [TableField]
        public short type { get; set; }
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
