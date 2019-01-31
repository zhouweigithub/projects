using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model.Custom
{
    public class CustCoupon
    {
        /// <summary>
        /// 优惠券id
        /// </summary>
        public int id { get; set; }
        public int category_id { get; set; }
        public string name { get; set; }
        public decimal money { get; set; }
        /// <summary>
        /// 是否可以使用多张（0可以1不可以）
        /// </summary>
        public short multiple { get; set; }
        public int count { get; set; }
        public DateTime start_time { get; set; }
        public DateTime end_time { get; set; }
    }
}
