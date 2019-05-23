﻿//***********************************************************************************
//文件名称：Authentication.cs
//功能描述：身份验证实现
//数据表： 
//作者：周围
//日期：2017-09-18
//修改记录：
//***********************************************************************************
using Spetmall.DAL;
using Spetmall.Model;
using System;
using System.Web;
using System.Web.Security;

namespace Spetmall.Admin.Common
{
    /// <summary>
    /// 身份验证实现
    /// </summary>
    public static class Authentication
    {
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="user"></param>
        /// <param name="createPersistentCookie"></param>
        public static void SignIn(Models.UserViewModel user, bool createPersistentCookie = true)
        {
            var now = DateTime.UtcNow.ToLocalTime();
            var ticket = new FormsAuthenticationTicket(
                1,
                user.UserName,
                now,
                now.AddHours(2),
                createPersistentCookie,
                user.Password,
                FormsAuthentication.FormsCookiePath);
            var encryptedTicket = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            cookie.HttpOnly = true;
            if (ticket.IsPersistent)
            {
                cookie.Expires = ticket.Expiration;
            }
            cookie.Secure = FormsAuthentication.RequireSSL;
            cookie.Path = FormsAuthentication.FormsCookiePath;
            HttpContext.Current.Response.Cookies.Remove(cookie.Name);
            HttpContext.Current.Response.Cookies.Add(cookie);
            HttpContext.Current.Session["LoginUserName"] = user.UserName;
            HttpContext.Current.Session["LoginUserId"] = user.id;

            //记住权限Pages
            try
            {
                string[] lstRole = user.roleids.Split(',');
                //获取角色权限表
                //获取角色信息列表
                RoleInfoListModel LstRoleInfo = RoleDAL.GetInstance().GetListRoles();

                string strPages = "";
                foreach (var kk in lstRole)
                {
                    var roleInfo = LstRoleInfo.RoleList.Find(m => m.Id == Convert.ToInt32(kk));
                    if (roleInfo != null)
                    {
                        strPages += roleInfo.Page + ",";
                    }
                }
                strPages = strPages.TrimEnd(',');
                HttpContext.Current.Session["Pages"] = strPages;
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 登出
        /// </summary>
        public static void SignOut()
        {
            HttpContext.Current.Session.Clear();

            FormsAuthentication.SignOut();
        }

        /// <summary>
        /// 获取验证的用户
        /// </summary>
        /// <returns></returns>
        public static Models.UserViewModel GetAuthenticatedUser()
        {
            if (HttpContext.Current == null ||
                HttpContext.Current.Request == null ||
                !HttpContext.Current.Request.IsAuthenticated ||
                !(HttpContext.Current.User.Identity is FormsIdentity))
            {
                return null;
            }

            var formsIdentity = (FormsIdentity)HttpContext.Current.User.Identity;
            var user = GetAuthenticatedUserFromTicket(formsIdentity.Ticket);

            return user;
        }

        /// <summary>
        /// 从票据中获取用户信息
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        public static Models.UserViewModel GetAuthenticatedUserFromTicket(FormsAuthenticationTicket ticket)
        {
            var userData = ticket.UserData;
            if (String.IsNullOrWhiteSpace(userData))
                return null;
            else
            {
                return new Models.UserViewModel() { UserName = ticket.Name, Password = userData };
            }
        }
    }
}