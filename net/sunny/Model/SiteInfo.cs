using Sunny.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model
{
    /// <summary>
    /// 站点基本信息
    /// </summary>
    public class SiteInfo
    {
        /// <summary>
        /// 关键字
        /// </summary>
        [TableField]
        public string key { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        [TableField]
        public string value { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime uptime { get; set; }
    }
}
