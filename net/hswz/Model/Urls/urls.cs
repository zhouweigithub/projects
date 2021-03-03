using System;
using Hswz.Model.Common;

namespace Hswz.Model.Urls
{
    public class urls
    {

        /// <summary>
        /// id
        /// </summary>
        public Int32 id { get; set; }

        /// <summary>
        /// 站点地址
        /// </summary>
        [TableField]
        public String url { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [TableField]
        public String uname { get; set; }

        /// <summary>
        /// 访问量
        /// </summary>
        [TableField]
        public Int32 clcik { get; set; }

        /// <summary>
        /// 状态 0未启用 1已启用
        /// </summary>
        [TableField]
        public Int32 status { get; set; } = 0;

        /// <summary>
        /// 创建者的IP地址
        /// </summary>
        [TableField]
        public String createIp { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [TableField]
        public String remark { get; set; }

        ///// <summary>
        ///// 创建时间
        ///// </summary>
        //public DateTime crtime { get; set; }
    }

    public class url_data
    {
        /// <summary>
        /// id
        /// </summary>
        public Int32 id { get; set; }

        /// <summary>
        /// 站点地址
        /// </summary>
        public String url { get; set; }

        /// <summary>
        /// 点赞的数量
        /// </summary>
        public Int32 zan { get; set; }

        /// <summary>
        /// 踩的数量
        /// </summary>
        public Int32 cai { get; set; }

        /// <summary>
        /// 连接时间（秒）
        /// </summary>
        public Int32 connect_time { get; set; }

        public String full_url
        {
            get
            {
                if (!url.StartsWith("http://") && !url.StartsWith("https://"))
                {
                    return "http://" + url;
                }
                else
                {
                    return url;
                }
            }
        }

        /// <summary>
        /// 连接时长进度条显示长度
        /// </summary>
        public String percent => (connect_time == 0 || connect_time >= 30 ? 0 : Math.Round((30 - connect_time) / 30d * 100)) + "%";
    }
}
