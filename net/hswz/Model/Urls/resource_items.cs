using System;
using Hswz.Model.Common;

namespace Hswz.Model.Urls
{
    public class resource_items
    {

        /// <summary>
        /// id
        /// </summary>
        public Int32 id { get; set; }

        /// <summary>
        /// 资源地址
        /// </summary>
        [TableField]
        public String url { get; set; }

        /// <summary>
        /// 资源域名
        /// </summary>
        [TableField]
        public String domain { get; set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        [TableField]
        public String title { get; set; }
    }
}
