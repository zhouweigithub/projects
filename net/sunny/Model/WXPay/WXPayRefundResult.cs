using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model.WXPay
{
    /// <summary>
    /// 微信支付申请退款的结果
    /// </summary>
    public class WXPayRefundResult
    {
        /// <summary>  
        /// 返回状态码    String(16)    必填(是)
        /// </summary>
        public string return_code { get; set; }
        /// <summary>  
        /// 返回信息    String(128)    必填(否)
        /// </summary>
        public string return_msg { get; set; }
        /// <summary>  
        /// 业务结果    String(16)    必填(是)
        /// </summary>
        public string result_code { get; set; }
        /// <summary>  
        /// 错误代码    String(32)    必填(否)
        /// </summary>
        public string err_code { get; set; }
        /// <summary>  
        /// 错误代码描述    String(128)    必填(否)
        /// </summary>
        public string err_code_des { get; set; }
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
        /// 微信订单号    String(32)    必填(是)
        /// </summary>
        public string transaction_id { get; set; }
        /// <summary>  
        /// 商户订单号    String(32)    必填(是)
        /// </summary>
        public string out_trade_no { get; set; }
        /// <summary>  
        /// 商户退款单号    String(64)    必填(是)
        /// </summary>
        public string out_refund_no { get; set; }
        /// <summary>  
        /// 微信退款单号    String(32)    必填(是)
        /// </summary>
        public string refund_id { get; set; }
        /// <summary>  
        /// 退款金额    Int    必填(是)
        /// </summary>
        public string refund_fee { get; set; }
        /// <summary>  
        /// 应结退款金额    Int    必填(否)
        /// </summary>
        public string settlement_refund_fee { get; set; }
        /// <summary>  
        /// 标价金额    Int    必填(是)
        /// </summary>
        public string total_fee { get; set; }
        /// <summary>  
        /// 应结订单金额    Int    必填(否)
        /// </summary>
        public string settlement_total_fee { get; set; }
        /// <summary>  
        /// 标价币种    String(8)    必填(否)
        /// </summary>
        public string fee_type { get; set; }
        /// <summary>  
        /// 现金支付金额    Int    必填(是)
        /// </summary>
        public string cash_fee { get; set; }
        /// <summary>  
        /// 现金支付币种    String(16)    必填(否)
        /// </summary>
        public string cash_fee_type { get; set; }
        /// <summary>  
        /// 现金退款金额    Int    必填(否)
        /// </summary>
        public string cash_refund_fee { get; set; }
        /// <summary>  
        /// 代金券类型    String(8)    必填(否)
        /// </summary>
        //public string coupon_type_$n{get;set;}
        /// <summary>  
        /// 代金券退款总金额    Int    必填(否)
        /// </summary>
        public string coupon_refund_fee { get; set; }
        /// <summary>  
        /// 单个代金券退款金额    Int    必填(否)
        /// </summary>
        //public string coupon_refund_fee_$n{get;set;}
        /// <summary>  
        /// 退款代金券使用数量    Int    必填(否)
        /// </summary>
        public string coupon_refund_count { get; set; }
        /// <summary>  
        /// 退款代金券ID    String(20)    必填(否)
        /// </summary>
        //public string coupon_refund_id_$n{get;set;}
    }
}
