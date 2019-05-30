using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Spetmall.BLL.Page;
using Spetmall.DAL;
using Spetmall.Model;

namespace Moqikaka.Tmp.Admin.Controllers
{
    public class ProductController : Controller
    {
        //
        // GET: /Product/

        public ActionResult Index(string category, string keyWord, string orderBy)
        {
            List<product> datas = productDAL.GetInstance().GetProducts(category, keyWord, orderBy, 1, int.MaxValue);
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


    }
}
