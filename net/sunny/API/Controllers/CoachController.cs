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
            ResponseResult result = null;
            try
            {
                Coach coach = GeneralBLL.GetCoachByUserName(token);
                //result.password = string.Empty;
                result = new ResponseResult(0, "ok", coach);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"api/coach/GetById 出错 token {token} \r\n {e}", Util.Log.LogType.Error);
                result = new ResponseResult(-1, "服务内部错误", null);
            }
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
                result = new ResponseResult(0, "ok", serverCount > 0);
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
                result = new ResponseResult(1, " 参数不全", null);
            }
            else
            {
                //服务器是否已存在该用户名
                int serverCount = DBData.GetInstance(DBTable.coach).GetCount($"username='{data.username}'");

                if (serverCount > 0)
                {   //已存在
                    result = new ResponseResult(2, "该账号已存在", null);
                }
                else
                {
                    bool isPhoneExist = DBData.GetInstance(DBTable.coach).GetCount($"phone='{data.phone}'") > 0;
                    if (isPhoneExist)
                    {
                        result = new ResponseResult(3, "手机号已注册", null);
                    }
                    else
                    {
                        //验证教练手机号
                        if (CoachDAL.IsCaptionPhoneExist(data.CaptionPhone))
                        {
                            Coach caption = DBData.GetInstance(DBTable.coach).GetEntity<Coach>("phone='data.CaptionPhone'");
                            bool isOk = CoachDAL.AddCoach(data, caption);
                            int code = isOk ? 0 : 4;
                            string msg = isOk ? "注册成功" : "注册失败";
                            result = new ResponseResult(code, msg, isOk);
                        }
                        else
                        {
                            result = new ResponseResult(5, "教练队长手机号不正确", null);
                        }
                    }
                }
            }

            return Json(result);
        }

        [HttpGet]
        [Route("api/coach/img")]
        public IHttpActionResult GetImages(string token)
        {
            ResponseResult result = null;
            try
            {
                Coach coach = GeneralBLL.GetCoachByUserName(token);
                IList<CoachImg> images = DBData.GetInstance(DBTable.coach_img).GetList<CoachImg>($"coach_id='{coach.id}' and state=0");
                result = new ResponseResult(0, "ok", images);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"api/coach/GetImages 出错 token {token} \r\n {e}", Util.Log.LogType.Error);
                result = new ResponseResult(-1, "服务内部错误", null);
            }
            return Json(result);
        }
    }
}
