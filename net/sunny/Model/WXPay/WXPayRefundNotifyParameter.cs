using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model.WXPay
{
    /// <summary>
    /// 微信支付申请退款结果通知参数
    /// </summary>
    public class WXPayRefundNotifyParameter
    {
        /// <summary>  
        /// 返回状态码    String(16)    必填(是)
        /// </summary>
        public string return_code { get; set; }
        /// <summary>  
        /// 返回信息    String(128)    必填(是)
        /// </summary>
        public string return_msg { get; set; }
        /// <summary>  
        /// 公众账号ID    String(32)    必填(是)
        /// </summary>
        public string appid { get; set; }
        /// <summary>  
        /// 退款的商户号    String(32)    必填(是)
        /// </summary>
        public string mch_id { get; set; }
        /// <summary>  
        /// 随机字符串    String(32)    必填(是)
        /// </summary>
        public string nonce_str { get; set; }
        /// <summary>  
        /// 加密信息    String(1024)    必填(是)
        /// </summary>
        public string req_info { get; set; }
        /// <summary>  
        /// 微信订单号    String(32)    必填(是)
        /// </summary>
        public string transaction_id { get; set; }
        /// <summary>  
        /// 商户订单号    String(32)    必填(是)
        /// </summary>
        public string out_trade_no { get; set; }
        /// <summary>  
        /// 微信退款单号    String(32)    必填(是)
        /// </summary>
        public string refund_id { get; set; }
        /// <summary>  
        /// 商户退款单号    String(64)    必填(是)
        /// </summary>
        public string out_refund_no { get; set; }
        /// <summary>  
        /// 订单金额    Int    必填(是)
        /// </summary>
        public string total_fee { get; set; }
        /// <summary>  
        /// 应结订单金额    Int    必填(否)
        /// </summary>
        public string settlement_total_fee { get; set; }
        /// <summary>  
        /// 申请退款金额    Int    必填(是)
        /// </summary>
        public string refund_fee { get; set; }
        /// <summary>  
        /// 退款金额    Int    必填(是)
        /// </summary>
        public string settlement_refund_fee { get; set; }
        /// <summary>  
        /// 退款状态    String(16)    必填(是)
        /// </summary>
        public string refund_status { get; set; }
        /// <summary>  
        /// 退款成功时间    String(20)    必填(否)
        /// </summary>
        public string success_time { get; set; }
        /// <summary>  
        /// 退款入账账户    String(64)    必填(是)
        /// </summary>
        public string refund_recv_accout { get; set; }
        /// <summary>  
        /// 退款资金来源    String(30)    必填(是)
        /// </summary>
        public string refund_account { get; set; }
        /// <summary>  
        /// 退款发起来源    String(30)    必填(是)
        /// </summary>
        public string refund_request_source { get; set; }
    }
}
