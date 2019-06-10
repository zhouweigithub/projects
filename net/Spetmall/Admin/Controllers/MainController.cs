using Spetmall.BLL.Page;
using Spetmall.DAL;
using Spetmall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Spetmall.Admin.Controllers
{
    [Common.CustomAuthorize]
    public class MainController : Controller
    {

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return Json(new
            {
                code = 1,
                status = true,
                msg = "退出成功",
                redirect = string.Empty,
                redirects = string.Empty,
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            bool isok = false;
            string msg = string.Empty;
            try
            {
                user user = userDAL.GetInstance().GetEntity<user>($"username='{username}' and password='{password}'");
                if (user != null)
                {
                    Session["username"] = user.username;
                    Session["name"] = user.name;
                    isok = true;
                }
                else
                {
                    isok = false;
                    msg = "用户名或密码错误";
                }
            }
            catch (Exception e)
            {
                msg = "服务内部错误";
                Util.Log.LogUtil.Write("登录出错：" + e, Util.Log.LogType.Error);
            }

            return Content(CommonBLL.GetLoginReturnJson(isok, msg));
        }

        public ActionResult Lock()
        {
            Session["isLock"] = true;
            return Json(new
            {
                status = true,
                msg = string.Empty,
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UnLock(string password)
        {

            bool isok = false;
            string msg = string.Empty;
            try
            {
                if (Session["username"] == null)
                {
                    msg = "登录已过期，请重新登录";
                }
                else
                {
                    string username = Session["username"].ToString();
                    int count = userDAL.GetInstance().GetCount($"username='{username}' and password='{password}'");
                    if (count > 0)
                    {
                        Session["isLock"] = false;
                        isok = true;
                    }
                    else
                    {
                        msg = "用户名或密码错误";
                    }
                }
            }
            catch (Exception e)
            {
                msg = "服务内部错误";
                Util.Log.LogUtil.Write("登录出错：" + e, Util.Log.LogType.Error);
            }

            return Json(new
            {
                status = isok,
                msg,
            }, JsonRequestBehavior.AllowGet);
        }

    }
}
