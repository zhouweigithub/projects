using Sunny.BLL.API;
using Sunny.DAL;
using Sunny.Model;
using Sunny.Model.Custom;
using Sunny.Model.Request;
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
                int student_id = GeneralBLL.GetStudentByUserName(token).id;
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
        public IHttpActionResult Get(string token, int product_id, decimal total_money)
        {
            ResponseResult result = null;
            try
            {
                int student_id = GeneralBLL.GetStudentByUserName(token).id;
                CouponListJson coupons = CouponBLL.GetDefaultCoupon(student_id, product_id, total_money);
                result = new ResponseResult(0, "ok", coupons);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"api/coupon/GetDefault 出错 token {token} product_id {product_id} total_money {total_money} \r\n {e}", Util.Log.LogType.Error);
                result = new ResponseResult(-1, "服务内部错误", null);
            }
            return Json(result);
        }

        [HttpGet]
        [Route("api/coupon/available")]
        public IHttpActionResult GetAvailableCoupon(string token, List<TmpProduct> products)
        {
            ResponseResult result = null;
            try
            {
                int student_id = GeneralBLL.GetStudentByUserName(token).id;
                List<CouponListJson> coupons = CouponBLL.GetAvailableCoupon(student_id, products);
                result = new ResponseResult(0, "ok", coupons);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"api/coupon/GetAvailableCoupon 出错 token {token} \r\n {e}", Util.Log.LogType.Error);
                result = new ResponseResult(-1, "服务内部错误", null);
            }
            return Json(result);
        }
    }
}
