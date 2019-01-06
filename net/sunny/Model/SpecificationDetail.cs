using Sunny.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model
{
    /// <summary>
    /// 规格详情
    /// </summary>
    public class SpecificationDetail
    {
        public int id { get; set; }
        /// <summary>
        /// 规格类型id
        /// </summary>
        [TableField]
        public int specification_id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [TableField]
        public string name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [TableField]
        public string summary { get; set; }
        /// <summary>
        /// 0正常 1禁用
        /// </summary>
        [TableField]
        public short state { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime crtime { get; set; }
    }
}
