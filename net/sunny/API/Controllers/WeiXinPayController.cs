using Sunny.BLL.API;
using Sunny.Model;
using Sunny.Model.WXPay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using Sunny.Common;

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

        [HttpPost]
        [Route("api/wxpay/paynotify")]
        public IHttpActionResult PayNotify(HttpContext context)
        {
            ResponseResult result = null;
            try
            {
                WXPayResultBackToWX para = WeiXinPayBLL.PayNotify(context);
                string xml = XmlHelper.XmlSerialize(para, Encoding.UTF8);
                xml = xml.Replace("<WXPayResultBackToWX>", "<xml>").Replace("</WXPayResultBackToWX>", "</xml>");
                Util.Log.LogUtil.Write($"支付回调返回结果：{xml}", Util.Log.LogType.Debug);
                context.Response.Write(xml);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"api/wxpay/paynotify 出错 \r\n {e}", Util.Log.LogType.Error);
                result = new ResponseResult(-1, "服务内部错误", null);
            }

            return Json(result);
        }

    }
}
