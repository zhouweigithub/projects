using Sunny.BLL.Page;
using Sunny.Common;
using Sunny.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    public class SmsVerificationCodeController : ApiController
    {
        [HttpGet]
        [Route("api/sms/get")]
        public IHttpActionResult Get(SmsVerificationCodeTypeEnum type, string phone)
        {
            ResponseResult result = null;
            try
            {
                string code = CommonBLL.CreateSmsVerificationCode(type, phone);
                result = new ResponseResult(0, "ok", code);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"api/sms/Get 出错 type {type} phone {phone} \r\n {e}", Util.Log.LogType.Error);
                result = new ResponseResult(-1, "服务内部错误", null);
            }
            return Json(result);
        }

        [HttpGet]
        [Route("api/sms/isok")]
        public IHttpActionResult IsSmsVerificationCodeOk(SmsVerificationCodeTypeEnum type, string phone, string code)
        {
            ResponseResult result = null;
            try
            {
                string serverCode = CommonBLL.GetSmsVerificationCodeFromCache(type, phone);
                result = new ResponseResult(0, "ok", serverCode == code);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"api/sms/isok 出错 type {type} phone {phone} code {code} \r\n {e}", Util.Log.LogType.Error);
                result = new ResponseResult(-1, "服务内部错误", null);
            }
            return Json(result);
        }

    }
}
