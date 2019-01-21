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
            IList<Campus> result = DBData.GetInstance(DBTable.campus).GetList<Campus>("state=0");
            return Json(result);
        }

        [HttpGet]
        [Route("api/BaseInfo/GetCampusDic")]
        public IHttpActionResult GetCampusDic()
        {
            Dictionary<int, List<Venue>> result = CampusBLL.GetCampusDic();
            return Json(result);
        }

        [HttpGet]
        [Route("api/BaseInfo/GetVenues")]
        public IHttpActionResult GetVenues()
        {
            IList<Venue> result = DBData.GetInstance(DBTable.venue).GetList<Venue>("state=0");
            return Json(result);
        }

        [HttpGet]
        [Route("api/BaseInfo/GetCourseTypes")]
        public IHttpActionResult GetCourseTypes()
        {
            IList<CourseType> result = DBData.GetInstance(DBTable.course_type).GetList<CourseType>("state=0");
            return Json(result);
        }

        [HttpGet]
        [Route("api/BaseInfo/GetDelivers")]
        public IHttpActionResult GetDelivers()
        {
            IList<Deliver> result = DBData.GetInstance(DBTable.deliver).GetList<Deliver>("state=0");
            return Json(result);
        }

        [HttpGet]
        [Route("api/BaseInfo/GetBanner")]
        public IHttpActionResult GetBanner(short type)
        {
            IList<Banner> result = DBData.GetInstance(DBTable.banner).GetList<Banner>($"type='{type}' and state=0");
            return Json(result);
        }

        [HttpGet]
        [Route("api/BaseInfo/GetSiteInfo")]
        public IHttpActionResult GetSiteInfo()
        {
            IList<SiteInfo> result = DBData.GetInstance(DBTable.site_info).GetList<SiteInfo>();
            return Json(result);
        }

        [HttpGet]
        [Route("api/BaseInfo/GetReceiver")]
        public IHttpActionResult GetReceiver(int id)
        {
            ReceiverInfo result = DBData.GetInstance(DBTable.receiver_info).GetEntityByKey<ReceiverInfo>(id);
            return Json(result);
        }

        [HttpGet]
        [Route("api/BaseInfo/GetReceiverList")]
        public IHttpActionResult GetReceiverList(string token)
        {
            int student_id = DBData.GetInstance(DBTable.student).GetEntity<Student>($"username='{token}'").id;
            IList<ReceiverInfo> result = DBData.GetInstance(DBTable.receiver_info).GetList<ReceiverInfo>($"student_id='{student_id}'");
            return Json(result);
        }

        [HttpPost]
        [Route("api/BaseInfo/UpdateReceiver")]
        public IHttpActionResult UpdateReceiver(ReceiverInfo info)
        {
            int count = DBData.GetInstance(DBTable.receiver_info).UpdateByKey(info, info.id);
            return Json(count > 0);
        }

        [HttpPost]
        [Route("api/BaseInfo/DeleteReceiver")]
        public IHttpActionResult DeleteReceiver(int id)
        {
            int count = DBData.GetInstance(DBTable.receiver_info).DeleteByKey(id);
            return Json(count > 0);
        }
    }
}
