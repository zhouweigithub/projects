using Aop.Api;
using Aop.Api.Request;
using Aop.Api.Response;
using Spetmall.Model.Alipay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spetmall.BLL.Page
{
    public class AlipayBLL
    {
        /// <summary>
        /// 条码支付
        /// </summary>
        /// <param name="orderid">商户订单号</param>
        /// <param name="buyerAlipayCode">用户付款码</param>
        /// <param name="oneProductName">任意一商品名称</param>
        /// <returns></returns>
        public static string Pay(string orderid, string buyerAlipayCode, string oneProductName)
        {
            IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", "app_id", "merchant_private_key", "json", "1.0", "RSA2", "alipay_public_key", "GBK", false);
            AlipayTradePayRequest request = new AlipayTradePayRequest();
            AlipayTradePayRequestParameters requestParameters = new AlipayTradePayRequestParameters()
            {
                out_trade_no = orderid,
                auth_code = buyerAlipayCode,
                scene = "bar_code",
                subject = oneProductName,
            };

            AlipayTradePayResponse response = client.Execute(request);

            return response.IsError ? response.SubMsg : "支付成功";
        }
    }
}
