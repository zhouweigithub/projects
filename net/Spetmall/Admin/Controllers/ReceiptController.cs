using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Spetmall.DAL;
using Spetmall.Model;
using Spetmall.Model.Page;

namespace Moqikaka.Tmp.Admin.Controllers
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
            Dictionary<int, int> productsList = Util.Json.JsonUtil.Deserialize<Dictionary<int, int>>(products);

            member member = null;
            if (memberid > 0)
                member = memberDAL.GetInstance().GetEntityByKey<member>(memberid);

            string productIds = string.Empty;
            foreach (int item in productsList.Keys)
            {
                productIds += item + ",";
            }
            productIds = productIds.TrimEnd(',');
            IList<product> productInfoList = productDAL.GetInstance().GetList<product>($"id in({productIds})");

            List<receipt_confirm_products> datas = new List<receipt_confirm_products>();
            foreach (product item in productInfoList)
            {
                receipt_confirm_products tmp = new receipt_confirm_products()
                {
                    productId = item.id,
                    productName = item.name,
                    thumbnail = item.thumbnail,
                    price = item.price,
                    count = productsList[item.id],
                    discount = member == null ? 0 : member.discount,
                    //选择了会员，并且会员折扣在0到10之间才算有折扣
                    isDiscounted = isDiscount == 1 && member != null && member.discount > 0 && member.discount < 10
                };
                tmp.money = tmp.price * tmp.count;
                tmp.discount_money = isDiscount == 1 && tmp.discount > 0 && tmp.discount < 10 ? (tmp.money * (decimal)(1 - tmp.discount / 10)) : 0;
                datas.Add(tmp);
            }

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
            List<product> datas = productDAL.GetInstance().GetProducts(string.Empty, keyWord, string.Empty, page, pageSize);
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
    }
}
