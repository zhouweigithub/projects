using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model.Custom
{
    /// <summary>
    /// 商品详情信息
    /// </summary>
    public class ProductInfoJson
    {
        /// <summary>
        /// 课程id
        /// </summary>
        public int product_id { get; set; }
        /// <summary>
        /// 课程名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 分类id
        /// </summary>
        public int category_id { get; set; }
        /// <summary>
        /// 是否是课程
        /// </summary>
        public bool iscourse { get { return false; } }
        /// <summary>
        /// 最低价
        /// </summary>
        public decimal min_price { get; set; }
        /// <summary>
        /// 最高价
        /// </summary>
        public decimal max_price { get; set; }
        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal discount_money { get; set; }
        /// <summary>
        /// 头部图片地址
        /// </summary>
        public string heading_urls { get; set; }
        /// <summary>
        /// 商品详情
        /// </summary>
        public string detail { get; set; }
    }

    ///// <summary>
    ///// 商品的价格信息
    ///// </summary>
    //public class CustProductPriceInfoJson
    //{
    //    /// <summary>
    //    /// 课程id
    //    /// </summary>
    //    public int product_id { get; set; }
    //    /// <summary>
    //    /// 课程名称
    //    /// </summary>
    //    public string course_name { get; set; }
    //    /// <summary>
    //    /// 原价
    //    /// </summary>
    //    public decimal price { get; set; }
    //    /// <summary>
    //    /// 折扣金额
    //    /// </summary>
    //    public decimal discount_money { get; set; }
    //    /// <summary>
    //    /// 折扣id
    //    /// </summary>
    //    public int discount_id { get; set; }
    //    /// <summary>
    //    /// 折扣名称
    //    /// </summary>
    //    public string discount_name { get; set; }

    //}

}
