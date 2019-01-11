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
    public class AppointmentController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get(int studentId)
        {
            List<AppointmentCourseListJson> result = AppointmentDAL.GetCourseInfoList(studentId, 0);
            return Json(result);
        }

        [HttpGet]
        public IHttpActionResult Get(int studentId, int productId)
        {
            List<AppointmentCourseListJson> result = AppointmentDAL.GetCourseInfoList(studentId, productId);
            return Json(result);
        }

        [HttpGet]
        public IHttpActionResult GetBookingTimes(int studentId)
        {
            List<Class> datas = AppointmentDAL.GetAppointmentedInfo(studentId);
            var times = datas.Select(a => a.start_time).ToArray();
            return Json(times);
        }
    }
}
