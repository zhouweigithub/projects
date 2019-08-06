using Spetmall.DAL;
using Spetmall.Model;
using Spetmall.Model.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Spetmall.Admin.Controllers
{
    [Common.CustomAuthorize]
    public class ReportsController : Controller
    {

        public ActionResult Index(string startdate, string enddate, int pageSize = 20, int page = 1)
        {
            if (string.IsNullOrEmpty(startdate) && string.IsNullOrEmpty(enddate))
            {
                startdate = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
                enddate = DateTime.Now.ToString("yyyy-MM-dd");
            }
            List<payInfo> datas = reportsDAL.GetPayInfos(startdate, enddate, pageSize, page);
            int count = reportsDAL.GetPayInfosCount(startdate, enddate);
            payInfo total = reportsDAL.GetPayInfosTotal(startdate, enddate);

            ViewBag.datas = datas;
            ViewBag.total = total;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalDataCount = count;

            return View();
        }

        public ActionResult SalePerformance(string startdate, string enddate)
        {
            if (string.IsNullOrEmpty(startdate) && string.IsNullOrEmpty(enddate))
            {
                startdate = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
                enddate = DateTime.Now.ToString("yyyy-MM-dd");
            }
            List<payInfo> datas = reportsDAL.GetPayInfos(startdate, enddate, int.MaxValue, 1);
            string dateJson = Util.Json.JsonUtil.Serialize(datas.Select(a => a.crdate.ToString("yyyy-MM-dd")));
            string valueJson = Util.Json.JsonUtil.Serialize(datas.Select(a => a.payMoney));
            ViewBag.dateJson = dateJson;
            ViewBag.valueJson = valueJson;
            return View();
        }

        public ActionResult Products(int page = 1, int pageSize = 20)
        {
            List<product> datas = productDAL.GetInstance().GetProducts(string.Empty, string.Empty, string.Empty, "sales", page, pageSize).Cast<product>().ToList();
            int count = productDAL.GetInstance().GetProductsCount(string.Empty, string.Empty, string.Empty);

            ViewBag.datas = datas;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalDataCount = count;

            return View();
        }

        public ActionResult ProductDetail(int productid, string startdate, string enddate)
        {
            if (string.IsNullOrEmpty(startdate) && string.IsNullOrEmpty(enddate))
            {
                startdate = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
                enddate = DateTime.Now.ToString("yyyy-MM-dd");
            }
            string where = string.Empty;
            if (!string.IsNullOrEmpty(startdate))
                where += $" and crdate>='{startdate}'";
            if (!string.IsNullOrEmpty(enddate))
                where += $" and crdate<='{enddate}'";

            List<countPayInfo> datas = reportsDAL.GetProductInfos(productid, startdate, enddate);
            string dateJson = Util.Json.JsonUtil.Serialize(datas.Select(a => a.crdate.ToString("yyyy-MM-dd")));
            string valueJson = Util.Json.JsonUtil.Serialize(datas.Select(a => a.count));
            ViewBag.dateJson = dateJson;
            ViewBag.valueJson = valueJson;
            return View();
        }

        public ActionResult Category(string startdate, string enddate)
        {
            if (string.IsNullOrEmpty(startdate) && string.IsNullOrEmpty(enddate))
            {
                startdate = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
                enddate = DateTime.Now.ToString("yyyy-MM-dd");
            }
            return View();
        }

        public ActionResult Member(string startdate, string enddate)
        {
            if (string.IsNullOrEmpty(startdate) && string.IsNullOrEmpty(enddate))
            {
                startdate = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
                enddate = DateTime.Now.ToString("yyyy-MM-dd");
            }
            List<countPayInfo> datas = reportsDAL.GetMemberInfos(startdate, enddate);
            string dateJson = Util.Json.JsonUtil.Serialize(datas.Select(a => a.crdate.ToString("yyyy-MM-dd")));
            string valueJson = Util.Json.JsonUtil.Serialize(datas.Select(a => a.count));
            ViewBag.dateJson = dateJson;
            ViewBag.valueJson = valueJson;
            return View();
        }

    }
}
