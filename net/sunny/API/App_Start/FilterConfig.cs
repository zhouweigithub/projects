using System.Web;
using System.Web.Mvc;

namespace API
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //自定义错误异常
            filters.Add(new MyExceptionAttribute());
        }

        //自定义错误异常
        public class MyExceptionAttribute : HandleErrorAttribute
        {
            /// <summary>
            /// 可捕获异常数据
            /// </summary>
            /// <param name="filterContext"></param>
            public override void OnException(ExceptionContext filterContext)
            {
                base.OnException(filterContext);

                //记录错误信息
                string errFormat = "【检测到未被捕获的异常】\r\n 【请求的URL】{0}  \r\n 【来源URL】{1}  \r\n 【异常信息】{2}";
                Util.Log.LogUtil.Write(string.Format(errFormat, filterContext.HttpContext.Request.Url, filterContext.HttpContext.Request.UrlReferrer, filterContext.Exception.ToString()), Util.Log.LogType.Error);
            }
        }
    }
}
