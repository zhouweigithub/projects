using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spetmall.BLL.Page
{
    internal class WxpayBLL
    {

        /// <summary>
        /// 微信条码支付
        /// </summary>
        /// <param name="body">商品描述</param>
        /// <param name="total_fee">总金额（分）</param>
        /// <param name="auth_code">用户付款码</param>
        public static void Pay(string body, decimal total_fee, string auth_code)
        {
            string result = WxPayAPI.MicroPay.Run(body, total_fee.ToString(), auth_code);
        }
    }
}
