using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model.WXPay
{
    /// <summary>
    /// 接收通知后回调给微信的参数
    /// </summary>
    public class WXPayResultBackToWX
    {
        private string _return_code;
        private string _return_msg;

        public WXPayResultBackToWX(string returnCode, string returnMsg)
        {
            _return_code = returnCode;
            _return_msg = returnMsg;
        }

        public string return_code { get => $"<![CDATA[{_return_code}]]>"; }
        public string return_msg { get => $"<![CDATA[{_return_msg}]]>"; }
    }
}
