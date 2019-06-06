using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spetmall.Model.Common;

namespace Spetmall.Model
{
    /// <summary>
    /// 限时折扣(BASE TABLE)
    /// </summary>
    public class discount
    {

        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [TableField]
        public string name { get; set; }
        /// <summary>
        /// 类型 0按店铺 1按分类 2按商品
        /// </summary>
        [TableField]
        public short type { get; set; }
        /// <summary>
        /// 方式 0按件折扣 1按价格折扣
        /// </summary>
        [TableField]
        public short way { get; set; }
        /// <summary>
        /// 是否能同时使用满就减
        /// </summary>
        [TableField]
        public short fullsend { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        [TableField]
        public DateTime starttime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        [TableField]
        public DateTime endtime { get; set; }
        /// <summary>
        /// 状态 0关闭 1启用
        /// </summary>
        [TableField]
        public short state { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime crtime { get; set; }

        public string TypeString
        {
            get
            {
                return type == 0 ? "店铺" : type == 1 ? "指定分类" : type == 2 ? "指定商品" : "未知";
            }
        }

    }
}
