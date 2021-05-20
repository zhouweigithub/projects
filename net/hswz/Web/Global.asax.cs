using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Hswz.Common;
using Util.Log;

namespace Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //记录js版本
            Application["ver"] = WebConfigData.Ver;
            //日志路径
            LogUtil.SetLogPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log"));
            Const.RootWebPath = Server.MapPath("/");
        }
    }
}
