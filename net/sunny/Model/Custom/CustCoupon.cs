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
        public string name { get; set; }
        public decimal money { get; set; }
        public int count { get; set; }
    }
}
