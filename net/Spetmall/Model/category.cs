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
        /// 
        /// </summary>
        public DateTime crtime { get; set; }

    }
}
