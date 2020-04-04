using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model.WXPay
{
    /// <summary>
    /// 微信支付统一下完成后单返回给小程序的结果
    /// </summary>
    public class WXPayToClientPara
    {
        public string appId { get; set; }
        public string timeStamp { get; set; }
        public string nonceStr { get; set; }
        public string package { get; set; }
        public string signType { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string return_msg { get; set; }
    }
}
