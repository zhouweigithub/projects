using System;
using Hswz.Model.Common;

namespace Hswz.Model.Urls
{
    public class domains
    {
        /// <summary>
        /// 资源的主域名集
        /// </summary>
        [TableField]
        public String domain { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime crtime { get; set; }
    }
}
