using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Spetmall.BLL.Page;
using Spetmall.DAL;
using Spetmall.Model;
using Spetmall.Model.Page;

namespace Spetmall.Admin.Controllers
{
    [Common.CustomAuthorize]
    public class TransactionController : Controller
    {

        public ActionResult Index(string keyword, string starttime, string endtime, string time = "1")
        {
            IList<order_detail> datas = orderDAL.GetOrderList(keyword, time, starttime, endtime, 0);
            ViewBag.time = time;
            ViewBag.datas = datas;
            ViewBag.keyword = keyword;
            ViewBag.starttime = starttime;
            ViewBag.endtime = endtime;
            return View();
        }

        public ActionResult Detail(string id)
        {
            ReceiptOrderInfo data = ReceiptBLL.GetOrderInfo(id, 0);
            return View(data);
        }
    }
}
