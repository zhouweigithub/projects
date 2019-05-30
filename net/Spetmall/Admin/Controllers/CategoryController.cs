using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Spetmall.Model;
using Spetmall.DAL;
using Spetmall.BLL.Page;

namespace Moqikaka.Tmp.Admin.Controllers
{
    public class CategoryController : Controller
    {

        public ActionResult Index()
        {
            List<category> datas = categoryDAL.GetInstance().GetFloorDatas();
            ViewBag.datas = datas;
            return View();
        }

        //添加顶级分类
        public ActionResult Add(int pid)
        {
            category category = new category()
            {
                pid = pid,
                state = 1,
            };
            return View("Edit", category);
        }

        //编辑非顶级分类
        public ActionResult Edit(int id)
        {
            category category = null;
            if (id == 0)
            {   //添加
                category = new category();
            }
            else
            {   //编辑
                category = categoryDAL.GetInstance().GetEntityByKey<category>(id);
            }
            return View(category);
        }

        [HttpPost]
        public ActionResult Edit(category category)
        {
            bool status = false;
            string errMsg = string.Empty;
            try
            {
                if (category.id == 0)
                {   //添加
                    status = categoryDAL.GetInstance().Add(category) > 0;
                }
                else
                {   //编辑
                    status = categoryDAL.GetInstance().UpdateByKey(category, category.id) > 0;
                }
            }
            catch (Exception e)
            {
                errMsg = e.Message;
                Util.Log.LogUtil.Write("编辑分类出错：" + e, Util.Log.LogType.Error);
            }

            return Content(CommonBLL.GetReturnJson(status, errMsg));
        }

        [HttpPost]
        public ActionResult EditField(int id, string field, string value)
        {
            bool status = false;
            string errMsg = string.Empty;
            try
            {
                Dictionary<string, object> keyValue = new Dictionary<string, object>();
                keyValue.Add(field, value);
                status = categoryDAL.GetInstance().UpdateByKey(keyValue, id) > 0;
            }
            catch (Exception e)
            {
                errMsg = e.Message;
                Util.Log.LogUtil.Write("编辑分类字段出错：" + e, Util.Log.LogType.Error);
            }

            return Json(new
            {
                msg = errMsg,
                status
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int id)
        {
            bool status = false;
            string errMsg = string.Empty;
            try
            {
                //更新category的剩余次数
                status = categoryDAL.GetInstance().DeleteByKey(id) > 0;
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
