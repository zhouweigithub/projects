using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model.WXPay
{
    /// <summary>
    /// 微信支付关闭订单参数
    /// </summary>
    public class WXPayCloseOrderParameter
    {
        /// <summary>  
        /// 小程序ID    String(32)    必填(是)
        /// </summary>
        public string appid;
        /// <summary>  
        /// 商户号    String(32)    必填(是)
        /// </summary>
        public string mch_id;
        /// <summary>  
        /// 商户订单号    String(32)    必填(是)
        /// </summary>
        public string out_trade_no;
        /// <summary>  
        /// 随机字符串    String(32)    必填(是)
        /// </summary>
        public string nonce_str;
        /// <summary>  
        /// 签名    String(32)    必填(是)
        /// </summary>
        public string sign;
        /// <summary>  
        /// 签名类型    String(32)    必填(否)
        /// </summary>
        public string sign_type;
    }
}
