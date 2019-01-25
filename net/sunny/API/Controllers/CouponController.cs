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
    public class CouponController : ApiController
    {

        [HttpGet]
        [Route("api/coupon/get")]
        public IHttpActionResult Get(string token)
        {
            ResponseResult result = null;
            try
            {
                int student_id = DBData.GetInstance(DBTable.student).GetEntity<Student>($"username='{token}'").id;
                List<CouponListJson> list = CouponDAL.GetCouponList(student_id);
                result = new ResponseResult(0, "ok", list);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"api/coupon/Get 出错 token {token} \r\n {e}", Util.Log.LogType.Error);
                result = new ResponseResult(-1, "服务内部错误", null);
            }
            return Json(result);
        }

        [HttpGet]
        [Route("api/coupon/GetDefault")]
        public IHttpActionResult Get(string token, int categoryId)
        {
            ResponseResult result = null;
            try
            {
                int student_id = DBData.GetInstance(DBTable.student).GetEntity<Student>($"username='{token}'").id;
                CouponListJson obj = CouponDAL.GetCouponDefaultOfStudent(student_id, categoryId);
                result = new ResponseResult(0, "ok", obj);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"api/coupon/Get 出错 token {token} categoryId {categoryId} \r\n {e}", Util.Log.LogType.Error);
                result = new ResponseResult(-1, "服务内部错误", null);
            }
            return Json(result);
        }

    }
}
