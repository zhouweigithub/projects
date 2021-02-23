using System;
using System.Web.Mvc;
using Hswz.DAL;

namespace Web.Controllers
{
    public class ResourceController : Controller
    {
        public ActionResult List(String name, Int32 page = 1, Int32 pageSize = 20)
        {
            var datas = ResourceDAL.GetList(name, page, pageSize);
            Int32 count = ResourceDAL.GetCount(name);
            ViewBag.Datas = datas;
            ViewBag.DataCount = count;
            return View();
        }
    }
}