﻿using Sunny.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model
{
    /// <summary>
    /// 订单商品的规格与价格表
    /// </summary>
    public class OrderSpecificationDetail
    {
        /// <summary>
        /// 订单id
        /// </summary>
        [TableField]
        public int order_id { get; set; }
        /// <summary>
        /// 商品id
        /// </summary>
        [TableField]
        public int product_id { get; set; }
        /// <summary>
        /// 规格详情id
        /// </summary>
        [TableField]
        public int specification_detail_id { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        [TableField]
        public decimal price { get; set; }
    }
}
