using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Spetmall.BLL.Page;
using Spetmall.DAL;
using Spetmall.Model;

namespace Moqikaka.Tmp.Admin.Controllers
{
    public class RailcardController : Controller
    {

        public ActionResult Index(string keyWord, string orderBy)
        {
            List<railcard> datas = railcardDAL.GetInstance().GetRailcards(keyWord, orderBy);
            ViewBag.keyWord = keyWord;
            ViewBag.datas = datas;
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
                //更新railcard的剩余次数
                railcard server = railcardDAL.GetInstance().GetEntityByKey<railcard>(railcard_record.railcardid);

                if (server == null)
                    errMsg = "无效优惠卡";
                else if (railcard_record.times == 0)
                    errMsg = "次数不能为0";
                else if (server.lefttimes < railcard_record.times)
                    errMsg = "次数不足";
                else
                {
                    server.lefttimes = server.lefttimes - railcard_record.times;
                    status = railcardDAL.GetInstance().UpdateByKey(server, railcard_record.railcardid) > 0;

                    //添加优惠卡的使用记录
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
