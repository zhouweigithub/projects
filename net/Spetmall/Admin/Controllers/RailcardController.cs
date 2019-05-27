using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Spetmall.DAL;
using Spetmall.Model;

namespace Moqikaka.Tmp.Admin.Controllers
{
    public class RailcardController : Controller
    {

        public ActionResult Index(string keyWord, string orderBy)
        {
            List<railcard> datas = railcardDAL.GetInstance().GetRailcards(keyWord, orderBy);
            ViewBag.keyWord = keyWord;
            ViewBag.datas = datas;
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult Record()
        {
            return View();
        }

        public ActionResult Delete()
        {
            return View();
        }
    }
}
