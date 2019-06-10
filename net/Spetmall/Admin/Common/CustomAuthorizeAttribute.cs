//***********************************************************************************
//文件名称：CustomAuthorizeAttribute.cs
//功能描述： 登录权限验证
//数据表： 
//作者：周围
//日期：2017-09-18
//修改记录：
//***********************************************************************************
using Spetmall.Admin.Models;
using Spetmall.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Spetmall.Admin.Common
{
    /// <summary>
    /// 登录权限验证
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = true)]
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private string ReturnUrl = "";

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            ReturnUrl = httpContext.Request.Url.PathAndQuery;
            string action = httpContext.Request.RequestContext.RouteData.Values["action"].ToString().ToLower();//当前访问的action
            string controller = httpContext.Request.RequestContext.RouteData.Values["controller"].ToString().ToLower();//当前访问的controler

            if ((new string[] { "login", "loginout" }).Contains(action) && controller == "main")
            {
                return true;
            }

            if (HttpContext.Current.Session["username"] == null ||
                (HttpContext.Current.Session["isLock"] != null && Convert.ToBoolean(HttpContext.Current.Session["isLock"]) == true
                && controller != "main" && action != "unlock"))
            {   //没登录则跳出
                httpContext.Response.StatusCode = 401;
                HttpContext.Current.Session.Abandon();
                return false;
            }

            return true;
        }


        /// <summary>
        /// 从票据里面获取用户
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Response.StatusCode == 401)
            {
                filterContext.Result = new RedirectResult("/" + (string.IsNullOrWhiteSpace(ReturnUrl) ? "" : "?returnUrl=" + HttpContext.Current.Server.UrlEncode(ReturnUrl)));
            }
        }
    }
}