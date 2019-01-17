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
        public IHttpActionResult IsBought(int studentid, int productId)
        {
            int count = DBData.GetInstance(DBTable.course).GetCount($"student_id='{studentid}' and product_id='{productId}'");
            return Json(new { result = count > 0 });
        }

        [HttpPost]
        [Route("api/order/create")]
        public IHttpActionResult Create(OrderRequest request)
        {
            int count = OrderBLL.CreateOrder(request);
            return Json(new { result = count > 0 });
        }

        [HttpGet]
        [Route("api/order/getlist")]
        public IHttpActionResult GetOrderList(int studentId)
        {
            List<OrderJson> result = OrderBLL.GetOrderInfo(studentId);
            return Json(result);
        }


        [HttpPost]
        [Route("api/order/paysuccess")]
        public IHttpActionResult PaySucess(string orderId, decimal money)
        {
            bool result = OrderDAL.OrderPaySuccess(orderId, money);
            return Json(result);
        }


        [HttpPost]
        [Route("api/order/updatestate")]
        public IHttpActionResult UpdateOrderState(string orderId, short state)
        {
            Dictionary<string, object> fieldValue = new Dictionary<string, object>();
            fieldValue.Add("state", state);
            int count = DBData.GetInstance(DBTable.order).UpdateByKey(fieldValue, orderId);
            return Json(count > 0);
        }


    }
}
