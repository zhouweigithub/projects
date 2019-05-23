﻿//***********************************************************************************
//文件名称：MenuProvider.cs
//功能描述：当前用户的菜单权限处理
//数据表： 
//作者：周围
//日期：2017-09-18
//修改记录：
//***********************************************************************************
using Spetmall.Admin.Models;
using Spetmall.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Spetmall.Admin.Common.Menu
{
    public class MenuProvider
    {
        private static int menuCacheTime = 60;
        /// <summary>
        /// 获取当前登录用户菜单
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<MenuGroup> GetMenuFromCurrentLoginUser()
        {
            IEnumerable<MenuGroup> menus = new List<MenuGroup>();

            if (HttpContext.Current.Session["Pages"] != null)
            {
                menus = GetMenuFromSelected(HttpContext.Current.Session["Pages"].ToString(), HttpContext.Current.Session["LoginUserName"].ToString());
            }

            return menus;
        }

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <returns></returns>
        public static MenuModel GetMenu()
        {
            string cacheName = "UI.MenuNew";

            MenuModel menuModel = null;

            // 自定义菜单 需要重新加载菜单
            if (MemoryCacheManager.IsSet(cacheName))
            {
                menuModel = MemoryCacheManager.Get<MenuModel>(cacheName);
            }
            else
            {
                try
                {
                    menuModel = XmlHelper.XmlDeserializeFromFile<MenuModel>(HttpContext.Current.Server.MapPath("~/App_Data/Menu.xml"), Encoding.UTF8);
                }
                catch (Exception)
                {
                }

                if (menuModel != null)
                {
                    MemoryCacheManager.Set(cacheName, menuModel, menuCacheTime);
                }
            }

            return menuModel;
        }


        /// <summary>
        /// 根据指定ID获取菜单
        /// </summary>
        /// <param name="selectedMenuPerList"></param>
        /// <returns></returns>
        private static List<MenuGroup> GetMenuFromSelected(string selectedMenuPerList, string loginUserName)
        {
            if (string.IsNullOrWhiteSpace(selectedMenuPerList))
                return new List<MenuGroup>();

            string[] menuIDS = selectedMenuPerList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var menus = MenuProvider.GetMenu();

            return RecursionMenuForSelected(menus, menuIDS, loginUserName);
        }

        /// <summary>
        /// 递归选中菜单
        /// </summary>
        public static List<MenuGroup> RecursionMenuForSelected(MenuModel menus, string[] menuIDS, string loginUserName)
        {
            List<MenuGroup> MenuGrouplist = new List<MenuGroup>();
            if (menus.MenuGroupList != null && menus.MenuGroupList.Count > 0)
            {
                //是否超级用户
                bool IfSuper = false;
                if (HttpContext.Current.Session["IfSuper"] != null)
                {
                    if (HttpContext.Current.Session["IfSuper"].ToString() == "1")
                    {
                        IfSuper = true;
                    }
                }

                ///循环一级标题
                foreach (var item in menus.MenuGroupList)
                {

                    List<Spetmall.Admin.Models.MenuItem> menuItemList = new List<Spetmall.Admin.Models.MenuItem>();

                    string[] extraUsernameArray = WebConfigData.ExtraUserNames.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    string[] extraPageMenuIdArray = WebConfigData.ExtraPageMenuIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var menu in item.MenuItemList)
                    {
                        #region 需要修改用户权限add by ch 20170330 TODO
                        bool isOk = false;
                        if (extraPageMenuIdArray.Contains(menu.ID))
                        {   //特殊页面
                            if (extraUsernameArray.Contains(loginUserName))
                            {   //特殊用户
                                isOk = true;
                            }
                        }
                        else
                        {   //正常页面
                            if (IfSuper || menuIDS.Contains(menu.ID))
                            {
                                isOk = true;
                            }
                        }

                        if (isOk)
                        {
                            var selectItem = new Spetmall.Admin.Models.MenuItem()
                            {
                                ID = menu.ID,
                                Text = menu.Text,
                                Action = menu.Action,
                                Controller = menu.Controller
                            };

                            menuItemList.Add(selectItem);
                        }
                        #endregion
                    }

                    if (menuItemList.Count > 0)
                    {
                        MenuGroup menuGroup = new MenuGroup();
                        menuGroup.ID = item.ID;
                        menuGroup.text = item.text;
                        menuGroup.MenuItemList = menuItemList;
                        MenuGrouplist.Add(menuGroup);
                    }
                }
            }
            HttpContext.Current.Session["MenuGrouplist"] = MenuGrouplist;
            return MenuGrouplist;
        }
    }
}