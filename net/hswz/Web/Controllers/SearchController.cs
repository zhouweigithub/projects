using System;
using System.Web.Mvc;
using Hswz.DAL;
using Hswz.Model.Urls;
using MySql.Data.MySqlClient;

namespace Web.Controllers
{
    public class SearchController : Controller
    {

        public String List(String key, Int32 page =1, Int32 pageSize =20)
        {
            String where = GetWhere(key);
            MySqlParameter para = new MySqlParameter("@title", $"%{key}%");

            var datas = DBData.GetInstance(DBTable.resource_items).GetListPage<ResourceDetail>(pageSize,page,where, para);
            Int32 count = DBData.GetInstance(DBTable.resource_items).GetCount(where, para);

            Object obj = new { 
                datas,
                count
            };
            return Util.Json.JsonUtil.Serialize(obj);
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
    }
}