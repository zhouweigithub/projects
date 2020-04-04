using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model.WXPay
{
    /// <summary>
    /// 微信支付申请退款
    /// </summary>
    public class WXPayRefundParameter
    {
        /// <summary>  
        /// 小程序ID    String(32)    必填(是)
        /// </summary>
        public string appid { get; set; }
        /// <summary>  
        /// 商户号    String(32)    必填(是)
        /// </summary>
        public string mch_id { get; set; }
        /// <summary>  
        /// 随机字符串    String(32)    必填(是)
        /// </summary>
        public string nonce_str { get; set; }
        /// <summary>  
        /// 签名    String(32)    必填(是)
        /// </summary>
        public string sign { get; set; }
        /// <summary>  
        /// 签名类型    String(32)    必填(否)
        /// </summary>
        public string sign_type { get; set; }
        /// <summary>  
        /// 微信订单号    String(32)    必填(二选一)
        /// </summary>
        public string transaction_id { get; set; }
        /// <summary>  
        /// 商户订单号    String(32)    必填()
        /// </summary>
        public string out_trade_no { get; set; }
        /// <summary>  
        /// 商户退款单号    String(64)    必填(是)
        /// </summary>
        public string out_refund_no { get; set; }
        /// <summary>  
        /// 订单金额    Int    必填(是)
        /// </summary>
        public string total_fee { get; set; }
        /// <summary>  
        /// 退款金额    Int    必填(是)
        /// </summary>
        public string refund_fee { get; set; }
        /// <summary>  
        /// 货币种类    String(8)    必填(否)
        /// </summary>
        public string refund_fee_type { get; set; }
        /// <summary>  
        /// 退款原因    String(80)    必填(否)
        /// </summary>
        public string refund_desc { get; set; }
        /// <summary>  
        /// 退款资金来源    String(30)    必填(否)
        /// </summary>
        public string refund_account { get; set; }
        /// <summary>  
        /// 退款结果通知url    String(256)    必填(否)
        /// </summary>
        public string notify_url { get; set; }
    }
}
