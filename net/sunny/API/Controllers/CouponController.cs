using Sunny.DAL;
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

        // GET: api/Coupon/5
        public IHttpActionResult Get(int studentId)
        {
            List<CouponListJson> result = CouponDAL.GetCouponList(studentId);
            return Json(result);
        }

        [Route("GetDefault")]
        [HttpGet]
        public IHttpActionResult Get(int studentId, int categoryId)
        {
            CouponListJson result = CouponDAL.GetCouponDefaultOfStudent(studentId, categoryId);
            return Json(result);
        }
    }
}
