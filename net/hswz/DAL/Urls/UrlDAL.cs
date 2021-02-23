using System;
using System.Collections.Generic;
using Hswz.Model.Urls;

namespace Hswz.DAL
{
    /// <summary>
    /// 域名数据
    /// </summary>
    public class UrlDAL
    {
        private const String getDataList = @"SELECT a.id,a.url,IFNULL(b.zan,0)zan,IFNULL(b.cai,0)cai,IFNULL(c.connect_time,0)connect_time FROM url a
LEFT JOIN url_attention b ON a.id=b.url_id
LEFT JOIN (SELECT * FROM(SELECT * FROM url_connect_time ORDER BY crdate DESC)t GROUP BY url_id,crdate ) c ON a.id=c.url_id";

        /// <summary>
        /// 取所有URL列表
        /// </summary>
        /// <returns></returns>
        public static List<url_data> GetList()
        {
            return DBData.GetDataListBySql<url_data>(getDataList);
        }

        /// <summary>
        /// 点赞
        /// </summary>
        /// <param name="urlId"></param>
        /// <returns></returns>
        public static Boolean Zan(Int32 urlId)
        {
            var db = DBData.GetInstance(DBTable.url_attention);
            var model = db.GetEntityByKey<url_attention>(urlId);

            if (model == null)
            {   //添加数据
                return db.Add(new url_attention()
                {
                    url_id = urlId,
                    zan = 1,
                    cai = 0
                }) > 0;
            }
            else
            {   //修改数据
                model.zan = model.zan + 1;
                return db.UpdateByKey(model, model.url_id) > 0;
            }
        }

        /// <summary>
        /// 点踩
        /// </summary>
        /// <param name="urlId"></param>
        /// <returns></returns>
        public static Boolean Cai(Int32 urlId)
        {
            var db = DBData.GetInstance(DBTable.url_attention);
            var model = db.GetEntityByKey<url_attention>(urlId);

            if (model == null)
            {   //添加数据
                return db.Add(new url_attention()
                {
                    url_id = urlId,
                    zan = 0,
                    cai = 1
                }) > 0;
            }
            else
            {   //修改数据
                model.cai = model.cai + 1;
                return db.UpdateByKey(model, model.url_id) > 0;
            }
        }

        /// <summary>
        /// 添加域名
        /// </summary>
        /// <param name="url">域名</param>
        /// <param name="ip">添加者的IP地址</param>
        /// <returns></returns>
        public static Boolean Add(String url, String ip)
        {
            var db = DBData.GetInstance(DBTable.url);

            return db.Add(new urls()
            {
                url = url,
                status = 0,
                createIp = ip
            }) > 0;
        }

        /// <summary>
        /// 添加连接时长数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static Boolean AddConnectTime(url_connect_time model)
        {
            var db = DBData.GetInstance(DBTable.url_connect_time);

            return db.Add(model) > 0;
        }

    }
}
