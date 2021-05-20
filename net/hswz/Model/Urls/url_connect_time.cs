using System;
using Hswz.Model.Common;

namespace Hswz.Model.Urls
{
    public class url_connect_time
    {

        /// <summary>
        /// 站点id
        /// </summary>
        [TableField]
        public Int32 url_id { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        [TableField]
        public DateTime crdate { get; set; }

        /// <summary>
        /// 连接时间（秒）
        /// </summary>
        [TableField]
        public Int32 connect_time { get; set; }
    }
}
