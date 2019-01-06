//***********************************************************************************
//文件名称：RoleController.cs
//功能描述：角色管理页
//数据表： 
//作者：周围
//日期：2017-09-18
//修改记录：
//***********************************************************************************
using Moqikaka.Tmp.Admin.Models;
using Moqikaka.Tmp.Common;
using Moqikaka.Tmp.DAL;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Moqikaka.Tmp.Admin.Controllers
{
    [Common.CustomAuthorize]
    /// <summary>
    /// 角色信息管理
    /// </summary>
    public class RoleController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// 查询条件
        /// </summary>
        /// <returns></returns>
        public ActionResult Search()
        {
            return View();
        }

        /// <summary>
        /// 查询所有角色信息 以列表返回
        /// </summary>
        /// <returns></returns>
        public ActionResult Data()
        {
            Model.RoleInfoListModel model = RoleDAL.GetListRoles();
            return View(model);
        }

        /// <summary>
        /// 编辑初始化
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(Model.RoleModel model)
        {
            //读取权限xml
            var roleRights = XmlHelper.XmlDeserializeFromFile<MenuModel>(Server.MapPath("~/App_Data/Menu.xml"), System.Text.Encoding.UTF8);
            string[] extraPageMenuIdArray = WebConfigData.ExtraPageMenuIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            roleRights.MenuGroupList.ForEach(a => a.MenuItemList.RemoveAll(b => extraPageMenuIdArray.Contains(b.ID)));//移除相应不予显示的特殊页面

            ViewBag.RoleRights = roleRights;
            ViewBag.OnSuccess = "onDataMidff";
            ViewBag.Title = "编辑";
            return View("Add", model);
        }

        /// <summary>
        /// 初始化信息 
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            //读取权限xml
            var roleRights = XmlHelper.XmlDeserializeFromFile<MenuModel>(Server.MapPath("~/App_Data/Menu.xml"), System.Text.Encoding.UTF8);
            string[] extraPageMenuIdArray = WebConfigData.ExtraPageMenuIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            roleRights.MenuGroupList.ForEach(a => a.MenuItemList.RemoveAll(b => extraPageMenuIdArray.Contains(b.ID)));//移除相应不予显示的特殊页面

            ViewBag.RoleRights = roleRights;
            ViewBag.OnSuccess = "onDataMidff";
            ViewBag.Title = "新增";
            Model.RoleModel viewModel = new Model.RoleModel();
            return View(viewModel);
        }

        [HttpPost]
        /// <summary>
        /// 新增【或编辑】保存角色信息
        /// </summary>
        /// <returns></returns>
        public ActionResult Add(Model.RoleModel model)
        {
            bool result = false;

            string strMsg = "";
            try
            {
                int iReturn = 0;

                model.Page = model.Page.TrimEnd(',');
                model.RolesName = model.RolesName.Trim();
                if (!string.IsNullOrWhiteSpace(model.Remark))
                    model.Remark = model.Remark.Trim();

                if (model.Id == 0)
                {
                    iReturn = DBData.GetInstance(DAL.DBTable.role).Add<Model.RoleModel>(model);
                }
                else
                {
                    iReturn = DBData.GetInstance(DAL.DBTable.role).UpdateByKey<Model.RoleModel>(model, model.Id);
                }
                if (iReturn > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                strMsg = ex.Message.ToString();
            }

            return Json(new
            {
                Result = result,
                Message = result ? "保存成功" : ("保存失败" + strMsg)
            }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 删除根据角色ID删除角色信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public string Delete(int Id)
        {
            bool result = false;
            try
            {
                if (DBData.GetInstance(DAL.DBTable.role).DeleteByKey(Id) > 0)
                    result = true;
            }
            catch (Exception)
            {

            }
            return result.ToString();

        }

        /// <summary>
        /// 判断是否存在该角色【新增或 编辑】
        /// </summary>
        /// <param name="Id">角色Id</param>
        /// <param name="RolesName">角色名称</param>
        /// <returns></returns>
        public string IsExsitRole(string Id, string RolesName)
        {
            Model.RoleModel model = new Model.RoleModel();
            model.Id = Convert.ToInt32(Id);
            model.RolesName = RolesName;
            return RoleDAL.IsExsitRole(model).ToString();
        }



    }
}
