using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Hswz.Common;
using Util.Log;

namespace Hswz.Admin
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            //记录js版本
            Application["ver"] = WebConfigData.Ver;
            //日志路径
            LogUtil.SetLogPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log"));
            Const.RootWebPath = Server.MapPath("/");
        }

        protected void Application_End(Object sender, EventArgs e)
        {
        }

    }
}