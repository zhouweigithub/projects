using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model.WXPay
{
    /// <summary>
    /// 微信支付关闭订单结果
    /// </summary>
    public class WXPayCloseOrderResult
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
        /// 业务结果描述    String(32)    必填(是)
        /// </summary>
        public string result_msg;
        /// <summary>  
        /// 错误代码    String(32)    必填(否)
        /// </summary>
        public string err_code;
        /// <summary>  
        /// 错误代码描述    String(128)    必填(否)
        /// </summary>
        public string err_code_des;
    }
}
