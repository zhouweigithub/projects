using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
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

        // POST: api/Student
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
                    int insertCount = DBData.GetInstance(DBTable.student).Add(data);
                    int code = insertCount > 0 ? 0 : -1;
                    string msg = insertCount > 0 ? "ok" : "fail";
                    result = new ResponseResult(code, msg);
                }
            }

            return Json(result);
        }

    }
}
