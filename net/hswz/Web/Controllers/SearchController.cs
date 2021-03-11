using System;
using System.Web.Mvc;
using Hswz.DAL;
using Hswz.Model.Urls;
using MySql.Data.MySqlClient;

namespace Web.Controllers
{
    public class SearchController : Controller
    {
        [HttpPost]
        public String List(String key, Int32 page = 1, Int32 pageSize = 20)
        {
            String where = GetWhere(key);
            MySqlParameter para = new MySqlParameter("@title", $"%{key}%");

            var datas = DBData.GetInstance(DBTable.resource_items).GetListPage<ResourceDetail>(pageSize, page, where, para);
            Int32 count = DBData.GetInstance(DBTable.resource_items).GetCount(where, para);

            Object obj = new
            {
                datas,
                count
            };
            return Util.Json.JsonUtil.Serialize(obj);
        }

        /// <summary>
        /// 资源访问记录
        /// </summary>
        /// <param name="id"></param>
        [HttpPost]
        public void Click(Int32 id)
        {
            DBData.GetInstance(DBTable.resource_click).Add(new resource_click()
            {
                resource_id = id,
                ip = GetRequestIP(),
                crdate = DateTime.Today,
                crtime = DateTime.Now,
            });
        }




        private String GetWhere(String key)
        {
            String result = "1=1";
            if (!String.IsNullOrWhiteSpace(key))
            {
                result += " and title like @title";
            }

            return result;
        }

        /// <summary>
        /// 获取客户端ip地址
        /// </summary>
        /// <returns></returns>
        private String GetRequestIP()
        {
            String userIP;
            var Request = HttpContext.Request;

            userIP = Request.UserHostAddress;

            if (String.IsNullOrEmpty(userIP))
            {
                // 如果使用代理，获取真实IP
                if (Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != "")
                {
                    userIP = Request.ServerVariables["REMOTE_ADDR"];
                }
                else
                {
                    userIP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                }
            }
            return userIP;
        }

    }
}