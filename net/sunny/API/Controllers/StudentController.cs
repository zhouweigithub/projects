using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sunny.BLL.Page;
using Sunny.DAL;
using Sunny.Model;

namespace API.Controllers
{
    public class StudentController : ApiController
    {

        [HttpGet]
        [Route("api/student/get")]
        public IHttpActionResult GetById(int id)
        {
            Student result = DBData.GetInstance(DBTable.student).GetEntityByKey<Student>(id);
            result.password = string.Empty;
            return Json(result);
        }

        [HttpGet]
        [Route("api/student/isexist")]
        public IHttpActionResult IsExist(string username)
        {
            ResponseResult result = null;
            if (string.IsNullOrWhiteSpace(username))
            {
                result = new ResponseResult(-1, "参数不全");
            }
            else
            {
                int serverCount = DBData.GetInstance(DBTable.student).GetCount($"username='{username}'");
                if (serverCount == 0)
                    result = new ResponseResult(0, "ok", false);
                else
                    result = new ResponseResult(0, "ok", true);
            }

            return Json(result);
        }

        [HttpPost]
        [Route("api/student/create")]
        public IHttpActionResult Create(StudentRequest data)
        {
            ResponseResult result = null;

            if (string.IsNullOrWhiteSpace(data.username) || string.IsNullOrWhiteSpace(data.password) || string.IsNullOrWhiteSpace(data.phone)
                || string.IsNullOrWhiteSpace(data.Invitationcode) || string.IsNullOrWhiteSpace(data.SmsVerificationCode))
            {
                result = new ResponseResult(-1, " 参数不全");
            }
            else
            {
                //服务器是否已存在该用户名
                int serverCount = DBData.GetInstance(DBTable.student).GetCount($"username='{data.username}'");

                if (serverCount > 0)
                {   //已存在
                    result = new ResponseResult(-1, "该账号已存在");
                }
                else
                {
                    bool isInvitationCodeExist = DBData.GetInstance(DBTable.student).GetCount($"phone='{data.Invitationcode}'") > 0;
                    if (isInvitationCodeExist)
                    {
                        string smsServerCode = CommonBLL.GetSmsVerificationCodeFromCache(Sunny.Common.SmsVerificationCodeTypeEnum.StudentRegister, data.phone);
                        if (smsServerCode == data.SmsVerificationCode)
                        {
                            bool isAddOk = StudentDAL.AddStudent(data);
                            int code = isAddOk ? 0 : -1;
                            string msg = isAddOk ? "ok" : "fail";
                            result = new ResponseResult(code, msg);
                        }
                        else
                        {
                            result = new ResponseResult(-1, "短信验证码不正确");
                        }
                    }
                    else
                    {
                        result = new ResponseResult(-1, "邀请码不正确");
                    }
                }
            }

            return Json(result);
        }

    }
}
