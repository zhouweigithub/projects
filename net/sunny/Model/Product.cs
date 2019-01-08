using Sunny.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model
{
    /// <summary>
    /// 商品信息
    /// </summary>
    public class Product
    {
        public int id { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        [TableField]
        public string name { get; set; }
        /// <summary>
        /// 商品编码
        /// </summary>
        [TableField]
        public string code { get; set; }
        /// <summary>
        /// 类别
        /// </summary>
        [TableField]
        public int category_id { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        [TableField]
        public string summary { get; set; }
        /// <summary>
        /// 详细描述
        /// </summary>
        [TableField]
        public int detail_id { get; set; }
        /// <summary>
        /// 库存数量
        /// </summary>
        [TableField]
        public int stock { get; set; }
        /// <summary>
        /// 单次最少购买量
        /// </summary>
        [TableField]
        public int min_buy_count { get; set; }
        /// <summary>
        /// 单次最大购买量
        /// </summary>
        [TableField]
        public int max_buy_count { get; set; }
        /// <summary>
        /// 主图
        /// </summary>
        [TableField]
        public string main_img { get; set; }
        /// <summary>
        /// 状态0正常 1禁用
        /// </summary>
        [TableField]
        public short state { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime crtime { get; set; }
    }
}
