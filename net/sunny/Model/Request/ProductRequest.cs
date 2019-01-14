using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model.Request
{
    public class ProductRequest
    {
        public int prouductid { get; set; }
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
        /// 折扣id
        /// </summary>
        public int discountids { get; set; }
        /// <summary>
        /// 折扣后价格
        /// </summary>
        public decimal price { get; set; }
        /// <summary>
        /// 最大教学人数
        /// </summary>
        public int max_person { get; set; }
        /// <summary>
        /// 场馆id
        /// </summary>
        public int venueid { get; set; }
    }
}
