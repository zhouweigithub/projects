using System;
using Hswz.Model.Common;

namespace Hswz.Model.Urls
{
    /// <summary>
    /// 用户点击信息
    /// </summary>
    public class url_click
    {

        /// <summary>
        /// 站点id
        /// </summary>
        [TableField]
        public Int32 url_id { get; set; }

        /// <summary>
        /// 访问日期
        /// </summary>
        [TableField]
        public DateTime crdate { get; set; }

        /// <summary>
        /// 访问时间
        /// </summary>
        [TableField]
        public DateTime crtime { get; set; }

        /// <summary>
        /// 访问者ip地址
        /// </summary>
        [TableField]
        public String ip { get; set; }
    }
}
