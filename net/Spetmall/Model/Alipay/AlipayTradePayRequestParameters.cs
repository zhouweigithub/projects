using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spetmall.Model.Alipay
{
    /// <summary>
    /// 统一收单交易支付接口参数
    /// </summary>
    public class AlipayTradePayRequestParameters
    {
        /// <summary>
        /// 商户订单号,64个字符以内、可包含字母、数字、下划线；需保证在商户端不重复
        /// </summary>
        public string out_trade_no;
        /// <summary>
        /// 支付场景 条码支付bar_code, 声波支付wave_code
        /// </summary>
        public string scene = "bar_code";
        /// <summary>
        /// 支付授权码，25~30开头的长度为16~24位的数字，实际字符串长度以开发者获取的付款码长度为准
        /// </summary>
        public string auth_code;
        /// <summary>
        /// 订单标题
        /// </summary>
        public string subject;
    }
}
