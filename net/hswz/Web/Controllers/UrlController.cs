using System;
using System.Web.Mvc;
using Hswz.DAL;


namespace Web.Controllers
{
    public class UrlController : Controller
    {
        public ActionResult List()
        {
            var datas = UrlDAL.GetList();
            ViewBag.Datas = datas;
            return View();
        }

        public ActionResult Zan(Int32 urlId)
        {
            Boolean isOk = UrlDAL.Zan(urlId);
            return Json(new
            {
                code = 0,
                msg = "ok",
                data = isOk,
            });
        }

        public ActionResult Cai(Int32 urlId)
        {
            Boolean isOk = UrlDAL.Cai(urlId);
            return Json(new
            {
                code = 0,
                msg = "ok",
                data = isOk,
            });
        }

        public ActionResult Add(String url)
        {
            String ip = GetRequestIP();
            Boolean isOk = UrlDAL.Add(url, ip);
            return Json(new
            {
                code = 0,
                msg = "ok",
                data = isOk,
            });
        }

        /// <summary>
        /// 获取客户端ip地址
        /// </summary>
        /// <returns></returns>
        private String GetRequestIP()
        {
            String userIP;
            var Request = HttpContext.Request;

            userIP = Request.UserHostAddress;

            if (String.IsNullOrEmpty(userIP))
            {
                // 如果使用代理，获取真实IP
                if (Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != "")
                {
                    userIP = Request.ServerVariables["REMOTE_ADDR"];
                }
                else
                {
                    userIP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                }
            }
            return userIP;
        }
    }
}