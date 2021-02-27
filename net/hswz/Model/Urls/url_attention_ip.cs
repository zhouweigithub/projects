using System;
using Hswz.Model.Common;

namespace Hswz.Model.Urls
{
    public class url_attention_ip
    {

        /// <summary>
        /// 站点id
        /// </summary>
        [TableField]
        public Int32 url_id { get; set; }

        /// <summary>
        /// 类型（zan/cai）
        /// </summary>
        [TableField]
        public String type { get; set; }

        /// <summary>
        /// 赞或踩的ips
        /// </summary>
        [TableField]
        public String ip { get; set; }
    }
}
