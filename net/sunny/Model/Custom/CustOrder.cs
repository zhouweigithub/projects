using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model.Custom
{
    public class CustOrderProduct
    {
        public string order_id { get; set; }
        public decimal money { get; set; }
        public short state { get; set; }
        public decimal coupon_money { get; set; }
        public decimal discount_money { get; set; }
        public DateTime crtime { get; set; }
        public string product_name { get; set; }
        public int product_id { get; set; }
        public int count { get; set; }
        public decimal price { get; set; }
        public decimal orig_price { get; set; }
        public decimal total_amount { get; set; }
        public string campus_name { get; set; }
        public string venue_name { get; set; }
        public string main_img { get; set; }
    }

    public class CustOrderProductSpecification
    {
        public string order_id { get; set; }
        public int product_id { get; set; }
        public string specifications { get; set; }
    }

    public class CustOrderCoupon
    {
        public string order_id { get; set; }
        public string name { get; set; }
        public int count { get; set; }
        public decimal money { get; set; }
    }

}
