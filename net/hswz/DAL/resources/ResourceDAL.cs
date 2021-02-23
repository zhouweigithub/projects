using System;
using System.Collections.Generic;
using Hswz.Model.Urls;
using MySql.Data.MySqlClient;

namespace Hswz.DAL
{
    /// <summary>
    /// 资源数据
    /// </summary>
    public class ResourceDAL
    {
        /// <summary>
        /// 获取资源数据
        /// </summary>
        /// <param name="name">资源名称</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">页资源数</param>
        /// <returns></returns>
        public static IList<resource> GetList(String name, Int32 page, Int32 pageSize)
        {
            MySqlParameter para = new MySqlParameter("name", $"%{name}%");
            return DBData.GetInstance(DBTable.resource).GetListPage<resource>(pageSize, page, $"rname like @name", para);
        }

        /// <summary>
        /// 获取资源总数量
        /// </summary>
        /// <param name="name">资源名称</param>
        /// <returns></returns>
        public static Int32 GetCount(String name)
        {
            MySqlParameter para = new MySqlParameter("name", $"%{name}%");
            return DBData.GetInstance(DBTable.resource).GetCount($"rname like @name", para);
        }
    }
}
