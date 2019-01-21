using Sunny.BLL.API;
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
        [Route("api/appointment/get")]
        public IHttpActionResult Get(string token)
        {
            int student_id = DBData.GetInstance(DBTable.student).GetEntity<Student>($"username='{token}'").id;
            List<AppointmentCourseListJson> result = AppointmentDAL.GetCourseInfoList(student_id, 0);
            return Json(result);
        }

        [HttpGet]
        [Route("api/appointment/get")]
        public IHttpActionResult Get(string token, int productId)
        {
            int student_id = DBData.GetInstance(DBTable.student).GetEntity<Student>($"username='{token}'").id;
            List<AppointmentCourseListJson> result = AppointmentDAL.GetCourseInfoList(student_id, productId);
            return Json(result);
        }

        /// <summary>
        /// 获取时间还没到的上课时间点
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/appointment/GetBookedTimes")]
        public IHttpActionResult GetBookingTimes(string token)
        {
            int student_id = DBData.GetInstance(DBTable.student).GetEntity<Student>($"username='{token}'").id;
            List<Class> datas = AppointmentDAL.GetAppointmentedInfo(student_id);
            var times = datas.Select(a => a.start_time).ToArray();
            return Json(times);
        }

        //获取教练可接单的预约信息
        [HttpGet]
        [Route("api/appointment/GetByCoach")]
        public IHttpActionResult GetByCoach(string token)
        {
            int coach_id = DBData.GetInstance(DBTable.coach).GetEntity<Coach>($"username='{token}'").id;
            List<ClassBookingOfCoachJson> classList = ClassDAL.GetBookingListOfCoach(coach_id);
            return Json(classList);
        }

        //用户发送预约请求
        [HttpPost]
        [Route("api/appointment/SendBooking")]
        public IHttpActionResult SendBooking(int courseId, DateTime startTime, DateTime endTime)
        {
            bool result = AppointmentBLL.AddAppointment(courseId, startTime, endTime);
            return Json(result);
        }

        //教练接受预约
        [HttpPost]
        [Route("api/appointment/ReceiveBooking")]
        public IHttpActionResult ReceiveBooking(int bookingId, string token)
        {
            int coach_id = DBData.GetInstance(DBTable.coach).GetEntity<Coach>($"username='{token}'").id;
            bool result = AppointmentBLL.ReceiveAppointment(bookingId, coach_id);
            return Json(result);
        }
    }
}
