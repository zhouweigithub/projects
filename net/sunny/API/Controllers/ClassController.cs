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
    public class ClassController : ApiController
    {

        [Route("get")]
        [HttpGet]
        public IHttpActionResult GetById(int id)
        {
            Class result = DBData.GetInstance(DBTable.class_).GetEntityByKey<Class>(id);
            return Json(result);
        }

        [Route("bystudent")]
        [HttpGet]
        public IHttpActionResult GetByStudent(int sudentid, short state)
        {
            List<Class> classList = ClassDAL.GetClassByStudentId(sudentid, state);
            return Json(classList);
        }

        [Route("bycoach")]
        [HttpGet]
        public IHttpActionResult GetByCoach(int coachid, short state)
        {
            List<Class> classList = ClassDAL.GetClassByCoachId(coachid, state);
            return Json(classList);
        }
    }
}
