using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Spetmall.BLL.Page;
using Spetmall.DAL;
using Spetmall.Model;

namespace Spetmall.Admin.Controllers
{
    [Common.CustomAuthorize]
    public class RailcardController : Controller
    {

        public ActionResult Index(string keyWord, string orderBy, int pageSize = 20, int page = 1)
        {
            List<railcard> datas = railcardDAL.GetInstance().GetRailcards(keyWord, orderBy);
            int count = railcardDAL.GetInstance().GetRailcardsCount(keyWord);

            ViewBag.keyWord = keyWord;
            ViewBag.orderBy = orderBy;
            ViewBag.datas = datas;

            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalDataCount = count;

            return View();
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(railcard railcard)
        {
            bool status = false;
            string errMsg = string.Empty;
            try
            {
                railcard.name = railcard.name.Trim();
                railcard.phone = railcard.phone.Trim();
                railcard.remark = railcard.remark.Trim();
                railcard.remark = railcard.remark.Trim();
                railcard.py = Spetmall.Common.ChineseSpell.GetChineseSpell(railcard.petname);
                status = railcardDAL.GetInstance().Add(railcard) > 0;
            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }
            return Content(CommonBLL.GetReturnJson(status, errMsg));
        }

        public ActionResult Edit(int id)
        {
            railcard railcard = railcardDAL.GetInstance().GetEntityByKey<railcard>(id);
            ViewBag.railcard = railcard;
            ViewBag.railcardid = id;
            return View();
        }

        [HttpPost]
        public ActionResult Edit(railcard_record railcard_record)
        {
            bool status = false;
            string errMsg = string.Empty;
            try
            {
                railcard server = railcardDAL.GetInstance().GetEntityByKey<railcard>(railcard_record.railcardid);

                if (server == null)
                    errMsg = "无效洗澡卡";
                else if (railcard_record.times == 0)
                    errMsg = "次数不能为0";
                else if (server.lefttimes < railcard_record.times)
                    errMsg = "次数不足";
                else if (server.starttime > DateTime.Now)
                    errMsg = "未到开始日期 " + server.starttime.ToString("yyyy-MM-dd");
                else if (server.endtime < DateTime.Now)
                    errMsg = "已过截止日期 " + server.endtime.ToString("yyyy-MM-dd");
                else
                {
                    //更新railcard的剩余次数
                    server.lefttimes = server.lefttimes - railcard_record.times;
                    status = railcardDAL.GetInstance().UpdateByKey(server, railcard_record.railcardid) > 0;

                    //添加洗澡卡的使用记录
                    railcardRecordDAL.GetInstance().Add(new railcard_record()
                    {
                        railcardid = railcard_record.railcardid,
                        times = railcard_record.times,
                        lefttimes = server.lefttimes,
                        remark = railcard_record.remark,
                    });
                }
            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }
            return Content(CommonBLL.GetReturnJson(status, errMsg));
        }

        public ActionResult Record(int id, string keyWord)
        {
            List<railcard_record> datas = railcardRecordDAL.GetInstance().GetRailcardRecords(id, keyWord);
            ViewBag.datas = datas;
            return View();
        }

        public ActionResult Delete(int id)
        {
            bool status = false;
            string errMsg = string.Empty;
            try
            {
                //更新railcard的剩余次数
                status = railcardDAL.GetInstance().DeleteByKey(id) > 0;
            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }

            return Json(new
            {
                status,
                msg = errMsg,
            }, JsonRequestBehavior.AllowGet);
        }


    }
}
