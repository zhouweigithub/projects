using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model.Custom
{

    /// <summary>
    /// 微信登录返回数据
    /// </summary>
    public class CustWXLoginResult
    {
        public string session_key { get; set; }
        public string openid { get; set; }
    }
}
