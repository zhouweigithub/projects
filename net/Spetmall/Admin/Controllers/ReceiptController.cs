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
    public class ReceiptController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SelectUser(string keyWord, string callback)
        {
            List<member_show> members = memberDAL.GetInstance().GetMembers(keyWord, string.Empty);
            var tmp = members.Cast<member>();
            ViewBag.callback = callback;
            ViewBag.datas = tmp;
            return View();
        }

        public ActionResult Confirm(int memberid, string products)
        {
            Dictionary<int, int> productsList = Util.Json.JsonUtil.Deserialize<Dictionary<int, int>>(products);
            member member = null;
            if (memberid > 0)
                member = memberDAL.GetInstance().GetEntityByKey<member>(memberid);

            ViewBag.member = member;
            ViewBag.products = products;

            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberid"></param>
        /// <param name="products"></param>
        /// <param name="isDiscount">是否启用会员折扣</param>
        /// <returns></returns>
        public ActionResult ConfirmLeft(int memberid, string products, short isDiscount = 1)
        {
            List<receipt_confirm_products> datas = ReceiptBLL.GetDatas(memberid, products, isDiscount);
            ViewBag.products = datas;
            return View();
        }


        public ActionResult QuDan()
        {
            return View();
        }

        public ActionResult GuaDan()
        {
            return View();
        }

        public ActionResult QueryProduct(string keyWord, int page, int pageSize)
        {
            List<product> datas = productDAL.GetInstance().GetProducts(string.Empty, string.Empty, keyWord, string.Empty, page, pageSize);
            ViewBag.datas = datas;
            return Json(new
            {
                current_page = 1,
                last_page = 1,
                per_page = 100,
                total = 1,
                data = datas,
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreateOrder(orderPost postData)
        {
            (bool isOk, string msg) = ReceiptBLL.CreateOrder(postData, 0);
            return Content(GetReturnJson(isOk, msg));
        }

        private static string GetReturnJson(bool isOk, string msg)
        {
            var obj = new
            {
                status = isOk,
                msg,
            };

            string result = "<html><body><script>window.top.receiptcommon.Receiptresult("
                + Util.Json.JsonUtil.Serialize(obj)
                + ");</script></body></html>";

            return result;
        }
    }
}
