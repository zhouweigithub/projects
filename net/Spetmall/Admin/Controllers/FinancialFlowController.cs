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
    [Common.CustomAuthorize]
    public class FinancialFlowController : Controller
    {

        public ActionResult Index(string keyWord, string startdate, string enddate, string type, string orderBy, int pageSize = 20, int page = 1)
        {
            List<financial_flow> datas = financialflowDAL.GetInstance().GetFinancialFlows(keyWord, startdate, enddate, type, orderBy, pageSize, page);
            int count = financialflowDAL.GetInstance().GetFinancialFlowsCount(keyWord, startdate, enddate, type);
            financial_flow total = financialflowDAL.GetInstance().GetFinancialFlowsTotal(keyWord, startdate, enddate, type);

            ViewBag.TypeList = GetTypeListItems(true);
            ViewBag.keyWord = keyWord;
            ViewBag.startdate = startdate;
            ViewBag.enddate = enddate;
            ViewBag.type = type;
            ViewBag.orderBy = orderBy;
            ViewBag.pageSize = pageSize;
            ViewBag.page = page;
            ViewBag.datas = datas;
            ViewBag.total = total;

            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalDataCount = count;

            return View();
        }

        public ActionResult Edit(int id)
        {
            ViewBag.TypeList = GetTypeListItems(false);
            financial_flow flow = null;
            if (id == 0)
            {   //添加
                flow = new financial_flow() { date = DateTime.Today, type = 1 };
            }
            else
            {   //编辑
                flow = financialflowDAL.GetInstance().GetEntityByKey<financial_flow>(id);
            }

            return View(flow);
        }

        [HttpPost]
        public ActionResult Edit(financial_flow flow)
        {
            bool status = false;
            string errMsg = string.Empty;
            try
            {
                if (!string.IsNullOrWhiteSpace(flow.remark))
                    flow.remark = flow.remark.Trim();
                if (flow.type == -1 && flow.money > 0)
                    flow.money = -flow.money;   //支出的金额置为负数
                else if (flow.type == 1 && flow.money < 0)
                    flow.money = -flow.money;

                if (flow.id == 0)
                {
                    status = financialflowDAL.GetInstance().Add(flow) > 0;
                }
                else
                {
                    status = financialflowDAL.GetInstance().UpdateByKey(flow, flow.id) > 0;
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
                status = financialflowDAL.GetInstance().DeleteByKey(id) > 0;
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

        private List<SelectListItem> GetTypeListItems(bool hasAll)
        {
            List<SelectListItem> result = new List<SelectListItem>();
            if (hasAll)
                result.Add(new SelectListItem() { Text = "ALL", Value = "" });

            result.Add(new SelectListItem() { Text = "收入", Value = "1" });
            result.Add(new SelectListItem() { Text = "支出", Value = "-1" });

            return result;
        }
    }
}
