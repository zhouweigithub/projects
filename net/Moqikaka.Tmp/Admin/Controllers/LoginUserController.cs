//***********************************************************************************
//文件名称：LoginUserController.cs
//功能描述：用户管理
//数据表： 
//作者：周围
//日期：2017-09-18
//修改记录：
//***********************************************************************************
using Moqikaka.Tmp.DAL;
using Moqikaka.Tmp.Model;
using Moqikaka.Util.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace Moqikaka.Tmp.Admin.Controllers
{

    /// <summary>
    /// 用户信息
    /// </summary>
    [Common.CustomAuthorize]
    public class LoginUserController : Controller
    {
        //单例

        public ActionResult LoginUserIndex()
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
        public ActionResult Data(string s_username)
        {
            var model = LoginUserDAL.GetListUserInfo(s_username);
            return View(model);
        }

        /// <summary>
        /// 编辑初始化
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(LoginUserModel model)
        {
            //读取权限
            //var roleRights = RoleDAL.GetListRoles();
            //ViewBag.Rights = roleRights;
            ViewBag.Title = "编辑";
            Model.MUser user = DBData.GetInstance(DBTable.m_user).GetEntityByKey<Model.MUser>(model.id);

            if (user != null)   //密码不能显示到界面上
                user.Password = string.Empty;

            return View("Add", user);
        }

        /// <summary>
        /// 初始化信息 
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            //读取权限
            //var roleRights = RoleDAL.GetListRoles();
            //ViewBag.Rights = roleRights;
            ViewBag.Title = "新增";
            Model.MUser viewModel = new Model.MUser();
            return View(viewModel);
        }

        [HttpPost]
        /// <summary>
        /// 新增【或编辑】保存用户信息
        /// </summary>
        /// <returns></returns>
        public ActionResult Add(MUser model)
        {
            bool result = false;

            string strMsg = "";
            try
            {
                int iReturn = 0;

                //去除空白字符
                model.UserName = model.UserName.Trim();
                if (!string.IsNullOrWhiteSpace(model.Name))
                    model.Name = model.Name.Trim();
                if (!string.IsNullOrWhiteSpace(model.Password))
                    model.Password = model.Password.Trim();

                if (model.Id == 0)
                {   //新增
                    model.Password = MD5Util.MD5(model.Password);
                    iReturn = DBData.GetInstance(DBTable.m_user).Add(model);
                }
                else
                {   //编辑
                    MUser serverData = DBData.GetInstance(DBTable.m_user).GetEntityByKey<MUser>(model.Id);

                    serverData.Name = model.Name;
                    serverData.UserName = model.UserName;
                    serverData.Status = model.Status;

                    if (!string.IsNullOrEmpty(model.Password))
                    {   //填写了密码就修改密码
                        serverData.Password = MD5Util.MD5(model.Password);
                    }

                    iReturn = DBData.GetInstance(DBTable.m_user).UpdateByKey(serverData, serverData.Id);
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
        /// 个人修改密码界面
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangePassword()
        {
            return View();
        }

        //个人修改密码
        [HttpPost]
        public ActionResult ChangePassword(string oldpassword, string password)
        {
            bool result = false;
            string strMsg = "";

            try
            {
                if (string.IsNullOrEmpty(oldpassword) || string.IsNullOrEmpty(password))
                {
                    strMsg = "旧密码不正确！";
                }
                else
                {
                    string loginUserName = Session["LoginUserName"].ToString();
                    oldpassword = MD5Util.MD5(oldpassword);
                    password = MD5Util.MD5(password);

                    if (LoginUserDAL.IsPasswordValid(loginUserName, oldpassword))
                    {
                        result = LoginUserDAL.UpdatePassword(loginUserName, password);
                    }
                    else
                    {
                        strMsg = "旧密码不正确！";
                    }
                }
            }
            catch (Exception ex)
            {
                strMsg = ex.Message.ToString();
            }

            return Json(new
            {
                Result = result,
                Message = result ? "修改成功" : ("修改失败：" + strMsg)
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult UnlockLogin(string username)
        {
            bool result = true;
            string strMsg = string.Empty;

            try
            {
                string _appKey = Moqikaka.Tmp.Common.Const.Application_Login_Failed_Times_ + username;
                System.Web.HttpContext.Current.Application.Remove(_appKey);
            }
            catch (Exception e)
            {
                result = false;
                strMsg = e.Message;
                Util.Log.LogUtil.Write(string.Format("解除登录限制失败：username: {0} , reason: {1}", username, e.ToString()), Util.Log.LogType.Error);
            }

            return Json(new
            {
                Result = result,
                Message = result ? "解除登录限制成功【" + username + "】" : ("解除登录限制失败【" + username + "】")
            }, JsonRequestBehavior.AllowGet);
        }





        #region private methods

        /// <summary>
        /// 删除根据ID删除用户信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public string Delete(int Id)
        {
            bool result = false;
            try
            {
                if (DBData.GetInstance(DBTable.m_user).DeleteByKey(Id) > 0)
                    result = true;
            }
            catch (Exception)
            {

            }
            return result.ToString();

        }

        /// <summary>
        /// 判断是否存在该用户【新增或 编辑】
        /// </summary>
        /// <param name="Id">用户Id</param>
        /// <param name="RolesName">用户名称</param>
        /// <returns></returns>
        public bool IsExsitUser(string username)
        {
            return (DBData.GetInstance(DBTable.m_user).GetCount($"username='{username}'") > 0);
        }

        public string CheckLetter(String password)
        {
            bool result = false;
            var letters = new List<string>();
            if (!String.IsNullOrEmpty(password))
            {
                if (password.Length < 8)
                {
                    return result.ToString();
                }
                foreach (char c in password)
                {
                    if (c >= 'a' && c <= 'z')
                    {
                        if (!letters.Contains("a")) letters.Add("a");
                    }
                    if (c >= 'A' && c <= 'Z')
                    {
                        if (!letters.Contains("A")) letters.Add("A");
                    }
                }
            }
            if (letters.Count == 2) result = true;
            return result.ToString();
        }


        #endregion

    }
}
