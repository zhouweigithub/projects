using Sunny.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model
{
    /// <summary>
    /// 折扣基本信息
    /// </summary>
    public class Discount
    {
        public int id { get; set; }
        /// <summary>
        /// 折扣名称
        /// </summary>
        [TableField]
        public string name { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        [TableField]
        public string summary { get; set; }
        /// <summary>
        /// 立减金额
        /// </summary>
        [TableField]
        public decimal money { get; set; }
        /// <summary>
        /// 状态0正常 1禁用
        /// </summary>
        [TableField]
        public short state { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime crtime { get; set; }
    }
}
