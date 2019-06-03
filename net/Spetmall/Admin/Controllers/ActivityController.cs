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
        //限时折扣
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
                discount.starttime = DateTime.Now.Date;
                discount.endtime = DateTime.Now.Date;
            }
            else
            {   //编辑
                discount = DiscountBLL.GetEditInfo(id);

                //先分配空值
                ViewBag.products = null;
                ViewBag.parents = null;

                if (discount.type == 2)
                {
                    var products = discount.products.Select(a => new
                    {
                        a.id,
                        title = a.productName
                    });
                    ViewBag.products = Util.Json.JsonUtil.Serialize(products);
                }
                else if (discount.type == 1)
                {
                    if (discount.products.Count > 0)
                    {
                        List<int> parents = new List<int>();
                        categoryDAL.GetInstance().GetParentIds(discount.products[0].productid, parents);
                        parents.Reverse();
                        ViewBag.parents = parents;
                    }
                }

                var rules = discount.rules.Select(a => new { min = a.aim, discount = a.sale });
                ViewBag.rules = Util.Json.JsonUtil.Serialize(rules);

            }

            return View(discount);
        }

        [HttpPost]
        public ActionResult DiscountEdit(discount_post discount)
        {
            bool status = true;
            string errMsg = "操作成功";
            try
            {
                if (discount.id == 0)
                {   //添加
                    discount.state = 1;
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
                status = false;
                errMsg = e.Message;
            }

            return Json(new
            {
                code = 0,
                status,
                msg = errMsg,
                href = "/Activity/DiscountIndex",
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DiscountDelete(int id)
        {
            bool status = false;
            string errMsg = string.Empty;
            try
            {
                status = discountDAL.GetInstance().DeleteByKey(id) > 0;
                saleProductDAL.GetInstance().Delete($"saleid={id}");
                saleRuleDAL.GetInstance().Delete($"saleid={id}");
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



        //满就减
        public ActionResult FullSendIndex()
        {
            IList<fullsend> datas = fullsendDAL.GetInstance().GetList<fullsend>();
            ViewBag.datas = datas;
            return View();
        }

        public ActionResult FullSendEdit(int id)
        {
            fullsend_edit fullsend = null;
            if (id == 0)
            {   //添加
                fullsend = new fullsend_edit();
                fullsend.starttime = DateTime.Now.Date;
                fullsend.endtime = DateTime.Now.Date;
            }
            else
            {   //编辑
                fullsend = FullSendBLL.GetEditInfo(id);

                //先分配空值
                ViewBag.products = null;
                ViewBag.parents = null;

                if (fullsend.type == 2)
                {
                    var products = fullsend.products.Select(a => new
                    {
                        a.id,
                        title = a.productName
                    });
                    ViewBag.products = Util.Json.JsonUtil.Serialize(products);
                }
                else if (fullsend.type == 1)
                {
                    if (fullsend.products.Count > 0)
                    {
                        List<int> parents = new List<int>();
                        categoryDAL.GetInstance().GetParentIds(fullsend.products[0].productid, parents);
                        parents.Reverse();
                        ViewBag.parents = parents;
                    }
                }

                var rules = fullsend.rules.Select(a => new { min_price = a.aim, reduce_price = a.sale, goods = new string[] { } });
                ViewBag.rules = Util.Json.JsonUtil.Serialize(rules);

            }

            return View(fullsend);
        }

        [HttpPost]
        public ActionResult FullSendEdit(fullsend_post fullsend)
        {
            bool status = true;
            string errMsg = "操作成功";
            try
            {
                if (fullsend.id == 0)
                {   //添加
                    fullsend.state = 1;
                    status = fullsendDAL.InsertData(fullsend);
                }
                else
                {   //编辑
                    status = fullsendDAL.GetInstance().UpdateByKey<fullsend>(fullsend, fullsend.id) > 0;

                    //删除原有的活动商品和相关规则
                    saleProductDAL.GetInstance().Delete($"saleid={fullsend.id}");
                    saleRuleDAL.GetInstance().Delete($"saleid={fullsend.id}");

                    //添加新的商品和规则
                    fullsendDAL.InsertExtraDatas(fullsend, fullsend.id);
                }
            }
            catch (Exception e)
            {
                status = false;
                errMsg = e.Message;
            }

            return Json(new
            {
                code = 0,
                status,
                msg = errMsg,
                href = "/Activity/FullSendIndex",
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FullSendDelete(int id)
        {
            bool status = false;
            string errMsg = string.Empty;
            try
            {
                status = fullsendDAL.GetInstance().DeleteByKey(id) > 0;
                saleProductDAL.GetInstance().Delete($"saleid={id}");
                saleRuleDAL.GetInstance().Delete($"saleid={id}");
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

        public ActionResult UpdateFullSendState(int id, short state)
        {
            bool status = false;
            string errMsg = string.Empty;
            try
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("state", state);
                status = fullsendDAL.GetInstance().UpdateByKey(dic, id) > 0;
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




        public ActionResult ChooseProducts(string productId, string keyWord)
        {
            List<product> datas = productDAL.GetInstance().GetProducts(productId, string.Empty, keyWord, string.Empty, 1, int.MaxValue);
            ViewBag.datas = datas;
            return View();
        }

        public ActionResult GetCategorys(int parentid, int hierarchy)
        {
            bool status = false;
            IList<category> datas = null;

            try
            {
                datas = categoryDAL.GetInstance().GetList<category>($"pid={parentid}");
                status = true;
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write("获取分类数据出错：" + e, Util.Log.LogType.Error);
            }

            return Json(new
            {
                status,
                categorys = datas
            }, JsonRequestBehavior.AllowGet);
        }


    }
}
