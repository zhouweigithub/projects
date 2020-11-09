﻿using System;
using System.Web.Mvc;
using Public.CSUtil.Log;

namespace FileShare
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            //自定义错误异常
            filters.Add(new MyExceptionAttribute());
        }

        //自定义错误异常
        private sealed class MyExceptionAttribute : HandleErrorAttribute
        {
            /// <summary>
            /// 可捕获异常数据
            /// </summary>
            /// <param name="filterContext"></param>
            public override void OnException(ExceptionContext filterContext)
            {
                base.OnException(filterContext);

                //记录错误信息
                String errFormat = "【检测到未被捕获的异常】\r\n【操作者用户名】{0}\r\n【请求的URL】{1}\r\n【来源URL】{2}\r\n【异常信息】{3}";

                String userInfo = filterContext.HttpContext.Session["LoginUserName"] != null ? filterContext.HttpContext.Session["LoginUserName"].ToString() : String.Empty;

                LogUtil.Write(String.Format(errFormat, userInfo, filterContext.HttpContext.Request.Url, filterContext.HttpContext.Request.UrlReferrer, filterContext.Exception.ToString()), LogType.Error);
            }
        }
    }
}
