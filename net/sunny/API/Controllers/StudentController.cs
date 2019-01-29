using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sunny.BLL.API;
using Sunny.BLL.Page;
using Sunny.DAL;
using Sunny.Model;
using Sunny.Model.Response;

namespace API.Controllers
{
    public class StudentController : ApiController
    {

        [HttpGet]
        [Route("api/student/get")]
        public IHttpActionResult GetById(string token)
        {
            ResponseResult result = null;
            try
            {
                Student obj = DBData.GetInstance(DBTable.student).GetEntity<Student>($"username='{token}'");
                //result.password = string.Empty;
                result = new ResponseResult(0, "ok", obj);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"api/student/Get 出错 token {token} \r\n {e}", Util.Log.LogType.Error);
                result = new ResponseResult(-1, "服务内部错误", null);
            }
            return Json(result);
        }

        [HttpGet]
        [Route("api/student/isexist")]
        public IHttpActionResult IsExist(string token)
        {
            ResponseResult result = null;
            if (string.IsNullOrWhiteSpace(token))
            {
                result = new ResponseResult(-1, "参数不全");
            }
            else
            {
                int serverCount = DBData.GetInstance(DBTable.student).GetCount($"username='{token}'");
                result = new ResponseResult(0, "ok", serverCount > 0);
            }

            return Json(result);
        }

        [HttpPost]
        [Route("api/student/create")]
        public IHttpActionResult Create(StudentRequest data)
        {
            ResponseResult result = null;

            if (string.IsNullOrWhiteSpace(data.username) || string.IsNullOrWhiteSpace(data.phone)
                || string.IsNullOrWhiteSpace(data.Invitationcode))
            {
                result = new ResponseResult(1, " 参数不全", null);
            }
            else
            {
                //服务器是否已存在该用户名
                int serverCount = DBData.GetInstance(DBTable.student).GetCount($"username='{data.username}'");

                if (serverCount > 0)
                {   //已存在
                    result = new ResponseResult(2, "该账号已存在", null);
                }
                else
                {
                    bool isPhoneExist = DBData.GetInstance(DBTable.student).GetCount($"phone='{data.phone}'") > 0;
                    if (isPhoneExist)
                    {
                        result = new ResponseResult(3, "手机号已注册", null);
                    }
                    else
                    {
                        bool isInvitationCodeExist = DBData.GetInstance(DBTable.student).GetCount($"phone='{data.Invitationcode}'") > 0;
                        if (isInvitationCodeExist)
                        {
                            //string smsServerCode = CommonBLL.GetSmsVerificationCodeFromCache(Sunny.Common.SmsVerificationCodeTypeEnum.StudentRegister, data.phone);
                            //if (smsServerCode == data.SmsVerificationCode)
                            //{
                            bool isAddOk = StudentDAL.AddStudent(data);
                            int code = isAddOk ? 0 : 4;
                            string msg = isAddOk ? "注册成功" : "注册失败";
                            result = new ResponseResult(code, msg, isAddOk);
                            //}
                            //else
                            //{
                            //    result = new ResponseResult(-1, "短信验证码不正确");
                            //}
                        }
                        else
                        {
                            result = new ResponseResult(5, "邀请码不正确", null);
                        }
                    }
                }
            }

            return Json(result);
        }

        [HttpGet]
        [Route("api/student/invations")]
        public IHttpActionResult InvationData(string token)
        {
            ResponseResult result = null;
            try
            {
                int student_id = DBData.GetInstance(DBTable.student).GetEntity<Student>($"username='{token}'").id;
                CashbackHistoryJson data = CashbackHistoryBLL.GetCashbackData(student_id);
                result = new ResponseResult(0, "ok", data);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"api/student/invations 出错 \r\n {e}", Util.Log.LogType.Error);
                result = new ResponseResult(-1, "服务内部错误", null);
            }

            return Json(result);
        }


    }
}
