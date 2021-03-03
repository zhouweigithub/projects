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
        private const String getDataList = @"
SELECT * FROM(
    SELECT a.id,a.url,IFNULL(b.zan,0)zan,IFNULL(b.cai,0)cai,IFNULL(c.connect_time,30)connect_time FROM url a
    LEFT JOIN url_attention b ON a.id=b.url_id
    LEFT JOIN (SELECT * FROM(SELECT * FROM url_connect_time ORDER BY crdate DESC)t GROUP BY url_id) c ON a.id=c.url_id
)t 
ORDER BY connect_time ASC,zan DESC,id ASC";

        /// <summary>
        /// 删除5天内累计访问超过150秒的项，每天默认访问最长时间为30秒
        /// </summary>
        private const String deleteTimeOutLinks = @"
DELETE FROM url_connect_time WHERE url_id IN(
	SELECT DISTINCT url_id FROM(
        -- b表中的每一项在a表中都能找到比其crdate小的项
		SELECT url_id,SUM(connect_time)connect_timeSum FROM url_connect_time a WHERE (SELECT COUNT(1) FROM url_connect_time b WHERE a.url_id=b.url_id AND b.crdate>a.crdate)<5 GROUP BY url_id HAVING connect_timeSum>=150
	)t
)";

        /// <summary>
        /// 取所有URL列表
        /// </summary>
        /// <returns></returns>
        public static List<url_data> GetList()
        {
            return DBData.GetDataListBySql<url_data>(getDataList);
        }

        public static urls GetEntity(String url)
        {
            return DBData.GetInstance(DBTable.url).GetEntity<urls>($"url='{url}'");
        }

        /// <summary>
        /// 点赞
        /// </summary>
        /// <param name="urlId"></param>
        /// <returns></returns>
        public static Boolean Zan(Int32 urlId, String ip)
        {
            String type = "zan";
            Boolean isExists = DBData.GetInstance(DBTable.url_attention_ip).GetCount($"url_id='{urlId}' and type='{type}' and ip='{ip}'") > 0;

            if (isExists)
            {
                return false;
            }

            var db = DBData.GetInstance(DBTable.url_attention);
            var model = db.GetEntityByKey<url_attention>(urlId);
            DBData.GetInstance(DBTable.url_attention_ip).Add(new url_attention_ip()
            {
                url_id = urlId,
                type = type,
                ip = ip
            });

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
        public static Boolean Cai(Int32 urlId, String ip)
        {
            String type = "cai";
            Boolean isExists = DBData.GetInstance(DBTable.url_attention_ip).GetCount($"url_id='{urlId}' and type='{type}' and ip='{ip}'") > 0;

            if (isExists)
            {
                return false;
            }

            var db = DBData.GetInstance(DBTable.url_attention);
            var model = db.GetEntityByKey<url_attention>(urlId);
            DBData.GetInstance(DBTable.url_attention_ip).Add(new url_attention_ip()
            {
                url_id = urlId,
                type = type,
                ip = ip
            });

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
            url = CheckInput(url);
            var db = DBData.GetInstance(DBTable.url);

            return db.Add(new urls()
            {
                url = url,
                status = 0,
                createIp = ip
            }) > 0;
        }

        public static void UpdaateTime(Int32 urlid, Int32 times)
        {
            DBData.GetInstance(DBTable.url_connect_time).Add(new url_connect_time()
            {
                url_id = urlid,
                connect_time = times,
                crdate = DateTime.Today
            });
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

        private static String CheckInput(String input)
        {
            List<String> forbidenString = new List<String>() { "script", "<", ">", "--" };

            foreach (String item in forbidenString)
            {
                input = input.Replace(item, String.Empty);
            }

            if (input.Length > 150)
            {
                input = input.Substring(0, 150);
            }

            return input;
        }

        /// <summary>
        /// 删除访问时间太慢的项
        /// </summary>
        public static Int32 DeleteTimeOutLinks()
        {
            return DBData.ExecuteNonQueryBySql(deleteTimeOutLinks);
        }
    }
}
