//***********************************************************************************
//文件名称：HomeController.cs
//功能描述：用户登录相关页面
//数据表： 
//作者：周围
//日期：2017-09-18
//修改记录：
//***********************************************************************************

using Sunny.Admin.Common;
using Sunny.Common;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI;

namespace Sunny.Admin.Controllers
{

    [Common.CustomAuthorize]
    public class HomeController : Controller
    {

        /// <summary>
        /// 登录页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {   //退出登录
            Authentication.SignOut();
            Session.Clear();
            return View(new Models.UserViewModel() { ReturnUrl = returnUrl });
        }

        [AllowAnonymous]
        public ActionResult LoginOut()
        {   //退出登录
            Authentication.SignOut();
            Session.Clear();

            return View("Login", new Models.UserViewModel());
        }

        /// <summary>
        /// 验证码
        /// </summary>
        [AllowAnonymous]
        [OutputCache(Location = OutputCacheLocation.None)]
        public void VCode()
        {
            var code = new ValidateCode();
            string retInfo;
            var codeStr = code.CreateValidateCode(5, out retInfo);
            Session["vcode"] = retInfo;
            code.CreateValidateGraphic(codeStr);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(Models.UserViewModel model, string vcode)
        {
            string message = string.Empty;
            bool result = false;

            //Application存储登录时密码错误次数的key
            string _applicationKey = Const.Application_Login_Failed_Times_ + model.UserName;

            if (Session["vcode"] == null)
            {
                message = "验证码过期";
            }
            else
            {
                if (Session["vcode"].ToString() != vcode)
                {
                    message = "验证码错误";
                }
                else
                {
                    if (string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password))
                    {
                        message = "请输入账号、密码！";
                    }
                    else
                    {
                        int loginedCount = System.Convert.ToInt16(System.Web.HttpContext.Current.Application[_applicationKey]);
                        if (loginedCount >= Const.MaxLoginFailedTimes)
                        {
                            message = "密码连续输错 5 次，你的账号已被限制登录！请联系管理员解除限制！";
                        }
                        else
                        {
                            var userinfo = DAL.LoginUserDAL.GetUser(model.UserName, model.Password);
                            //赋值手机号
                            if (userinfo == null)
                            {
                                System.Web.HttpContext.Current.Application[_applicationKey] = loginedCount + 1;
                                message = "请输入正确的账号、密码！";
                            }
                            else if (userinfo.Status == 1)
                            {
                                message = "账号已被锁定！";
                            }
                            else
                            {
                                Authentication.SignIn(userinfo);
                                result = true;
                            }

                        }
                    }
                    Session["vcode"] = string.Empty;
                }
            }
            if (result)
            {
                //登录成功后移除登录失败次数计数
                System.Web.HttpContext.Current.Application.Remove(_applicationKey);
                if (string.IsNullOrEmpty(model.ReturnUrl) || model.ReturnUrl.Trim() == "/")
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return Redirect(model.ReturnUrl);
                }
            }
            else
            {
                model.Message = message;
                return View("Login", model);
            }
        }



        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取客户端IP地址（无视代理）
        /// </summary>
        /// <returns>若失败则返回回送地址</returns>
        public static string GetHostAddress()
        {
            var userHostAddress = System.Web.HttpContext.Current.Request.UserHostAddress;

            if (string.IsNullOrEmpty(userHostAddress))
            {
                userHostAddress = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            //最后判断获取是否成功，并检查IP地址的格式（检查其格式非常重要）
            if (!string.IsNullOrEmpty(userHostAddress) && IsIP(userHostAddress))
            {
                return userHostAddress;
            }
            return "127.0.0.1";
        }

        /// <summary>
        /// 检查IP地址格式
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
    }
}
