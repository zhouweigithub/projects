using Sunny.BLL.Page;
using Sunny.Common;
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
        public string Get(SmsVerificationCodeTypeEnum type, string phone)
        {
            return CommonBLL.CreateSmsVerificationCode(type, phone);
        }

        [HttpPost]
        public bool IsSmsVerificationCodeOk(SmsVerificationCodeTypeEnum type, string phone, string code)
        {
            string serverCode = CommonBLL.GetSmsVerificationCodeFromCache(type, phone);
            return serverCode == code;
        }

    }
}
