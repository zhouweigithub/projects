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
        [Route("api/BaseInfo/GetCampus")]
        [HttpGet]
        public IHttpActionResult GetCampus()
        {
            IList<Campus> result = DBData.GetInstance(DBTable.campus).GetList<Campus>("state=0");
            return Json(result);
        }

        [Route("api/BaseInfo/GetCampusDic")]
        [HttpGet]
        public IHttpActionResult GetCampusDic()
        {
            Dictionary<int, List<Venue>> result = CampusBLL.GetCampusDic();
            return Json(result);
        }

        [Route("api/BaseInfo/GetVenues")]
        [HttpGet]
        public IHttpActionResult GetVenues()
        {
            IList<Venue> result = DBData.GetInstance(DBTable.venue).GetList<Venue>("state=0");
            return Json(result);
        }

        [Route("api/BaseInfo/GetCourseTypes")]
        [HttpGet]
        public IHttpActionResult GetCourseTypes()
        {
            IList<CourseType> result = DBData.GetInstance(DBTable.course_type).GetList<CourseType>("state=0");
            return Json(result);
        }

        [Route("api/BaseInfo/GetDelivers")]
        [HttpGet]
        public IHttpActionResult GetDelivers()
        {
            IList<Deliver> result = DBData.GetInstance(DBTable.deliver).GetList<Deliver>("state=0");
            return Json(result);
        }


    }
}
