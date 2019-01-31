using Sunny.BLL.API;
using Sunny.Model;
using Sunny.Model.WXPay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Sunny.API.Controllers
{
    public class WeiXinPayController : ApiController
    {

        [HttpGet]
        [Route("api/wxpay/prepay")]
        public IHttpActionResult GetPrePayInfo(string orderId, string openId)
        {
            ResponseResult result = null;
            try
            {
                WXPayToClientPara para = WeiXinPayBLL.SendPreOrder(orderId, openId);
                result = new ResponseResult(0, "ok", para);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"api/wxpay/prepay 出错 \r\n {e}", Util.Log.LogType.Error);
                result = new ResponseResult(-1, "服务内部错误", null);
            }

            return Json(result);
        }
    }
}
