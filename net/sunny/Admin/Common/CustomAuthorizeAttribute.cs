//***********************************************************************************
//文件名称：CustomAuthorizeAttribute.cs
//功能描述： 登录权限验证
//数据表： 
//作者：周围
//日期：2017-09-18
//修改记录：
//***********************************************************************************
using Sunny.Admin.Models;
using Sunny.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sunny.Admin.Common
{
    [AttributeUsage(AttributeTargets.All, Inherited = true)]
    /// <summary>
    /// 登录权限验证
    /// </summary>
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private string ReturnUrl = "";

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            ReturnUrl = httpContext.Request.Url.PathAndQuery;
            string action = httpContext.Request.RequestContext.RouteData.Values["action"].ToString();//当前访问的action
            string controller = httpContext.Request.RequestContext.RouteData.Values["controller"].ToString();//当前访问的controler

            if ((new string[] { "login", "loginout" }).Contains(action.ToLower()) && controller.ToLower() == "Home")
            {
                return true;
            }

            if (HttpContext.Current.Session["LoginUserName"] == null)
            {   //没登录则跳出
                httpContext.Response.StatusCode = 401;
                return false;
            }

            //判断菜单是否是用户拥有的菜单，否则则跳转到登录页面
            //#region 判断菜单是否是用户拥有的菜单，否则则跳转到登录页面


            //try
            //{
            //    if (HttpContext.Current.Session["MenuGrouplist"] != null)
            //    {
            //        List<MenuGroup> MenuGrouplist = (List<MenuGroup>)HttpContext.Current.Session["MenuGrouplist"];
            //        bool flag = false;
            //        string[] extraUsernameArray = WebConfigData.ExtraUserNames.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            //        foreach (var item in MenuGrouplist)
            //        {
            //            foreach (var kk in item.MenuItemList)
            //            {   //有main权限的也给予links的权限
            //                if (kk.Controller == controller || (kk.Controller.ToUpper() == "MAIN" && controller.ToUpper() == "LINKS"))
            //                {
            //                    flag = true;
            //                    break;
            //                }
            //                else if (extraUsernameArray.Contains(HttpContext.Current.Session["LoginUserName"]))
            //                {
            //                    flag = true;
            //                    break;
            //                }
            //            }
            //            if (flag)
            //            {
            //                break;
            //            }
            //        }
            //        if (!flag)
            //        {
            //            httpContext.Response.StatusCode = 401;
            //            return false;
            //        }
            //    }
            //}
            //catch (System.Exception e)
            //{
            //    Util.Log.LogUtil.Write("菜单权限判定时出错啦！" + e.ToString(), Util.Log.LogType.Error);
            //}
            //#endregion

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
                #region 热云跳转                

                if (!string.IsNullOrWhiteSpace(ReturnUrl))
                {

                    if (ReturnUrl.Contains("ActiveGroupInfo"))
                    {
                        ReturnUrl = ReturnUrl.Substring(0, ReturnUrl.LastIndexOf('/') + 1) + "Index";
                    }
                }
                #endregion

                filterContext.Result = new RedirectResult("/" + (string.IsNullOrWhiteSpace(ReturnUrl) ? "" : "?returnUrl=" + HttpContext.Current.Server.UrlEncode(ReturnUrl)));
            }
        }
    }
}