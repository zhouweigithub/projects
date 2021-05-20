using System;
using System.Collections.Generic;
using System.Linq;
using Hswz.DAL;
using Hswz.Model.Urls;

namespace ResourceSpider.GetItems
{
    public class DbCenter
    {

        /// <summary>
        /// 获取收集到的各个网站入口地址主机名列表，已去重
        /// </summary>
        /// <returns></returns>
        public static List<String> GetDistinctDomainHostUrls()
        {
            List<String> result = new List<String>(0);
            var datas = UrlDAL.GetList();
            if (datas != null && datas.Count > 0)
            {
                var lst = datas.Select(a => a.url);
                foreach (String item in lst)
                {
                    String host = Comm.GetUrlHost(item);
                    if (!result.Contains(host))
                    {
                        result.Add(host);
                    }
                }

                return result;
            }
            else
            {
                return new List<String>();
            }
        }

        public static Boolean IsSourceItemExists(String url)
        {
            return DBData.GetInstance(DBTable.resource_items).GetCount($"url='{url}'") > 0;
        }

        /// <summary>
        /// 保存资源详情
        /// </summary>
        /// <param name="url"></param>
        /// <param name="domain"></param>
        /// <param name="title"></param>
        public static void SaveSourceItem(String url, String domain, String title)
        {
            DBData.GetInstance(DBTable.resource_items).Add(new resource_items()
            {
                url = url,
                domain = domain,
                title = title
            });
        }

        /// <summary>
        /// 保存资源的列表信息
        /// </summary>
        /// <param name="host"></param>
        /// <param name="listUrlFormat"></param>
        public static void SaveListFormat(String host, String listUrlFormat)
        {
            DBData.GetInstance(DBTable.p_list_format).Add(new p_list_format()
            {
                domain = host,
                url_format = listUrlFormat,
            });
        }

        /// <summary>
        /// 检测是否已经有了该站点的列表数据
        /// </summary>
        /// <param name="host">主域名</param>
        /// <returns></returns>
        public static Boolean IsListFormatExistsWithHost(String host)
        {
            String sql = String.Format("SELECT COUNT(1) FROM p_list_format WHERE domain='{0}'", host);
            return DBData.ExecuteScalarIntBySql(sql) > 0;
        }

        /// <summary>
        /// 检测是否已经有了该站点的列表数据
        /// </summary>
        /// <param name="urlFormat">完整链接</param>
        /// <returns></returns>
        public static Boolean IsListFormatExistsWithUrl(String urlFormat)
        {
            String sql = String.Format("SELECT COUNT(1) FROM p_list_format WHERE url_format='{0}'", urlFormat);
            return DBData.ExecuteScalarIntBySql(sql) > 0;
        }

        /// <summary>
        /// 是否是不需要检测数据列表页
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static Boolean IsForbiddenDomain(String host)
        {
            return DBData.GetInstance(DBTable.p_list_forbid).GetCount($"domain='{host}'") > 0;
        }

        public static void AddToForbiddenDomain(String host)
        {
            DBData.GetInstance(DBTable.p_list_forbid).Add(new p_list_forbid()
            {
                domain = host
            });
        }

        /// <summary>
        /// 检测当天是否已经抓取过该分页地址
        /// </summary>
        /// <param name="crdate"></param>
        /// <param name="url_format"></param>
        /// <returns></returns>
        public static Boolean IsRequestedListExists(DateTime crdate, String url_format)
        {
            return DBData.GetInstance(DBTable.p_requested_list).GetCount($"crdate='{crdate:yyyy-MM-dd}' and url_format='{url_format}'") > 0;
        }

        /// <summary>
        /// 保存当天已抓取过的分页链接地址
        /// </summary>
        /// <param name="crdate"></param>
        /// <param name="url_format"></param>
        public static void SaveRequestedList(DateTime crdate, String url_format)
        {
            DBData.GetInstance(DBTable.p_requested_list).Add(new p_requested_list()
            {
                crdate = crdate,
                url_format = url_format
            });
        }

        /// <summary>
        /// 获取已有的分页链接
        /// </summary>
        /// <returns></returns>
        public static IList<p_list_format> GetFormatList()
        {
            return DBData.GetInstance(DBTable.p_list_format).GetList<p_list_format>();
        }

    }
}
