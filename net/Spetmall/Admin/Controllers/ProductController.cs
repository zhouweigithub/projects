﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Spetmall.BLL.Page;
using Spetmall.DAL;
using Spetmall.Model;

namespace Spetmall.Admin.Controllers
{
    public class ProductController : Controller
    {
        //
        // GET: /Product/

        public ActionResult Index(string category, string keyWord, string orderBy)
        {
            List<product> datas = productDAL.GetInstance().GetProducts(string.Empty, category, keyWord, orderBy, 1, int.MaxValue);
            List<category> categorys = categoryDAL.GetInstance().GetFloorDatas();
            ViewBag.categorys = categorys;
            ViewBag.datas = datas;
            return View();
        }

        public ActionResult Edit(int id)
        {
            product product = null;
            if (id == 0)
            {   //添加
                product = new product();
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
                if (product.id == 0)
                {
                    status = productDAL.GetInstance().Add<product>(product) > 0;
                }
                else
                {
                    status = productDAL.GetInstance().UpdateByKey<product>(product, product.id) > 0;
                }
            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }
            return Content(CommonBLL.GetReturnJson(status, errMsg));
        }

        public List<SelectListItem> GetCategoryItems()
        {
            List<SelectListItem> result = new List<SelectListItem>();
            List<category> list = categoryDAL.GetInstance().GetFloorDatas();
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
    }
}
