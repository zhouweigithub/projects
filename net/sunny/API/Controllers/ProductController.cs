using Sunny.BLL.API;
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
    public class ProductController : ApiController
    {

        // GET: api/Product/5
        public IHttpActionResult Get(int id)
        {
            CourseInfoJson result = CourseBLL.GetCourseInfo(id);
            return Json(result);
        }

        public IHttpActionResult Get(string name, int categoryId, int page, int pageSize)
        {
            List<CourseListJson> result = CourseDAL.GetCourseList(name, categoryId, page, pageSize);
            return Json(result);
        }


    }
}
