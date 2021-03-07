using System;
using Hswz.Model.Common;

namespace Hswz.Model.Urls
{
    public class p_list_forbid
    {

        /// <summary>
        /// id
        /// </summary>
        [TableField]
        public Int32 id { get; set; }

        /// <summary>
        /// 主域名
        /// </summary>
        [TableField]
        public String domain { get; set; }
    }
}
