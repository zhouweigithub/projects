using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model.Request
{
    public class ProductRequest
    {
        /// <summary>
        /// 商品id
        /// </summary>
        public int prouduct_id { get; set; }
        /// <summary>
        /// 商品分类id
        /// </summary>
        public int category_id { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int count { get; set; }
        /// <summary>
        /// 规格编号
        /// </summary>
        public string plan_code { get; set; }
        /// <summary>
        /// 原始价格
        /// </summary>
        public decimal plan_price { get; set; }
        /// <summary>
        /// 折扣后价格
        /// </summary>
        public decimal price { get; set; }
        /// <summary>
        /// 最大教学人数id
        /// </summary>
        public int type_id { get; set; }
        /// <summary>
        /// 场馆id
        /// </summary>
        public int venue_id { get; set; }
    }

    public class TmpProduct
    {
        /// <summary>
        /// 商品id
        /// </summary>
        public int product_id { get; set; }
        /// <summary>
        /// 单价*数量
        /// </summary>
        public decimal total_money { get; set; }
    }

}
