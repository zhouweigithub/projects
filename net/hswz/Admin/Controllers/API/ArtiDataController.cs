using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Hswz.DAL;
using Hswz.Model;
using Hswz.Model.wx;
using Util.Log;

namespace Hswz.Admin.Controllers.API
{

    public class ArtiDataController : Controller
    {

        public ActionResult GetArtical(Int32 id)
        {
            Model.MArtical result = null;

            if (id > 0)
            {
                result = DBData.GetInstance(DBTable.m_artical).GetEntityByKey<Model.MArtical>(id);
            }

            if (result == null)
            {
                result = new Model.MArtical();
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetArticalListByYear(Int32 year)
        {
            IList<Model.MArticalBase> result = null;

            if (year > 0)
            {
                result = MArticalDAL.GetArticals(String.Empty, year, String.Empty, 1, Int32.MaxValue);
            }

            if (result == null)
            {
                result = new List<Model.MArticalBase>();
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetArticalListByArea(String area)
        {
            IList<Model.MArticalBase> result = null;

            if (!String.IsNullOrWhiteSpace(area))
            {
                result = MArticalDAL.GetArticals(String.Empty, 0, area, 1, Int32.MaxValue);
            }

            if (result == null)
            {
                result = new List<Model.MArticalBase>();
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetArticalListByKeyWord(String keyword)
        {
            IList<Model.MArticalBase> result = null;

            if (!String.IsNullOrWhiteSpace(keyword))
            {
                result = MArticalDAL.GetArticals(keyword, 0, String.Empty, 1, Int32.MaxValue);
            }

            if (result == null)
            {
                result = new List<Model.MArticalBase>();
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetArticalListByRandom()
        {
            IList<Model.MArticalBase> result = MArticalDAL.GetArticalsRandom();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void LoginPostBack(String deviceid, String code, String name)
        {
            try
            {
                var settings = BLL.Page.CommonBLL.GetMiniProgromSettingFromFile();
                var setting = settings.FirstOrDefault(a => a.Name == name);

                String url = $"https://api.weixin.qq.com/sns/jscode2session?appid={setting.Appid}&secret={setting.Secret}&js_code={code}&grant_type=authorization_code";
                String result = Util.Web.WebUtil.GetWebData(url, String.Empty, Util.Web.DataCompress.NotCompress);

                if (!String.IsNullOrWhiteSpace(result))
                {
                    var loginResult = Util.Json.JsonUtil.Deserialize<WXLoginResult>(result);
                    if (loginResult != null)
                    {
                        DBData.GetInstance(DBTable.m_device_openid).Add(new MDeviceOpenid()
                        {
                            DeviceToken = deviceid,
                            Openid = loginResult.openid,
                        });
                    }
                }
            }
            catch (Exception)
            {
                LogUtil.Write($"获取微信openid出错:deviceid:{deviceid} code:{code} name:{name}", LogType.Error);
            }
        }

        [HttpPost]
        public void AddHistory(MHistory model)
        {
            DBData.GetInstance(DBTable.m_history).Add(model);
        }

        [HttpPost]
        public void AddFavorite(MFavorite model)
        {
            DBData.GetInstance(DBTable.m_favorite).Add(model);
        }

        [HttpPost]
        public void DeleteFavorite(MFavorite model)
        {
            DBData.GetInstance(DBTable.m_favorite).Delete($"DeviceToken='{model.DeviceToken}' and Articalid='{model.Articalid}'");
        }

        [HttpPost]
        public void AddMUserLoginLog(MUserLoginLog model)
        {
            DBData.GetInstance(DBTable.m_user_login_log).Add(model);
        }

        [HttpPost]
        public void AddPageClickLog(MPageClickLog model)
        {
            ArtiRequestDAL.AddPageClickLog(model);
        }

        [HttpPost]
        public void AddArticalClickLog(MArticalClickLog model)
        {
            ArtiRequestDAL.AddArticalClickLog(model);
        }

        [HttpPost]
        public void AddArticalShareLog(MShare model)
        {
            DBData.GetInstance(DBTable.m_share).Add(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public void AddFeedback(MFeedback model)
        {
            ArtiRequestDAL.AddFeedback(model);
        }

    }
}
