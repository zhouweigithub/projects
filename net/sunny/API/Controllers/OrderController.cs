using Sunny.DAL;
using Sunny.Model;
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
        public IHttpActionResult IsBought(int studentid, int productId)
        {
            int count = DBData.GetInstance(DBTable.course).GetCount($"student_id='{studentid}' and product_id='{productId}'");
            return Json(new { result = count > 0 });
        }


    }
}
