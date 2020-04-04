using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model.WXPay
{
    /// <summary>
    /// 统一下单返回结果
    /// </summary>
    public class WXPayRequestResult
    {
        /// <summary>  
        /// 返回状态码    String(16)    必填(是)
        /// </summary>
        public string return_code { get; set; }
        /// <summary>  
        /// 返回信息    String(128)    必填(否)
        /// </summary>
        public string return_msg { get; set; }

        // 以下字段在return_code为SUCCESS的时候有返回

        /// <summary>  
        /// 小程序ID    String(32)    必填(是)
        /// </summary>
        public string appid { get; set; }
        /// <summary>  
        /// 商户号    String(32)    必填(是)
        /// </summary>
        public string mch_id { get; set; }
        /// <summary>  
        /// 设备号    String(32)    必填(否)
        /// </summary>
        public string device_info { get; set; }
        /// <summary>  
        /// 随机字符串    String(32)    必填(是)
        /// </summary>
        public string nonce_str { get; set; }
        /// <summary>  
        /// 签名    String(32)    必填(是)
        /// </summary>
        public string sign { get; set; }
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

        // 以下字段在return_code 和result_code都为SUCCESS的时候有返回

        /// <summary>  
        /// 交易类型    String(16)    必填(是)
        /// </summary>
        public string trade_type { get; set; }
        /// <summary>  
        /// 预支付交易会话标识    String(64)    必填(是)
        /// </summary>
        public string prepay_id { get; set; }
        /// <summary>  
        /// 二维码链接    String(64)    必填(否)
        /// </summary>
        public string code_url { get; set; }
    }
}
