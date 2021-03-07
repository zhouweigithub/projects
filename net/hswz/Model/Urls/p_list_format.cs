using System;
using Hswz.Model.Common;

namespace Hswz.Model.Urls
{
    public class p_list_format
    {

        /// <summary>
        /// id
        /// </summary>
        [TableField]
        public Int32 url_id { get; set; }

        /// <summary>
        /// 列表页地址
        /// </summary>
        [TableField]
        public String url_format { get; set; }

        /// <summary>
        /// 主域名
        /// </summary>
        [TableField]
        public String domain { get; set; }
    }
}
