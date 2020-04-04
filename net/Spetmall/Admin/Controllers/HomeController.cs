//***********************************************************************************
//文件名称：HomeController.cs
//功能描述：用户登录相关页面
//数据表： 
//作者：周围
//日期：2017-09-18
//修改记录：
//***********************************************************************************

using Spetmall.Admin.Common;
using Spetmall.Common;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI;

namespace Spetmall.Admin.Controllers
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
        public ActionResult Login(Models.UserViewModel model, string vcode, string SmsCode, string posttype, int needbindmobile, int needsmscode)
        {
            //是否需要短信验证码，用户名，密码，验证码正确才显示手机验证码
            ViewBag.needSmsCode = needsmscode;

            string message = string.Empty;
            bool result = false;

            //是否需要绑定手机
            var needBindMobile = needbindmobile;

            //if (Session["vcode"] == null)
            //{
            //    message = "验证码过期";
            //}
            //else
            //{
            //if (Session["vcode"].ToString() != vcode)
            //{
            //    message = "验证码错误";
            //}
            //else
            //{
            Models.UserViewModel loginUser = new Models.UserViewModel();
            //Application存储登录时密码错误次数的key
            string _applicationKey = Spetmall.Common.Const.Application_Login_Failed_Times_ + model.UserName;
            if (string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password))
            {
                message = "请输入账号、密码！";
            }
            else
            {
                int loginedCount = System.Convert.ToInt16(System.Web.HttpContext.Current.Application[_applicationKey]);
                if (loginedCount >= Spetmall.Common.Const.MaxLoginFailedTimes)
                {
                    message = "密码连续输错 5 次，你的账号已被限制登录！请联系管理员解除限制！";
                }
                else
                {
                    var userinfo = DAL.LoginUserDAL.GetInstance().GetUser(model.UserName, model.Password);
                    loginUser = ModelConvert(userinfo);
                    //赋值手机号
                    model.Mobile = (userinfo == null || string.IsNullOrWhiteSpace(userinfo.Mobile)) ? model.Mobile : userinfo.Mobile;
                    if (loginUser == null)
                    {
                        System.Web.HttpContext.Current.Application[_applicationKey] = loginedCount + 1;
                        message = "请输入正确的账号、密码！";
                    }
                    else if (loginUser.Status == 1)
                    {
                        message = "账号已被锁定！";
                    }
                    else
                    {
                        #region 是否需要短信验证码 未在白名单且IP范围外需要

                        var ip = GetHostAddress();
                        var needSmsCode = !WebConfigData.IgnoreSmsCodeIp.Contains(ip) && userinfo.SmsVerify == 1;
                        if (needSmsCode)
                        {
                            ViewBag.needSmsCode = 1;
                        }

                        #endregion

                        //没有手机号，都需要绑定手机
                        if (string.IsNullOrWhiteSpace(model.Mobile) && string.IsNullOrWhiteSpace(loginUser.Mobile))
                        {
                            message = "请绑定手机号！";
                            needBindMobile = 1;
                            ViewBag.needSmsCode = 1;//绑定手机时都需要发送验证码
                        }
                        else
                        {
                            //绑定手机或者需要验证码，需要先验证短信验证码
                            if (posttype == "bindmobile")
                            {
                                if (string.IsNullOrWhiteSpace(model.Mobile))
                                {
                                    message = "请输入手机号";
                                }
                                else if (string.IsNullOrWhiteSpace(SmsCode))
                                {
                                    message = "请输入短信验证码";
                                }
                                else if (Session["LoginSmsCode"] == null || Session["LoginSmsCode"].ToString() != SmsCode)
                                {
                                    message = "短信验证码不正确";
                                }
                                else if (Session["LoginSmsMobile"] == null || Session["LoginSmsMobile"].ToString() != model.Mobile)
                                {
                                    message = "短信验证码和手机号不匹配";
                                }
                                else
                                {
                                    userinfo.Mobile = model.Mobile;
                                    //BLL.LoginUserBLL.Instance.Update(userinfo, " where `id`=" + userinfo.Id);
                                    DAL.LoginUserDAL.GetInstance().UpdateByKey<Model.LoginUser>(userinfo, new List<string> { "id", "username", "password", "createtime", "roleids", "appids", "IfSuper", "Status", "name", "type" }, userinfo.Id);

                                    Model.LoginUserModel userModel = DAL.LoginUserDAL.GetInstance().GetEntityByKey<Model.LoginUserModel>(userinfo.Id);
                                    Session["IfSuper"] = userModel.IfSuper;
                                    Authentication.SignIn(loginUser);
                                    result = true;
                                }
                            }
                            else if (needSmsCode)//需要短信验证码
                            {
                                if (string.IsNullOrWhiteSpace(SmsCode))
                                {
                                    if (needsmscode == 1)
                                    {
                                        message = "请输入短信验证码";
                                    }
                                }
                                else if (Session["LoginSmsCode"] == null || Session["LoginSmsCode"].ToString() != SmsCode)
                                {
                                    message = "短信验证码不正确";
                                }
                                else
                                {
                                    Model.LoginUserModel userModel = DAL.LoginUserDAL.GetInstance().GetEntityByKey<Model.LoginUserModel>(userinfo.Id);
                                    Session["IfSuper"] = userModel.IfSuper;
                                    Authentication.SignIn(loginUser);
                                    result = true;
                                }
                            }
                            else
                            {
                                Model.LoginUserModel userModel = DAL.LoginUserDAL.GetInstance().GetEntityByKey<Model.LoginUserModel>(userinfo.Id);
                                Session["IfSuper"] = userModel.IfSuper;
                                Authentication.SignIn(loginUser);
                                result = true;
                            }
                        }

                    }
                    //}
                    //Session["vcode"] = string.Empty;
                }
            }
            //}
            if (result)
            {
                Session["LoginSmsCode"] = string.Empty;
                Session["LoginSmsMobile"] = string.Empty;
                //登录成功后移除登录失败次数计数
                System.Web.HttpContext.Current.Application.Remove(_applicationKey);
                if (string.IsNullOrEmpty(model.ReturnUrl) || model.ReturnUrl.Trim() == "/")
                {
                    //return RedirectToAction("CarouselData", "HomeMg");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return Redirect(model.ReturnUrl);
                }
            }
            else
            {
                ViewBag.needBindMobile = needBindMobile;
                model.Message = message;
                //if (message != "还未绑定手机号！")
                //{
                //    model.Password = string.Empty;
                //}

                return View("Login", model);
            }
        }



        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 发送短信验证码
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <param name="Mobile">电话号码：绑定手机直接读取；已绑定手机从数据库读取</param>
        /// <param name="vcode"></param>
        /// <param name="posttype"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult GetSmsCode(string UserName, string Password, string Mobile, string vcode, string posttype)
        {
            var result = false;
            var errMsg = string.Empty;
            var sendmobile = Mobile;//发送短信的电话
            //if (Session["vcode"] == null)
            //{
            //    errMsg = "验证码过期";
            //}
            //else
            //{
            //    if (Session["vcode"].ToString() != vcode)
            //    {
            //        errMsg = "验证码错误";
            //    }
            //    else
            //    {
            Models.UserViewModel loginUser = new Models.UserViewModel();
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
            {
                errMsg = "请输入账号、密码！";
            }
            else
            {
                loginUser = ModelConvert(DAL.LoginUserDAL.GetInstance().GetUser(UserName, Password));

                if (loginUser == null)
                {
                    errMsg = "请输入正确的账号、密码！";
                }
                else if (loginUser.Status == 1)
                {
                    errMsg = "账号已被锁定！";
                }
                else
                {
                    //绑定账号直接读
                    if (posttype != "bindmobile")
                    {
                        sendmobile = loginUser.Mobile;
                    }

                    //此处发送短信
                    string smsCode = Function.GetRangeNumber(4, RangeType.Number);
                    SmsHelper.SmsSendResult smsResult = SmsHelper.SingleSend(sendmobile, smsCode);
                    if (smsResult == null || smsResult.result != 0)
                    {   //短信发送失败
                        errMsg = smsResult != null ? smsResult.errmsg : "未知原因！";
                        Util.Log.LogUtil.Write(string.Format("发送短信失败！{0}  {1}    {2}", sendmobile, smsCode, errMsg), Util.Log.LogType.Error);
                    }

                    result = string.IsNullOrEmpty(errMsg);
                    if (result)
                    {
                        Session["LoginSmsCode"] = smsCode;
                        Session["LoginSmsMobile"] = sendmobile;
                    }
                    else
                    {
                        Session["LoginSmsCode"] = string.Empty;
                        Session["LoginSmsMobile"] = string.Empty;
                    }
                }
            }
            //        Session["vcode"] = string.Empty;
            //    }
            //}


            return Json(new
            {
                Result = result,
                Message = result ? "发送成功：" + sendmobile : "发送失败：" + errMsg
            }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 模型转化
        /// </summary>
        private Model.LoginUser ModelConvert(Models.UserViewModel model)
        {
            if (model == null)
                return null;

            return new Model.LoginUser()
            {
                Id = model.id,
                Username = model.UserName,
                Password = model.Password,
                Roleids = model.roleids
            };
        }

        /// <summary>
        /// 模型转化
        /// </summary>
        private Models.UserViewModel ModelConvert(Model.LoginUser model)
        {
            if (model == null)
                return null;

            return new Models.UserViewModel()
            {
                id = model.Id,
                UserName = model.Username,
                Password = model.Password,
                roleids = model.Roleids,
                Status = model.Status,
                Mobile = model.Mobile
            };
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
