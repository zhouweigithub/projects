using System;
using Hswz.Model.Common;

namespace Hswz.Model.Urls
{
    public class url_attention
    {

        /// <summary>
        /// 站点id
        /// </summary>
        [TableField]
        public Int32 url_id { get; set; }

        /// <summary>
        /// 点赞的数量
        /// </summary>
        [TableField]
        public Int32 zan { get; set; }

        /// <summary>
        /// 踩的数量
        /// </summary>
        [TableField]
        public Int32 cai { get; set; }
    }
}
