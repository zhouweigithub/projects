using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model.WXPay
{
    /// <summary>
    /// 查询支付情况的结果
    /// </summary>
    public class WXPayQueryOrderResult
    {
        /// <summary>  
        /// 返回状态码    String(16)    必填(是)
        /// </summary>
        public string return_code;
        /// <summary>  
        /// 返回信息    String(128)    必填(否)
        /// </summary>
        public string return_msg;
        /// <summary>  
        /// 小程序ID    String(32)    必填(是)
        /// </summary>
        public string appid;
        /// <summary>  
        /// 商户号    String(32)    必填(是)
        /// </summary>
        public string mch_id;
        /// <summary>  
        /// 随机字符串    String(32)    必填(是)
        /// </summary>
        public string nonce_str;
        /// <summary>  
        /// 签名    String(32)    必填(是)
        /// </summary>
        public string sign;
        /// <summary>  
        /// 业务结果    String(16)    必填(是)
        /// </summary>
        public string result_code;
        /// <summary>  
        /// 错误代码    String(32)    必填(否)
        /// </summary>
        public string err_code;
        /// <summary>  
        /// 错误代码描述    String(128)    必填(否)
        /// </summary>
        public string err_code_des;
        /// <summary>  
        /// 字段名    类型    必填(必填)
        /// </summary>
        public string 变量名;
        /// <summary>  
        /// 设备号    String(32)    必填(否)
        /// </summary>
        public string device_info;
        /// <summary>  
        /// 用户标识    String(128)    必填(是)
        /// </summary>
        public string openid;
        /// <summary>  
        /// 是否关注公众账号    String(1)    必填(是)
        /// </summary>
        public string is_subscribe;
        /// <summary>  
        /// 交易类型    String(16)    必填(是)
        /// </summary>
        public string trade_type;
        /// <summary>  
        /// 交易状态    String(32)    必填(是) SUCCESS—支付成功        REFUND—转入退款        
        /// NOTPAY—未支付        CLOSED—已关闭        REVOKED—已撤销（刷卡支付）
        /// USERPAYING--用户支付中       PAYERROR--支付失败(其他原因，如银行返回失败)
        /// </summary>
        public string trade_state;
        /// <summary>  
        /// 付款银行    String(16)    必填(是)
        /// </summary>
        public string bank_type;
        /// <summary>  
        /// 标价金额    Int    必填(是)
        /// </summary>
        public string total_fee;
        /// <summary>  
        /// 应结订单金额    Int    必填(否)
        /// </summary>
        public string settlement_total_fee;
        /// <summary>  
        /// 标价币种    String(8)    必填(否)
        /// </summary>
        public string fee_type;
        /// <summary>  
        /// 现金支付金额    Int    必填(是)
        /// </summary>
        public string cash_fee;
        /// <summary>  
        /// 现金支付币种    String(16)    必填(否)
        /// </summary>
        public string cash_fee_type;
        /// <summary>  
        /// 代金券金额    Int    必填(否)
        /// </summary>
        public string coupon_fee;
        /// <summary>  
        /// 代金券使用数量    Int    必填(否)
        /// </summary>
        public string coupon_count;
        /// <summary>  
        /// 微信支付订单号    String(32)    必填(是)
        /// </summary>
        public string transaction_id;
        /// <summary>  
        /// 商户订单号    String(32)    必填(是)
        /// </summary>
        public string out_trade_no;
        /// <summary>  
        /// 附加数据    String(128)    必填(否)
        /// </summary>
        public string attach;
        /// <summary>  
        /// 支付完成时间    String(14)    必填(是)
        /// </summary>
        public string time_end;
        /// <summary>  
        /// 交易状态描述    String(256)    必填(是)
        /// </summary>
        public string trade_state_desc;

    }
}
