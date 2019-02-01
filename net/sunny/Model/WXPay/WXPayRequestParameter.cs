using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model.WXPay
{

    /// <summary>
    /// 统一下单请求参数
    /// </summary>
    public class WXPayRequestParameter
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
        /// 签名类型    String(32)    必填(否)
        /// </summary>
        public string sign_type { get; set; }
        /// <summary>  
        /// 商品描述    String(128)    必填(是)
        /// </summary>
        public string body { get; set; }
        /// <summary>  
        /// 商品详情    String(6000)    必填(否)
        /// </summary>
        public string detail { get; set; }
        /// <summary>  
        /// 附加数据    String(127)    必填(否)
        /// </summary>
        public string attach { get; set; }
        /// <summary>  
        /// 商户订单号    String(32)    必填(是)
        /// </summary>
        public string out_trade_no { get; set; }
        /// <summary>  
        /// 标价币种    String(16)    必填(否)
        /// </summary>
        public string fee_type { get; set; }
        /// <summary>  
        /// 标价金额(单位分)    Int    必填(是)
        /// </summary>
        public int total_fee { get; set; }
        /// <summary>  
        /// 终端IP    String(64)    必填(是)
        /// </summary>
        public string spbill_create_ip { get; set; }
        /// <summary>  
        /// 交易起始时间    String(14)    必填(否)
        /// </summary>
        public string time_start { get; set; }
        /// <summary>  
        /// 交易结束时间    String(14)    必填(否)
        /// </summary>
        public string time_expire { get; set; }
        /// <summary>  
        /// 订单优惠标记    String(32)    必填(否)
        /// </summary>
        public string goods_tag { get; set; }
        /// <summary>  
        /// 通知地址    String(256)    必填(是)
        /// </summary>
        public string notify_url { get; set; }
        /// <summary>  
        /// 交易类型    String(16)    必填(是)
        /// </summary>
        public string trade_type { get; set; }
        /// <summary>  
        /// 商品ID    String(32)    必填(否)
        /// </summary>
        public string product_id { get; set; }
        /// <summary>  
        /// 指定支付方式    String(32)    必填(否)
        /// </summary>
        public string limit_pay { get; set; }
        /// <summary>  
        /// 用户标识    String(128)    必填(否)
        /// </summary>
        public string openid { get; set; }
        /// <summary>  
        /// 电子发票入口开放标识    String(8)    必填(否)
        /// </summary>
        public string receipt { get; set; }
        /// <summary>  
        /// +场景信息    String(256)    必填(否)
        /// </summary>
        public string scene_info { get; set; }
    }
}
