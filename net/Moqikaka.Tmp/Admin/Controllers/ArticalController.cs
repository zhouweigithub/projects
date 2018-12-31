using Moqikaka.Tmp.DAL;
using Moqikaka.Tmp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Moqikaka.Tmp.Admin.Controllers
{
    [Common.CustomAuthorize]
    public class ArticalController : Controller
    {

        public ActionResult Index(string titlePara, string areaPara, int yearPara = 0, int page = 1, int pageSize = 20)
        {
            List<MArticalBase> datas = MArticalDAL.GetArticals(titlePara, yearPara, areaPara, page, pageSize);

            int totalCount = MArticalDAL.GetArticalsCount(titlePara, yearPara, areaPara);

            ViewBag.datas = datas;

            ViewBag.YearList = GetYearList();
            ViewBag.AreaList = GetAreaList();

            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalCount = totalCount;

            return View();
        }

        public ActionResult EditArtical(int id)
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
            var ret = true;
            var retMsg = string.Empty;

            try
            {
                model.Title = model.Title.Trim();

                if (model.Id != 0)
                {   //编辑
                    MArtical serverInfo = DAL.DBData.GetInstance(DAL.DBTable.m_artical).GetEntityByKey<MArtical>(model.Id);
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
                Moqikaka.Util.Log.LogUtil.Write("EditArtical 出错\r\n" + ex.Message, Util.Log.LogType.Error);
            }

            return Json(new
            {
                Result = ret,
                Message = ret ? "操作成功！" : ("操作失败!" + retMsg)
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteArtical(string id)
        {
            bool ret = false;
            string retMsg = string.Empty;

            try
            {
                ret = DAL.DBData.GetInstance(DAL.DBTable.m_artical).DeleteByKey(id) > 0;
            }
            catch (Exception ex)
            {
                retMsg = ex.Message;
                Moqikaka.Util.Log.LogUtil.Write("DeleteArtical 出错\r\n" + ex.Message, Util.Log.LogType.Error);
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
            for (int i = 2018; i >= 2000; i--)
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
            string[] provinceList = new string[] { "全国", "北京", "天津", "上海", "重庆", "河北", "山西", "辽宁", "吉林", "黑龙江", "江苏", "浙江", "安徽", "福建", "江西", "山东", "河南", "湖北", "湖南", "广东", "海南", "四川", "贵州", "云南", "陕西", "甘肃", "青海", "台湾", "内蒙古", "广西", "西藏", "宁夏", "新疆", "香港", "澳门" };
            for (int i = 0; i < provinceList.Length; i++)
            {
                list.Add(new SelectListItem() { Text = provinceList[i], Value = provinceList[i] });
            }

            return list;
        }
    }
}
