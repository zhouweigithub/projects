using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model.Request
{
    public class OrderRequest
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string user_name { get; set; }
        /// <summary>
        /// 订单实际需要支付的总金额
        /// </summary>
        public decimal money { get; set; }
        /// <summary>
        /// 优惠券信息
        /// </summary>
        public TmpCoupon[] coupons { get; set; }
        /// <summary>
        /// 收货人信息
        /// </summary>
        public int receiverid { get; set; }
        /// <summary>
        /// 留言
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 送货方式
        /// </summary>
        public int deliverid { get; set; }
        /// <summary>
        /// 运费
        /// </summary>
        public float freight { get; set; }
        /// <summary>
        /// 商品信息
        /// </summary>
        public List<ProductRequest> products { get; set; }

    }
    public class TmpCoupon
    {
        public int id;
        public int count;
    }

}
