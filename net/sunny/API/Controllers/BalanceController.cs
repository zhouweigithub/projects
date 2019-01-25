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

        /// <summary>
        /// 余额记录
        /// </summary>
        /// <param name="token"></param>
        /// <param name="type">0学员1教练</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/balance/get")]
        public IHttpActionResult Get(string token, int type)
        {
            ResponseResult result = null;
            try
            {
                int userid = 0;
                if (type == 0)
                    userid = DBData.GetInstance(DBTable.student).GetEntity<Student>($"username='{token}'").id;
                else
                    userid = DBData.GetInstance(DBTable.coach).GetEntity<Coach>($"username='{token}'").id;

                string where = $"crtime>='{DateTime.Now.AddMonths(-3).ToString("yyyy-MM-dd")}' and user_id='{userid}' and user_type='{type}'";
                IList<PayRecord> records = DBData.GetInstance(DBTable.pay_record).GetList<PayRecord>(where);
                List<PayRecordJson> list = new List<PayRecordJson>();
                foreach (PayRecord item in records)
                {
                    list.Add(new PayRecordJson()
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
                result = new ResponseResult(0, "ok", list);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"api/balance/get 出错 token {token} type {type} \r\n {e}", Util.Log.LogType.Error);
                result = new ResponseResult(-1, "服务内部错误", null);
            }
            return Json(result);
        }
    }
}
