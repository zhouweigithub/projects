using Moqikaka.Tmp.BLL.Page.ServerLog;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Moqikaka.Tmp.Admin.Controllers
{
    [Common.CustomAuthorize]
    public class ServerLogController : Controller
    {
        /// <summary>
        /// 系统日志
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult LogList(int type = 1)
        {
            List<ServerLogBLL.FileData> fileList = ServerLogBLL.GetFileList((ServerLogBLL.LogType)type);
            ViewBag.Title = Enum.GetName(typeof(ServerLogBLL.LogType), (ServerLogBLL.LogType)type) + " - Logs";
            ViewBag.Type = type;
            ViewBag.Datas = fileList;
            return View();
        }

        public ActionResult LogView(int type = 1, string name = "")
        {
            ViewBag.Title = name;
            byte[] bytes = ServerLogBLL.GetFileBytes((ServerLogBLL.LogType)type, name);
            return File(bytes, ServerLogBLL.fileContentType);
        }

    }
}
