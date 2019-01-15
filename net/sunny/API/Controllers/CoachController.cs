﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sunny.DAL;
using Sunny.Model;

namespace API.Controllers
{
    public class CoachController : ApiController
    {

        [HttpGet]
        public IHttpActionResult GetById(int id)
        {
            Coach result = DBData.GetInstance(DBTable.coach).GetEntityByKey<Coach>(id);
            result.password = string.Empty;
            return Json(result);
        }

        [HttpGet]
        [Route("api/coach/isexist")]
        public IHttpActionResult IsExist(string username)
        {
            ResponseResult result = null;
            if (string.IsNullOrWhiteSpace(username))
            {
                result = new ResponseResult(-1, "参数不全");
            }
            else
            {
                int serverCount = DBData.GetInstance(DBTable.coach).GetCount($"username='{username}'");
                if (serverCount == 0)
                    result = new ResponseResult(0, "ok", false);
                else
                    result = new ResponseResult(0, "ok", true);
            }

            return Json(result);
        }

        [HttpPost]
        [Route("api/coach/Create")]
        public IHttpActionResult Create(CoachRequest data)
        {
            ResponseResult result = null;

            if (string.IsNullOrWhiteSpace(data.username) || string.IsNullOrWhiteSpace(data.password) || string.IsNullOrWhiteSpace(data.phone)
                || string.IsNullOrWhiteSpace(data.CaptionPhone) || string.IsNullOrWhiteSpace(data.SmsVerificationCode))
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
                    int insertCount = DBData.GetInstance(DBTable.coach).Add(data);
                    int code = insertCount > 0 ? 0 : -1;
                    string msg = insertCount > 0 ? "ok" : "fail";
                    result = new ResponseResult(code, msg);
                }
            }

            return Json(result);
        }

    }
}
