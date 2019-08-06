using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spetmall.BLL.Page
{
    public class PayBLL
    {
        /// <summary>
        /// 条码支付
        /// </summary>
        /// <param name="orderid">订单号</param>
        /// <param name="auth_code">付款码</param>
        /// <param name="subject">商品信息</param>
        /// <param name="money">支付金额</param>
        public void Pay(string orderid, string auth_code, string subject, decimal money)
        {
            if (IsAlipay(auth_code))
            {
                AlipayBLL.Pay(orderid, auth_code, subject, money);
            }
            else if (IsWxpay(auth_code))
            {
                WxpayBLL.Pay(subject, money * 100, auth_code);
            }
            else
            {
                Util.Log.LogUtil.Write("未知付款码 " + auth_code, Util.Log.LogType.Error);
            }
        }

        /// <summary>
        /// 是否是支付宝支付
        /// </summary>
        /// <param name="auth_code"></param>
        /// <returns></returns>
        private bool IsAlipay(string auth_code)
        {
            return true;
        }

        /// <summary>
        /// 是否是微信支付
        /// </summary>
        /// <param name="auth_code"></param>
        /// <returns></returns>
        private bool IsWxpay(string auth_code)
        {
            return true;
        }
    }
}
