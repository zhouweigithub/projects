using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sunny.BLL.API;
using Sunny.Common;
using Sunny.DAL;
using Sunny.Model;

namespace API.Controllers
{
    public class CoachController : ApiController
    {

        [HttpGet]
        [Route("api/coach/get")]
        public IHttpActionResult GetById(string token)
        {
            Coach result = DBData.GetInstance(DBTable.coach).GetEntity<Coach>($"username='{token}'");
            result.password = string.Empty;
            return Json(result);
        }

        [HttpGet]
        [Route("api/coach/isexist")]
        public IHttpActionResult IsExist(string token)
        {
            ResponseResult result = null;
            if (string.IsNullOrWhiteSpace(token))
            {
                result = new ResponseResult(-1, "参数不全");
            }
            else
            {
                int serverCount = DBData.GetInstance(DBTable.coach).GetCount($"username='{token}'");
                if (serverCount == 0)
                    result = new ResponseResult(0, "ok", false);
                else
                    result = new ResponseResult(0, "ok", true);
            }

            return Json(result);
        }

        [HttpPost]
        [Route("api/coach/create")]
        public IHttpActionResult Create(CoachRequest data)
        {
            ResponseResult result = null;

            if (string.IsNullOrWhiteSpace(data.username) || string.IsNullOrWhiteSpace(data.phone)
                || string.IsNullOrWhiteSpace(data.CaptionPhone))
            {
                result = new ResponseResult(-1, " 参数不全");
            }
            else
            {
                //服务器是否已存在该用户名
                int serverCount = DBData.GetInstance(DBTable.coach).GetCount($"username='{data.username}'");

                if (serverCount > 0)
                {   //已存在
                    result = new ResponseResult(-1, "该账号已存在");
                }
                else
                {
                    //验证教练手机号
                    if (CoachDAL.IsCaptionPhoneExist(data.CaptionPhone))
                    {
                        int insertCount = DBData.GetInstance(DBTable.coach).Add(data);
                        int code = insertCount > 0 ? 0 : -1;
                        string msg = insertCount > 0 ? "ok" : "fail";
                        result = new ResponseResult(code, msg);
                    }
                    else
                    {
                        result = new ResponseResult(-1, "教练队长手机号不正确");
                    }
                }
            }

            return Json(result);
        }

    }
}
