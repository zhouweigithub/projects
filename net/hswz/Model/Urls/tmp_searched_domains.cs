using System;
using Hswz.Model.Common;

namespace Hswz.Model.Urls
{
    /// <summary>
    /// 已经检索过的主域名
    /// </summary>
    public class tmp_searched_domains
    {
        /// <summary>
        /// 资源的主域名集
        /// </summary>
        [TableField]
        public String domain { get; set; }

        /// <summary>
        /// 状态 0未检索 1已检索
        /// </summary>
        [TableField]
        public Int32 status { get; set; } = 0;

        ///// <summary>
        ///// 创建时间
        ///// </summary>
        //public DateTime crtime { get; set; }
    }
}
