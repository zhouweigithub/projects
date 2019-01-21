using Sunny.DAL;
using Sunny.Model;
using Sunny.Model.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    public class CouponController : ApiController
    {

        [HttpGet]
        [Route("api/coupon/get")]
        public IHttpActionResult Get(string token)
        {
            int student_id = DBData.GetInstance(DBTable.student).GetEntity<Student>($"username='{token}'").id;
            List<CouponListJson> result = CouponDAL.GetCouponList(student_id);
            return Json(result);
        }

        [HttpGet]
        [Route("api/coupon/GetDefault")]
        public IHttpActionResult Get(string token, int categoryId)
        {
            int student_id = DBData.GetInstance(DBTable.student).GetEntity<Student>($"username='{token}'").id;
            CouponListJson result = CouponDAL.GetCouponDefaultOfStudent(student_id, categoryId);
            return Json(result);
        }

    }
}
