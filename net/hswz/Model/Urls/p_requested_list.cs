using System;
using Hswz.Model.Common;

namespace Hswz.Model.Urls
{
    public class p_requested_list
    {

        /// <summary>
        /// 资源地址
        /// </summary>
        [TableField]
        public DateTime crdate { get; set; }

        /// <summary>
        /// 资源域名
        /// </summary>
        [TableField]
        public String url_format { get; set; }
    }
}
