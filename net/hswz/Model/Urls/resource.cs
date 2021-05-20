using System;
using Hswz.Model.Common;

namespace Hswz.Model.Urls
{
    public class resource
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
        /// 名称
        /// </summary>
        [TableField]
        public String rname { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [TableField]
        public String remark { get; set; }

        /// <summary>
        /// 主域名
        /// </summary>
        [TableField]
        public String domain { get; set; }

        /// <summary>
        /// 点击量
        /// </summary>
        [TableField]
        public Int32 click { get; set; }

        ///// <summary>
        ///// 创建时间
        ///// </summary>
        //public DateTime crtime { get; set; }
    }
}
