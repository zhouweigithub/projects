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
        public string UserName { get; set; }
        /// <summary>
        /// 订单实际需要支付的总金额
        /// </summary>
        public decimal Money { get; set; }
        /// <summary>
        /// 优惠券信息
        /// </summary>
        public TmpCoupon[] Coupons { get; set; }
        /// <summary>
        /// 使用优惠券减掉的金额
        /// </summary>
        public decimal CouponMoney { get; set; }
        /// <summary>
        /// 收货人信息
        /// </summary>
        public int ReceiverId { get; set; }
        /// <summary>
        /// 留言
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 送货方式
        /// </summary>
        public int DeliverId { get; set; }
        /// <summary>
        /// 运费
        /// </summary>
        public float Freight { get; set; }
        /// <summary>
        /// 商品信息
        /// </summary>
        public List<ProductRequest> Products { get; set; }
    }
}
