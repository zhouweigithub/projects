using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spetmall.Model.Common;

namespace Spetmall.Model
{
    /// <summary>
    /// 商品信息(BASE TABLE)
    /// </summary>
    public class product
    {

        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 商品条码
        /// </summary>
        [TableField]
        public string barcode { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        [TableField]
        public string name { get; set; }
        /// <summary>
        /// 商品名称简拼
        /// </summary>
        [TableField]
        public string py { get; set; }
        /// <summary>
        /// 分类
        /// </summary>
        [TableField]
        public int category { get; set; }
        /// <summary>
        /// 库存
        /// </summary>
        [TableField]
        public int store { get; set; }
        /// <summary>
        /// 库存警戒值
        /// </summary>
        [TableField]
        public int warn { get; set; }
        /// <summary>
        /// 销量
        /// </summary>
        [TableField]
        public int sales { get; set; }
        /// <summary>
        /// 成本
        /// </summary>
        [TableField]
        public decimal cost { get; set; }
        /// <summary>
        /// 卖价
        /// </summary>
        [TableField]
        public decimal price { get; set; }
        /// <summary>
        /// 是否启用会员折扣（0不启用 1启用）
        /// </summary>
        [TableField]
        public short ismemberdiscount { get; set; }
        /// <summary>
        /// 缩略图
        /// </summary>
        [TableField]
        public string thumbnail { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [TableField]
        public string remark { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime crtime { get; set; }

    }

    public class product_show : product
    {
        public string categoryName { get; set; }
    }
}
