using Sunny.BLL.API;
using Sunny.DAL;
using Sunny.Model;
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
        [Route("api/product/get")]
        public IHttpActionResult Get(int id)
        {
            ResponseResult result = null;
            try
            {
                object obj = null;
                Category category = CategoryDAL.GetCategoryByProductId(id);

                if (category.type == 0)
                    obj = CourseBLL.GetCourseInfo(id);
                else
                    obj = ProductBLL.GetProductInfo(id);

                result = new ResponseResult(0, "ok", obj);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"api/product/Get 出错 id {id} \r\n {e}", Util.Log.LogType.Error);
                result = new ResponseResult(-1, "服务内部错误", null);
            }
            return Json(result);
        }

        [HttpGet]
        [Route("api/product/getlist")]
        public IHttpActionResult Get(string name, int categoryId, int page, int pageSize)
        {
            ResponseResult result = null;
            try
            {
                List<ProductListJson> courseList = CourseDAL.GetCourseList(name, categoryId, page, pageSize);
                List<ProductListJson> productList = ProductDAL.GetProductList(name, categoryId, page, pageSize);
                var list = courseList.Concat(productList);
                result = new ResponseResult(0, "ok", list);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"api/product/getlist 出错 name {name} categoryId {categoryId} page {page} pageSize {pageSize} \r\n {e}", Util.Log.LogType.Error);
                result = new ResponseResult(-1, "服务内部错误", null);
            }
            return Json(result);
        }

        [HttpGet]
        [Route("api/product/random")]
        public IHttpActionResult GetRandom()
        {
            ResponseResult result = null;
            try
            {
                List<ProductListJson> list = CourseDAL.GetRandomCourseList(3);
                result = new ResponseResult(0, "ok", list);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"api/product/random 出错 \r\n {e}", Util.Log.LogType.Error);
                result = new ResponseResult(-1, "服务内部错误", null);
            }
            return Json(result);
        }

        //[HttpGet]
        //[Route("api/product/coursespecification")]
        //public IHttpActionResult GetCourseSpecification(int id)
        //{
        //    ResponseResult result = null;
        //    try
        //    {
        //        CourseSpecificationJson category = ProductExtroBLL.GetCourseSpecification(id);
        //        result = new ResponseResult(0, "ok", category);
        //    }
        //    catch (Exception e)
        //    {
        //        Util.Log.LogUtil.Write($"api/product/GetCourseSpecification 出错 id {id} \r\n {e}", Util.Log.LogType.Error);
        //        result = new ResponseResult(-1, "服务内部错误", null);
        //    }
        //    return Json(result);
        //}


    }
}
