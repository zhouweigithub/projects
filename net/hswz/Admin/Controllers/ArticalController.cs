using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Hswz.DAL;
using Hswz.Model;

namespace Hswz.Admin.Controllers
{
    [Common.CustomAuthorize]
    public class ArticalController : Controller
    {

        public ActionResult Index(String titlePara, String areaPara, Int32 yearPara = 0, Int32 page = 1, Int32 pageSize = 20)
        {
            var datas = MArticalDAL.GetArticals(titlePara, yearPara, areaPara, page, pageSize);

            Int32 totalCount = MArticalDAL.GetArticalsCount(titlePara, yearPara, areaPara);

            ViewBag.datas = datas;

            ViewBag.YearList = GetYearList();
            ViewBag.AreaList = GetAreaList();

            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalCount = totalCount;

            return View();
        }

        public ActionResult EditArtical(Int32 id)
        {
            MArtical artical = null;
            ViewBag.YearList = GetYearList();
            ViewBag.AreaList = GetAreaList();

            if (id != 0)
            {
                artical = DAL.DBData.GetInstance(DAL.DBTable.m_artical).GetEntityByKey<MArtical>(id);
                ViewBag.title = "修改文章";
            }
            else
            {
                artical = new MArtical();
                ViewBag.title = "添加文章";
            }

            return View(artical);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditArticalPost(Model.MArtical model, FormCollection collection)
        {
            Boolean ret = true;
            String retMsg = String.Empty;

            try
            {
                model.Title = model.Title.Trim();

                if (model.Id != 0)
                {   //编辑
                    var serverInfo = DAL.DBData.GetInstance(DAL.DBTable.m_artical).GetEntityByKey<MArtical>(model.Id);
                    serverInfo.Title = model.Title;
                    serverInfo.Content = model.Content;
                    serverInfo.Year = model.Year;
                    serverInfo.Area = model.Area;

                    ret = DAL.DBData.GetInstance(DAL.DBTable.m_artical).UpdateByKey(serverInfo, model.Id) > 0;
                }
                else
                {   //添加
                    model.CrTime = DateTime.Now;
                    ret = DAL.DBData.GetInstance(DAL.DBTable.m_artical).Add(model) > 0;
                }

            }
            catch (Exception ex)
            {
                retMsg = ex.Message;
                Util.Log.LogUtil.Write("EditArtical 出错\r\n" + ex.Message, Util.Log.LogType.Error);
            }

            return Json(new
            {
                Result = ret,
                Message = ret ? "操作成功！" : ("操作失败!" + retMsg)
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteArtical(String id)
        {
            Boolean ret = false;
            String retMsg = String.Empty;

            try
            {
                ret = DAL.DBData.GetInstance(DAL.DBTable.m_artical).DeleteByKey(id) > 0;
            }
            catch (Exception ex)
            {
                retMsg = ex.Message;
                Util.Log.LogUtil.Write("DeleteArtical 出错\r\n" + ex.Message, Util.Log.LogType.Error);
            }

            return Json(new
            {
                Result = ret,
                Message = ret ? "删除成功！" : ("删除失败！" + retMsg)
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取年份列表
        /// </summary>
        /// <returns></returns>
        private List<SelectListItem> GetYearList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            for (Int32 i = 2018; i >= 1999; i--)
            {
                list.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }

            return list;
        }

        /// <summary>
        /// 获取区域列表
        /// </summary>
        /// <returns></returns>
        private List<SelectListItem> GetAreaList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            String[] provinceList = new String[] { "全国", "北京", "天津", "上海", "重庆", "河北", "山西", "辽宁", "吉林", "黑龙江", "江苏", "浙江", "安徽", "福建", "江西", "山东", "河南", "湖北", "湖南", "广东", "海南", "四川", "贵州", "云南", "陕西", "甘肃", "青海", "台湾", "内蒙古", "广西", "西藏", "宁夏", "新疆", "香港", "澳门" };
            for (Int32 i = 0; i < provinceList.Length; i++)
            {
                list.Add(new SelectListItem() { Text = provinceList[i], Value = provinceList[i] });
            }

            return list;
        }
    }
}
