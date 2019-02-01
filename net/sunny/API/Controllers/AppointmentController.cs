using Sunny.BLL.API;
using Sunny.DAL;
using Sunny.Model;
using Sunny.Model.Custom;
using Sunny.Model.Request;
using Sunny.Model.Response;
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
        //学生预约信息
        [HttpGet]
        [Route("api/appointment/get")]
        public IHttpActionResult Get(string token)
        {
            ResponseResult result = null;
            try
            {
                int student_id = GeneralBLL.GetStudentByUserName(token).id;
                List<AppointmentCourseListJson> list = AppointmentDAL.GetCourseInfoList(student_id, 0);
                result = new ResponseResult(0, "ok", list);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"api/appointment/get 出错 token {token} \r\n {e}", Util.Log.LogType.Error);
                result = new ResponseResult(-1, "服务内部错误", null);
            }
            return Json(result);
        }

        //学生预约信息
        [HttpGet]
        [Route("api/appointment/get")]
        public IHttpActionResult Get(string token, int productId)
        {
            ResponseResult result = null;
            try
            {
                int student_id = GeneralBLL.GetStudentByUserName(token).id;
                List<AppointmentCourseListJson> list = AppointmentDAL.GetCourseInfoList(student_id, productId);
                result = new ResponseResult(0, "ok", list);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"api/appointment/get 出错 token {token} productId {productId} \r\n {e}", Util.Log.LogType.Error);
                result = new ResponseResult(-1, "服务内部错误", null);
            }
            return Json(result);
        }

        //时间还没到的上课时间点
        [HttpGet]
        [Route("api/appointment/GetStudentBookedTimes")]
        public IHttpActionResult GetStudentBookedTimes(string token)
        {
            ResponseResult result = null;
            try
            {
                int student_id = GeneralBLL.GetStudentByUserName(token).id;
                List<Class> datas = AppointmentDAL.GetStudentAppointmentedInfo(student_id);
                var times = datas.Select(a => a.start_time).ToArray();
                result = new ResponseResult(0, "ok", times);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"api/appointment/GetStudentBookedTimes 出错 token {token}  \r\n {e}", Util.Log.LogType.Error);
                result = new ResponseResult(-1, "服务内部错误", null);
            }
            return Json(result);
        }

        //时间还没到的上课时间点
        [HttpGet]
        [Route("api/appointment/GetCoachBookedClass")]
        public IHttpActionResult GetCoachBookedClass(string token)
        {
            ResponseResult result = null;
            try
            {
                int student_id = GeneralBLL.GetStudentByUserName(token).id;
                List<CoachAppointedClassJson> datas = AppointmentDAL.GetCoachAppointmentedClassInfo(student_id);
                result = new ResponseResult(0, "ok", datas);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"api/appointment/GetCoachBookedClass 出错 token {token}  \r\n {e}", Util.Log.LogType.Error);
                result = new ResponseResult(-1, "服务内部错误", null);
            }
            return Json(result);
        }

        //获取教练可接单的预约信息
        [HttpGet]
        [Route("api/appointment/GetByCoach")]
        public IHttpActionResult GetByCoach(string token)
        {
            ResponseResult result = null;
            try
            {
                int coach_id = GeneralBLL.GetCoachByUserName(token).id;
                List<ClassBookingOfCoachJson> classList = AppointmentBLL.GetAppointmentOfCoach(coach_id);
                result = new ResponseResult(0, "ok", classList);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"api/appointment/GetByCoach 出错 token {token}  \r\n {e}", Util.Log.LogType.Error);
                result = new ResponseResult(-1, "服务内部错误", null);
            }
            return Json(result);
        }

        //用户发送预约请求
        [HttpPost]
        [Route("api/appointment/SendBooking")]
        public IHttpActionResult SendBooking(SendBookingRequest request)
        {
            ResponseResult result = null;
            try
            {
                bool isOk = AppointmentBLL.AddAppointment(request.courseId, request.startTime, request.endTime);
                result = new ResponseResult(0, "ok", isOk);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"api/appointment/SendBooking 出错 courseId {request.courseId} startTime {request.startTime} endTime {request.endTime}  \r\n {e}", Util.Log.LogType.Error);
                result = new ResponseResult(-1, "服务内部错误", null);
            }
            return Json(result);
        }

        //教练接受预约
        [HttpPost]
        [Route("api/appointment/ReceiveBooking")]
        public IHttpActionResult ReceiveBooking(ReceiveBookingRequest request)
        {
            ResponseResult result = null;
            try
            {
                int coach_id = GeneralBLL.GetCoachByUserName(request.token).id;
                bool isOk = AppointmentBLL.ReceiveAppointment(request.bookingId, coach_id, request.startTime, request.endTime, out string msg);
                result = new ResponseResult(0, "ok", isOk);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"api/appointment/ReceiveBooking 出错 bookingId {request.bookingId} startTime {request.startTime} endTime {request.endTime}  \r\n {e}", Util.Log.LogType.Error);
                result = new ResponseResult(-1, "服务内部错误", null);
            }
            return Json(result);
        }
    }
}
