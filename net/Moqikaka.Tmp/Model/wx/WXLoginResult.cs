using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moqikaka.Tmp.Model.wx
{

    /// <summary>
    /// 微信登录返回数据
    /// </summary>
    public class WXLoginResult
    {
        public string session_key { get; set; }
        public string openid { get; set; }
    }
}
