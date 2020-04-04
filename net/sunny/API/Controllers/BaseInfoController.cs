using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sunny.BLL.API;
using Sunny.DAL;
using Sunny.Model;

namespace API.Controllers
{
    public class BaseInfoController : ApiController
    {
        [HttpGet]
        [Route("api/BaseInfo/GetCampus")]
        public IHttpActionResult GetCampus()
        {
            ResponseResult result = null;
            try
            {
                IList<Campus> list = DBData.GetInstance(DBTable.campus).GetList<Campus>("state=0");
                result = new ResponseResult(0, "ok", list);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"api/BaseInfo/GetCampus 出错 \r\n {e}", Util.Log.LogType.Error);
                result = new ResponseResult(-1, "服务内部错误", null);
            }
            return Json(result);
        }

        [HttpGet]
        [Route("api/BaseInfo/GetCampusDic")]
        public IHttpActionResult GetCampusDic()
        {
            ResponseResult result = null;
            try
            {
                Dictionary<int, List<Venue>> list = CampusBLL.GetCampusDic();
                result = new ResponseResult(0, "ok", list);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"api/BaseInfo/GetCampusDic 出错 \r\n {e}", Util.Log.LogType.Error);
                result = new ResponseResult(-1, "服务内部错误", null);
            }
            return Json(result);
        }

        [HttpGet]
        [Route("api/BaseInfo/GetVenues")]
        public IHttpActionResult GetVenues()
        {
            ResponseResult result = null;
            try
            {
                IList<Venue> list = DBData.GetInstance(DBTable.venue).GetList<Venue>("state=0");
                result = new ResponseResult(0, "ok", list);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"api/BaseInfo/GetVenues 出错 \r\n {e}", Util.Log.LogType.Error);
                result = new ResponseResult(-1, "服务内部错误", null);
            }
            return Json(result);
        }

        [HttpGet]
        [Route("api/BaseInfo/GetCourseTypes")]
        public IHttpActionResult GetCourseTypes()
        {
            ResponseResult result = null;
            try
            {
                IList<CourseType> list = DBData.GetInstance(DBTable.course_type).GetList<CourseType>("state=0");
                result = new ResponseResult(0, "ok", list);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"api/BaseInfo/GetCourseTypes 出错 \r\n {e}", Util.Log.LogType.Error);
                result = new ResponseResult(-1, "服务内部错误", null);
            }
            return Json(result);
        }

        [HttpGet]
        [Route("api/BaseInfo/GetDelivers")]
        public IHttpActionResult GetDelivers()
        {
            ResponseResult result = null;
            try
            {
                IList<Deliver> list = DBData.GetInstance(DBTable.deliver).GetList<Deliver>("state=0");
                result = new ResponseResult(0, "ok", list);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"api/BaseInfo/GetDelivers 出错 \r\n {e}", Util.Log.LogType.Error);
                result = new ResponseResult(-1, "服务内部错误", null);
            }
            return Json(result);
        }

        [HttpGet]
        [Route("api/BaseInfo/GetBanner")]
        public IHttpActionResult GetBanner(short type)
        {
            ResponseResult result = null;
            try
            {
                IList<Banner> list = DBData.GetInstance(DBTable.banner).GetList<Banner>($"type='{type}' and state=0");
                result = new ResponseResult(0, "ok", list);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"api/BaseInfo/GetBanner 出错 type {type} \r\n {e}", Util.Log.LogType.Error);
                result = new ResponseResult(-1, "服务内部错误", null);
            }
            return Json(result);
        }

        [HttpGet]
        [Route("api/BaseInfo/GetSiteInfo")]
        public IHttpActionResult GetSiteInfo()
        {
            ResponseResult result = null;
            try
            {
                IList<SiteInfo> list = DBData.GetInstance(DBTable.site_info).GetList<SiteInfo>();
                result = new ResponseResult(0, "ok", list);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"api/BaseInfo/GetSiteInfo 出错 \r\n {e}", Util.Log.LogType.Error);
                result = new ResponseResult(-1, "服务内部错误", null);
            }
            return Json(result);
        }

        [HttpGet]
        [Route("api/BaseInfo/GetReceiver")]
        public IHttpActionResult GetReceiver(int id)
        {
            ResponseResult result = null;
            try
            {
                ReceiverInfo list = DBData.GetInstance(DBTable.receiver_info).GetEntityByKey<ReceiverInfo>(id);
                result = new ResponseResult(0, "ok", list);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"api/BaseInfo/GetReceiver 出错 id {id} \r\n {e}", Util.Log.LogType.Error);
                result = new ResponseResult(-1, "服务内部错误", null);
            }
            return Json(result);
        }

        [HttpGet]
        [Route("api/BaseInfo/GetReceiverList")]
        public IHttpActionResult GetReceiverList(string token)
        {
            ResponseResult result = null;
            try
            {
                int student_id = GeneralBLL.GetStudentByUserName(token).id;
                IList<ReceiverInfo> list = DBData.GetInstance(DBTable.receiver_info).GetList<ReceiverInfo>($"student_id='{student_id}'");
                result = new ResponseResult(0, "ok", list);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"api/BaseInfo/GetReceiverList 出错 token {token} \r\n {e}", Util.Log.LogType.Error);
                result = new ResponseResult(-1, "服务内部错误", null);
            }
            return Json(result);
        }

        [HttpPost]
        [Route("api/BaseInfo/UpdateReceiver")]
        public IHttpActionResult UpdateReceiver(ReceiverInfo info)
        {
            ResponseResult result = null;
            try
            {
                int count = DBData.GetInstance(DBTable.receiver_info).UpdateByKey(info, info.id);
                result = new ResponseResult(0, "ok", count > 0);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"api/BaseInfo/UpdateReceiver 出错 \r\n {e}", Util.Log.LogType.Error);
                result = new ResponseResult(-1, "服务内部错误", null);
            }
            return Json(result);
        }

        [HttpPost]
        [Route("api/BaseInfo/DeleteReceiver")]
        public IHttpActionResult DeleteReceiver(int id)
        {
            ResponseResult result = null;
            try
            {
                int count = DBData.GetInstance(DBTable.receiver_info).DeleteByKey(id);
                result = new ResponseResult(0, "ok", count > 0);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"api/BaseInfo/DeleteReceiver 出错 id {id} \r\n {e}", Util.Log.LogType.Error);
                result = new ResponseResult(-1, "服务内部错误", null);
            }
            return Json(result);
        }
    }
}
