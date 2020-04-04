using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spetmall.Model.Common;

namespace Spetmall.Model
{
    /// <summary>
    /// 分类(BASE TABLE)
    /// </summary>
    public class category
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
        /// 父id
        /// </summary>
        [TableField]
        public int pid { get; set; }
        /// <summary>
        /// 外部链接
        /// </summary>
        [TableField]
        public string url { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [TableField]
        public short state { get; set; }
        /// <summary>
        /// 顺序
        /// </summary>
        [TableField]
        public int index { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [TableField]
        public string remark { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime crtime { get; set; }
        /// <summary>
        /// 层级，顶层为0
        /// </summary>
        public int floor { get; set; }

    }
}
