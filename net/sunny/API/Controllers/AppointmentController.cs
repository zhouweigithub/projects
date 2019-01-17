﻿using Sunny.BLL.API;
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

        /// <summary>
        /// 获取时间还没到的上课时间点
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Appointment/GetBookedTimes")]
        public IHttpActionResult GetBookingTimes(int studentId)
        {
            List<Class> datas = AppointmentDAL.GetAppointmentedInfo(studentId);
            var times = datas.Select(a => a.start_time).ToArray();
            return Json(times);
        }

        //获取教练可接单的预约信息
        [HttpGet]
        [Route("api/Appointment/GetByCoach")]
        public IHttpActionResult GetByCoach(int coachid)
        {
            List<ClassBookingOfCoachJson> classList = ClassDAL.GetBookingListOfCoach(coachid);
            return Json(classList);
        }

        //用户发送预约请求
        [HttpPost]
        [Route("api/Appointment/SendBooking")]
        public IHttpActionResult SendBooking(int courseId, DateTime startTime, DateTime endTime)
        {
            bool result = AppointmentBLL.AddAppointment(courseId, startTime, endTime);
            return Json(result);
        }

        //教练接收预约
        [HttpPost]
        [Route("api/Appointment/ReceiveBooking")]
        public IHttpActionResult ReceiveBooking(int bookingId, int coachId)
        {
            bool result = AppointmentBLL.ReceiveAppointment(bookingId, coachId);
            return Json(result);
        }
    }
}
