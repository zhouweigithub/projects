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
    public class OrderController : ApiController
    {

        [HttpGet]
        [Route("api/order/isbought")]
        public IHttpActionResult IsBought(string token, int productId)
        {
            ResponseResult result = null;
            try
            {
                int student_id = DBData.GetInstance(DBTable.student).GetEntity<Student>($"username='{token}'").id;
                int count = DBData.GetInstance(DBTable.course).GetCount($"student_id='{student_id}' and product_id='{productId}'");
                result = new ResponseResult(0, "ok", count > 0);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"api/order/isbought 出错 token {token} productId {productId} \r\n {e}", Util.Log.LogType.Error);
                result = new ResponseResult(-1, "服务内部错误", null);
            }
            return Json(result);
        }

        [HttpPost]
        [Route("api/order/create")]
        public IHttpActionResult Create(OrderRequest request)
        {
            ResponseResult result = null;
            try
            {
                bool isOk = OrderBLL.CreateOrder(request, out string msg);
                string message = isOk ? "ok" : msg;
                int code = isOk ? 0 : -1;
                result = new ResponseResult(code, message, isOk);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"api/order/create 出错 \r\n {e}", Util.Log.LogType.Error);
                result = new ResponseResult(-1, "服务内部错误", null);
            }
            return Json(result);
        }

        [HttpGet]
        [Route("api/order/getlist")]
        public IHttpActionResult GetOrderList(string token, int state)
        {
            ResponseResult result = null;
            try
            {
                int student_id = DBData.GetInstance(DBTable.student).GetEntity<Student>($"username='{token}'").id;
                List<OrderJson> list = OrderBLL.GetOrderInfo(student_id, state);
                result = new ResponseResult(0, "ok", list);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"api/order/getlist 出错 token {token} \r\n {e}", Util.Log.LogType.Error);
                result = new ResponseResult(-1, "服务内部错误", null);
            }
            return Json(result);
        }


        [HttpPost]
        [Route("api/order/paysuccess")]
        public IHttpActionResult PaySucess(PaySucessRequest request)
        {
            ResponseResult result = null;
            try
            {
                bool list = OrderDAL.OrderPaySuccess(request.orderId, request.money);
                result = new ResponseResult(0, "ok", list);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"api/order/paysuccess 出错 orderId {request.orderId} money {request.money} \r\n {e}", Util.Log.LogType.Error);
                result = new ResponseResult(-1, "服务内部错误", null);
            }
            return Json(result);
        }


        [HttpPost]
        [Route("api/order/updatestate")]
        public IHttpActionResult UpdateOrderState(UpdateOrderStateRequest request)
        {
            ResponseResult result = null;
            try
            {
                Dictionary<string, object> fieldValue = new Dictionary<string, object>();
                fieldValue.Add("state", request.state);
                int count = DBData.GetInstance(DBTable.order).UpdateByKey(fieldValue, request.orderId);
                result = new ResponseResult(0, "ok", count > 0);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"api/order/updatestate 出错 orderId {request.orderId} state {request.state} \r\n {e}", Util.Log.LogType.Error);
                result = new ResponseResult(-1, "服务内部错误", null);
            }
            return Json(result);
        }


    }
}
