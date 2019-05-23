using Spetmall.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Spetmall.Admin.Mvc.Html
{
    public static class Menu
    {
        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <returns></returns>
        public static MvcHtmlString CreateMenu(this HtmlHelper html, IEnumerable<MenuGroup> menuItems, Func<string, string, string> menuUrl)
        {  
            return MvcHtmlString.Create(CreateMenuList(html, menuItems, menuUrl));
        }

        /// <summary>
        /// 拼接菜单
        /// </summary>
        /// <param name="html"></param>
        /// <param name="menuItems"></param>
        /// <param name="menuUrl"></param>  
        /// <returns></returns>
        private static string CreateMenuList(HtmlHelper html, IEnumerable<MenuGroup> menuItems, Func<string, string, string> menuUrl)
        {  
            string menuHtml = string.Empty;

            try
            {
                if (menuItems != null && menuItems.Count() > 0)
                {  
                    //一级标题
                    string strFistInnerHtml = "";
                  
                    foreach (var menu in menuItems)
                    {
                        var liFirst = new TagBuilder("li");
                        var aFirst = new TagBuilder("a");
                        aFirst.Attributes["href"] = "#";
                        aFirst.Attributes["class"] = "dropdown-toggle";
                        aFirst.Attributes["data-toggle"] = "dropdown";
                        aFirst.InnerHtml = menu.text+"<span class='caret'></span>";
                        
                        var ulSecond = new TagBuilder("ul");
                        ulSecond.Attributes["class"] = "dropdown-menu";
                        #region 二级标题
                        if (menu.MenuItemList != null && menu.MenuItemList.Count > 0)
                        {
                            //二级标题
                            string strInnerHtml = "";
                            foreach (var item in menu.MenuItemList)
                            {
                                var liSecond = new TagBuilder("li");
                             
                                var aSecond = new TagBuilder("a");
                                aSecond.Attributes["href"] = menuUrl(item.Action, item.Controller);// +"?radom=" + DateTime.Now.ToString("hhmmss");
                                aSecond.InnerHtml = item.Text;
                                
                                liSecond.InnerHtml = aSecond.ToString();
                                strInnerHtml += liSecond.ToString();
                            }                           
                            ulSecond.InnerHtml = strInnerHtml;
                        }
                        #endregion
                        liFirst.InnerHtml = aFirst.ToString() + ulSecond.ToString();

                        strFistInnerHtml += liFirst.ToString();
                    }

                    menuHtml = strFistInnerHtml;
                }

            }
            catch (Exception)
            {
            }

            return menuHtml;
        }
    }
}