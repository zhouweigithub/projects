﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Spetmall.BLL.Page;
using Spetmall.Common;
using Spetmall.DAL;
using Spetmall.Model;

namespace Spetmall.Admin.Controllers
{
    [Common.CustomAuthorize]
    public class ProductController : Controller
    {

        private static readonly string defaultProductImage = "/static/images/upload-pic.png";

        public ActionResult Index(string category, string keyWord, string orderBy, int pageSize = 20, int page = 1)
        {
            List<product_show> datas = productDAL.GetInstance().GetProducts(string.Empty, category, keyWord, orderBy, page, pageSize);
            int count = productDAL.GetInstance().GetProductsCount(string.Empty, category, keyWord);
            List<category> categorys = categoryDAL.GetInstance().GetFloorDatas(false);
            ViewBag.categorys = categorys;
            ViewBag.datas = datas;
            ViewBag.orderBy = orderBy;

            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalDataCount = count;
            //ViewBag.TotalPages = ((count % pageSize == 0) ? (count / pageSize) : (count / pageSize + 1));

            return View();
        }

        public ActionResult Edit(int id)
        {
            product product = null;
            if (id == 0)
            {   //添加
                product = new product()
                {
                    ismemberdiscount = 1,
                    thumbnail = defaultProductImage,
                };
            }
            else
            {   //编辑
                product = productDAL.GetInstance().GetEntityByKey<product>(id);
            }

            ViewBag.categoryItems = GetCategoryItems();
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(product product)
        {
            bool status = false;
            string errMsg = string.Empty;
            try
            {
                product.name = product.name.Trim();
                product.py = ChineseSpell.GetChineseSpell(product.name);

                //缩略图如果是默认图，则去掉
                if (product.thumbnail == defaultProductImage || string.IsNullOrWhiteSpace(product.thumbnail))
                    product.thumbnail = string.Empty;

                if (product.id == 0)
                {
                    status = productDAL.GetInstance().Add(product) > 0;
                }
                else
                {
                    status = productDAL.GetInstance().UpdateByKey(product, product.id) > 0;
                }
            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }
            return Content(CommonBLL.GetReturnJson(status, errMsg));
        }

        public ActionResult Delete(int id)
        {
            bool status = false;
            string errMsg = string.Empty;
            try
            {
                status = productDAL.GetInstance().DeleteByKey(id) > 0;
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

        public List<SelectListItem> GetCategoryItems()
        {
            List<SelectListItem> result = new List<SelectListItem>();
            List<category> list = categoryDAL.GetInstance().GetFloorDatas(false);
            foreach (category item in list)
            {
                string space = string.Empty;
                for (int i = 0; i < item.floor; i++)
                {
                    space += "　　";
                }
                result.Add(new SelectListItem()
                {
                    Text = space + item.name,
                    Value = item.id.ToString(),
                });
            }
            return result;
        }

        public ActionResult Export(string category, string keyWord, string orderBy)
        {
            try
            {
                string downloadFileName = string.Format("商品数据{0}.xlsx", DateTime.Now.ToString("yyyyMMddHHmmssffff"));
                List<product_show> datas = productDAL.GetInstance().GetProducts(string.Empty, category, keyWord, orderBy, 1, int.MaxValue);

                if (datas != null)
                {
                    string _strPath = System.Web.HttpContext.Current.Server.MapPath(Const.ExportPath);
                    string savedExcelPath = ExportBLL.ExportProductToExcel(datas, _strPath);

                    return File(savedExcelPath, "application/vnd.ms-excel", downloadFileName);
                }

            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write("导出商品数据失败：" + e, Util.Log.LogType.Error);
            }

            return new EmptyResult();
        }
    }
}
