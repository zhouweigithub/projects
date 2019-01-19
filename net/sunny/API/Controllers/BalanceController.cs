using Sunny.DAL;
using Sunny.Model;
using Sunny.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Sunny.API.Controllers
{
    public class BalanceController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get(int userid)
        {
            string where = $"crtime>='{DateTime.Now.AddMonths(-3).ToString("yyyy-MM-dd")}'";
            IList<PayRecord> records = DBData.GetInstance(DBTable.pay_record).GetList<PayRecord>(where);
            List<PayRecordJson> result = new List<PayRecordJson>();
            foreach (PayRecord item in records)
            {
                result.Add(new PayRecordJson()
                {
                    id = item.Id,
                    type = item.type,
                    balance = item.balance,
                    comment = item.comment,
                    money = item.money,
                    order_id = item.order_id,
                    crtime = item.crtime,
                });
            }
            return Json(result);
        }
    }
}
