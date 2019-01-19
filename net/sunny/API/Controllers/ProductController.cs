using Sunny.BLL.API;
using Sunny.DAL;
using Sunny.Model.Custom;
using Sunny.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    public class ProductController : ApiController
    {

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            CourseInfoJson result = CourseBLL.GetCourseInfo(id);
            return Json(result);
        }

        [HttpGet]
        [Route("api/product/getlist")]
        public IHttpActionResult Get(string name, int categoryId, int page, int pageSize)
        {
            List<CourseListJson> result = CourseDAL.GetCourseList(name, categoryId, page, pageSize);
            return Json(result);
        }

        [HttpGet]
        [Route("api/product/random")]
        public IHttpActionResult GetRandom()
        {
            List<CourseListJson> result = CourseDAL.GetRandomCourseList();
            return Json(result);
        }

    }
}
