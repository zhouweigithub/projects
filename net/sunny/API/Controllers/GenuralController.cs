using Sunny.BLL.API;
using Sunny.Common;
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
using Util.Log;

namespace Sunny.API.Controllers
{
    public class GenuralController : ApiController
    {
        /// <summary>
        /// 首页数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/genural/homepage")]
        public IHttpActionResult GetHomePageDatas()
        {
            HomePageJson result = CourseBLL.GetHomePageDatas();
            return Json(result);
        }

        /// <summary>
        /// 商城页数据
        /// </summary>
        /// <returns></returns>
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
        public IHttpActionResult WithdrawCoach(string token, decimal cash)
        {
            int coach_id = DBData.GetInstance(DBTable.coach).GetEntity<Coach>($"username='{token}'").id;
            bool result = WithdrawBLL.Withdraw(coach_id, cash, UserType.Coach);
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
        public IHttpActionResult WithdrawStudent(string token, decimal cash)
        {
            int student_id = DBData.GetInstance(DBTable.student).GetEntity<Student>($"username='{token}'").id;
            bool result = WithdrawBLL.Withdraw(student_id, cash, UserType.Student);
            return Json(result);
        }

        /// <summary>
        /// 获取openid
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/genural/getopenid")]
        public IHttpActionResult GetOpenId(string code)
        {
            ResponseResult result = null;
            try
            {
                string url = $"https://api.weixin.qq.com/sns/jscode2session?appid={WebConfigData.MiniAppid}&secret={WebConfigData.MiniSecret}&js_code={code}&grant_type=authorization_code";
                string postBack = Util.Web.WebUtil.GetWebData(url, string.Empty, Util.Web.DataCompress.NotCompress);

                if (!string.IsNullOrWhiteSpace(postBack))
                {
                    CustWXLoginResult loginResult = Util.Json.JsonUtil.Deserialize<CustWXLoginResult>(postBack);
                    result = new ResponseResult(0, "ok", loginResult);
                }
                else
                {
                    result = new ResponseResult(-1, "获取失败");
                }
            }
            catch (Exception e)
            {
                result = new ResponseResult(-1, "服务器异常");
                LogUtil.Write($"获取微信openid出错:code:{code} \r\n {e}", LogType.Error);
            }

            return Json(result);
        }

    }
}
