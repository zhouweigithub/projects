using System;
using Hswz.Model.Common;

namespace Hswz.Model.Urls
{
    public class resource_click
    {

        /// <summary>
        /// 资源id
        /// </summary>
        [TableField]
        public Int32 resource_id { get; set; }

        /// <summary>
        /// 请求者ip
        /// </summary>
        [TableField]
        public String ip { get; set; }

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
    }
}
