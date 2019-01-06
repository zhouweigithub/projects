using Sunny.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model
{
    /// <summary>
    /// 场馆信息
    /// </summary>
    public class Venue
    {
        public int id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [TableField]
        public string name { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        [TableField]
        public string code { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        [TableField]
        public string address { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        [TableField]
        public string summary { get; set; }
        /// <summary>
        /// 状态 0正常 1受限
        /// </summary>
        [TableField]
        public short state { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime crtime { get; set; }
    }
}
