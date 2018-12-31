using Moqikaka.Tmp.DAL;
using Moqikaka.Tmp.Model;
using Moqikaka.Util.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Moqikaka.Tmp.Admin.Controllers.API
{

    public class ArtiDataController : Controller
    {

        public ActionResult GetArtical(int id)
        {
            Model.MArtical result = null;

            if (id > 0)
                result = DBData.GetInstance(DBTable.m_artical).GetEntityByKey<Model.MArtical>(id);

            if (result == null)
                result = new Model.MArtical();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetArticalListByYear(int year)
        {
            IList<Model.MArticalBase> result = null;

            if (year > 0)
                result = MArticalDAL.GetArticals(string.Empty, year, string.Empty, 1, int.MaxValue);

            if (result == null)
                result = new List<Model.MArticalBase>();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetArticalListByArea(string area)
        {
            IList<Model.MArticalBase> result = null;

            if (!string.IsNullOrWhiteSpace(area))
                result = MArticalDAL.GetArticals(string.Empty, 0, area, 1, int.MaxValue);

            if (result == null)
                result = new List<Model.MArticalBase>();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetArticalListByKeyWord(string keyword)
        {
            IList<Model.MArticalBase> result = null;

            if (!string.IsNullOrWhiteSpace(keyword))
                result = MArticalDAL.GetArticals(keyword, 0, string.Empty, 1, int.MaxValue);

            if (result == null)
                result = new List<Model.MArticalBase>();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetArticalListByRandom()
        {
            IList<Model.MArticalBase> result = MArticalDAL.GetArticalsRandom();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void LoginPostBack(string code, string name)
        {
            List<MiniProgromSetting> settings = BLL.Page.CommonBLL.GetMiniProgromSettingFromFile();
            MiniProgromSetting setting = settings.FirstOrDefault(a => a.Name == name);

            string url = $"https://api.weixin.qq.com/sns/jscode2session?appid={setting.Appid}&secret={setting.Secret}&js_code={code}&grant_type=authorization_code";
            string result = Tmp.Common.HttpHelper.GetHtml(url, null, "Get", string.Empty, out string cookie);
            LogUtil.Write("openid信息：" + result, LogType.Debug);
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
