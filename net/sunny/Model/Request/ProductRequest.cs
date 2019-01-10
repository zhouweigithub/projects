using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model.Request
{
    public class ProductRequest
    {
        public int ProuductId { get; set; }
        public int Count { get; set; }
        /// <summary>
        /// 规格编号
        /// </summary>
        public string PlanCode { get; set; }
        /// <summary>
        /// 原始价格
        /// </summary>
        public decimal PlanPrice { get; set; }
        /// <summary>
        /// 折扣id
        /// </summary>
        public int DiscountIds { get; set; }
        /// <summary>
        /// 折扣后价格
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 最大教学人数
        /// </summary>
        public int MaxPerson { get; set; }
        /// <summary>
        /// 场馆id
        /// </summary>
        public int VenueId { get; set; }
    }
}
