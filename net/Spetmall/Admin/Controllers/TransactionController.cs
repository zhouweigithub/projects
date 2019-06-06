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
    public class TransactionController : Controller
    {
        //
        // GET: /Transaction/

        public ActionResult Index(string keyword, string time, string starttime, string endtime)
        {
            IList<order_detail> datas = orderDAL.GetOrderList(keyword, time, starttime, endtime, 0);
            ViewBag.datas = datas;
            return View();
        }

        public ActionResult Detail(string id)
        {
            ReceiptOrderInfo data = ReceiptBLL.GetOrderInfo(id, 0);
            return View(data);
        }
    }
}
