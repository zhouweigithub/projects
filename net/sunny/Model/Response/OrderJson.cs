using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model.Custom
{
    public class OrderJson
    {
        public string order_id { get; set; }
        public decimal money { get; set; }
        public decimal coupon_money { get; set; }
        public decimal discount_money { get; set; }
        public short state { get; set; }
        public DateTime crtime { get; set; }
        public List<OrderCouponJson> coupons { get; set; }
        public List<OrderProductJson> products { get; set; }
        public string state_string
        {
            get
            {
                switch (state)
                {
                    case 0:
                        return "未支付";
                    case 1:
                        return "已支付";
                    case 2:
                        return "已发货";
                    case 3:
                        return "已收货";
                    case 4:
                        return "已评价";
                    default:
                        return "未知";
                }
            }
        }

    }


    public class OrderProductJson
    {
        public string order_id { get; set; }
        public int product_id { get; set; }
        public string product_name { get; set; }
        public int count { get; set; }
        public decimal price { get; set; }
        public decimal orig_price { get; set; }
        public decimal total_amount { get; set; }
        public string specifications { get; set; }
        public string venue_name { get; set; }
        public string campus { get; set; }
    }


    public class OrderCouponJson
    {
        public string order_id { get; set; }
        public string name { get; set; }
        public int count { get; set; }
        public decimal money { get; set; }
    }
}
