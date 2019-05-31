using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Spetmall.BLL.Page;
using Spetmall.DAL;
using Spetmall.Model;
using Spetmall.Model.Page;

namespace Moqikaka.Tmp.Admin.Controllers
{
    public class ActivityController : Controller
    {

        public ActionResult DiscountIndex()
        {
            IList<discount> datas = discountDAL.GetInstance().GetList<discount>();
            ViewBag.datas = datas;
            return View();
        }

        public ActionResult DiscountEdit(int id)
        {
            discount_edit discount = null;
            if (id == 0)
            {   //添加
                discount = new discount_edit();
            }
            else
            {   //编辑
                discount = DiscountBLL.GetEditInfo(id);
            }

            return View(discount);
        }

        [HttpPost]
        public ActionResult DiscountEdit(discount_post discount)
        {
            bool status = false;
            string errMsg = string.Empty;
            try
            {
                if (discount.id == 0)
                {   //添加
                    status = discountDAL.InsertData(discount);
                }
                else
                {   //编辑
                    status = discountDAL.GetInstance().UpdateByKey<discount>(discount, discount.id) > 0;

                    //删除原有的活动商品和相关规则
                    saleProductDAL.GetInstance().Delete($"saleid={discount.id}");
                    saleRuleDAL.GetInstance().Delete($"saleid={discount.id}");

                    //添加新的商品和规则
                    discountDAL.InsertExtraDatas(discount, discount.id);
                }
            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }
            return Content(CommonBLL.GetReturnJson(status, errMsg));
        }

        public ActionResult DiscountDelete(int id)
        {
            bool status = false;
            string errMsg = string.Empty;
            try
            {
                status = discountDAL.GetInstance().DeleteByKey(id) > 0;
            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }

            return Json(new
            {
                status,
                msg = errMsg,
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateDiscountState(int id, short state)
        {
            bool status = false;
            string errMsg = string.Empty;
            try
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("state", state);
                status = discountDAL.GetInstance().UpdateByKey(dic, id) > 0;
            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }

            return Json(new
            {
                status,
                msg = errMsg,
            }, JsonRequestBehavior.AllowGet);
        }


    }
}
