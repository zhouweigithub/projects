using Sunny.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model
{
    /// <summary>
    /// 站内各处的图片
    /// </summary>
    public class Banner
    {
        public int id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [TableField]
        public string url { get; set; }
        /// <summary>
        /// 类型0首页顶部图片1电话处图片2介绍处图片3介绍内容图片4商城顶部图片
        /// </summary>
        [TableField]
        public short type { get; set; }
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
