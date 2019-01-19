using Sunny.BLL.API;
using Sunny.Common;
using Sunny.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Sunny.API.Controllers
{
    public class GenuralController : ApiController
    {
        [HttpGet]
        [Route("api/genural/homepage")]
        public IHttpActionResult GetHomePageDatas()
        {
            HomePageJson result = CourseBLL.GetHomePageDatas();
            return Json(result);
        }

        [HttpGet]
        [Route("api/genural/mallpage")]
        public IHttpActionResult GetMallPageDatas()
        {
            MallPageJson result = CourseBLL.GetMallPageDatas();
            return Json(result);
        }

        /// <summary>
        /// 教练提现
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="cash"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/genural/withdrawcoach")]
        public IHttpActionResult WithdrawCoach(int userid, decimal cash)
        {
            bool result = WithdrawBLL.Withdraw(userid, cash, UserType.Coach);
            return Json(result);
        }

        /// <summary>
        /// 学员提现
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="cash"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/genural/withdrawstudent")]
        public IHttpActionResult WithdrawStudent(int userid, decimal cash)
        {
            bool result = WithdrawBLL.Withdraw(userid, cash, UserType.Student);
            return Json(result);
        }
    }
}
