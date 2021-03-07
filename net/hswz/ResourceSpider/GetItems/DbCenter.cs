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

    }
}
