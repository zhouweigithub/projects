using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Hswz.DAL;
using Hswz.Model.Urls;

namespace Web.Controllers
{
    public class UrlController : Controller
    {
        public String List()
        {
            var datas = UrlDAL.GetList();

            if (!IsTodayTested())
            {
                Task.Factory.StartNew(() => ReTest(datas));
            }

            return Util.Json.JsonUtil.Serialize(datas);
        }

        public String Zan(Int32 urlId)
        {
            String ip = GetRequestIP();
            Boolean isOk = UrlDAL.Zan(urlId, ip);
            return Util.Json.JsonUtil.Serialize(new
            {
                code = 0,
                msg = "ok",
                data = isOk,
            });
        }

        public String Cai(Int32 urlId)
        {
            String ip = GetRequestIP();
            Boolean isOk = UrlDAL.Cai(urlId, ip);
            return Util.Json.JsonUtil.Serialize(new
            {
                code = 0,
                msg = "ok",
                data = isOk,
            });
        }

        public String Add(String url)
        {
            url = url.ToLower();
            String ip = GetRequestIP();
            Boolean isOk = UrlDAL.Add(url, ip);
            url_data model = null;
            if (isOk)
            {
                var urlInfo = UrlDAL.GetEntity(url);
                model = new url_data()
                {
                    id = urlInfo.id,
                    url = url,
                    cai = 0,
                    zan = 0,
                    connect_time = 0,
                };
            }
            return Util.Json.JsonUtil.Serialize(new
            {
                code = 0,
                msg = "ok",
                data = model,
            });
        }

        /// <summary>
        /// 测试站点反应时间
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public String Test(Int32 urlid)
        {
            var urlInfo = DBData.GetInstance(DBTable.url).GetEntityByKey<urls>(urlid);

            Int32 time = TestConnect(urlid, urlInfo.url);

            return Util.Json.JsonUtil.Serialize(new
            {
                data = new url_data() { connect_time = time }.percent,
            });
        }

        private Int32 TestConnect(Int32 urlid, String url)
        {
            var dtStart = DateTime.Now;
            //测试访问地址
            Boolean isOk = getUrl(url);
            var dtEnd = DateTime.Now;

            Int32 time = 0;
            if (isOk)
            {
                time = (Int32)dtEnd.Subtract(dtStart).TotalSeconds;
                if (time == 0)
                {
                    time = 1;
                }
            }
            if (time == 0)
            {
                time = 30;
            }

            DBData.GetInstance(DBTable.url_connect_time).Add(new url_connect_time()
            {
                url_id = urlid,
                crdate = DateTime.Today,
                connect_time = time
            });

            return time;
        }

        private Boolean getUrl(String url)
        {
            try
            {
                if (!url.StartsWith("http://") && !url.StartsWith("https://"))
                {
                    url = "http://" + url;
                }
                String userAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4324.190 Safari/537.36";

                Util.Web.WebUtil.GetWebData(url, "", Util.Web.DataCompress.NotCompress, timeout: 30 * 1000, userAgent: userAgent);

                return true;
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write(e.Message, Util.Log.LogType.Error);
            }
            return false;
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

        private Boolean IsTodayTested()
        {
            String today = DateTime.Today.ToString("yyyy-MM-dd");
            Int32 count = DBData.GetInstance(DBTable.url_connect_time).GetCount($"crdate='{today}'");
            return count > 0;
        }

        private void ReTest(List<url_data> datas)
        {
            Util.Log.LogUtil.Write("开始测试各个网址访问时长", Util.Log.LogType.Debug);

            Int32 maxThreadCount = datas.Count / 10;
            if (maxThreadCount == 0)
            {
                maxThreadCount = 1;
            }
            if (maxThreadCount > 10)
            {
                maxThreadCount = 10;
            }

            Util.Log.LogUtil.Write($"启动线程数量：{maxThreadCount}", Util.Log.LogType.Debug);

            //全部测试一遍
            Hswz.Common.ThreadHelper.Threading(datas, maxThreadCount, 5,
                (item) =>
                {
                    TestConnect(item.id, item.url);
                });

            Util.Log.LogUtil.Write("测试结束", Util.Log.LogType.Debug);
        }
    }
}