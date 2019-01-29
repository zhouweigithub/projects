using Sunny.BLL.API;
using Sunny.Common;
using Sunny.DAL;
using Sunny.Model;
using Sunny.Model.Custom;
using Sunny.Model.Request;
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
            ResponseResult result = null;
            try
            {
                HomePageJson obj = CourseBLL.GetHomePageDatas();
                result = new ResponseResult(0, "ok", obj);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"api/genural/homepage 出错 \r\n {e}", Util.Log.LogType.Error);
                result = new ResponseResult(-1, "服务内部错误", null);
            }
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
            ResponseResult result = null;
            try
            {
                MallPageJson list = CourseBLL.GetMallPageDatas();
                result = new ResponseResult(0, "ok", list);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"api/genural/mallpage 出错 \r\n {e}", Util.Log.LogType.Error);
                result = new ResponseResult(-1, "服务内部错误", null);
            }
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
        public IHttpActionResult WithdrawCoach(WithdrawRequest request)
        {
            ResponseResult result = null;
            try
            {
                int coach_id = DBData.GetInstance(DBTable.coach).GetEntity<Coach>($"username='{request.token}'").id;
                bool list = WithdrawBLL.Withdraw(coach_id, request.cash, UserType.Coach);
                result = new ResponseResult(0, "ok", list);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"api/genural/withdrawcoach 出错 token {request.token} cash {request.cash} \r\n {e}", Util.Log.LogType.Error);
                result = new ResponseResult(-1, "服务内部错误", null);
            }
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
        public IHttpActionResult WithdrawStudent(WithdrawRequest request)
        {
            ResponseResult result = null;
            try
            {
                int student_id = DBData.GetInstance(DBTable.student).GetEntity<Student>($"username='{request.token}'").id;
                bool list = WithdrawBLL.Withdraw(student_id, request.cash, UserType.Student);
                result = new ResponseResult(0, "ok", list);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"api/genural/withdrawstudent 出错 token {request.token} cash {request.cash} \r\n {e}", Util.Log.LogType.Error);
                result = new ResponseResult(-1, "服务内部错误", null);
            }
            return Json(result);
        }

        /// <summary>
        /// 获取openid
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/genural/getopenid")]
        public IHttpActionResult GetOpenId(string code)
        {
            ResponseResult result = null;
            try
            {
                string url = $"https://api.weixin.qq.com/sns/jscode2session?appid={WebConfigData.MiniAppid}&secret={WebConfigData.MiniSecret}&js_code={code}&grant_type=authorization_code";
                string postBack = Util.Web.WebUtil.GetWebData(url, string.Empty, Util.Web.DataCompress.NotCompress);

                LogUtil.Write($"获取微信openid-url:{url} ", LogType.Debug);
                LogUtil.Write($"获取微信openid-postBack:{postBack} ", LogType.Debug);

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
