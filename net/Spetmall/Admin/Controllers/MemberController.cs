using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Spetmall.BLL.Page;
using Spetmall.DAL;
using Spetmall.Model;
using Spetmall.Model.Page;

namespace Spetmall.Admin.Controllers
{
    [Common.CustomAuthorize]
    public class MemberController : Controller
    {

        public ActionResult Index(string keyWord, string orderBy)
        {
            List<member_show> datas = memberDAL.GetInstance().GetMembers(keyWord, orderBy);
            ViewBag.keyWord = keyWord;
            ViewBag.orderBy = orderBy;
            ViewBag.datas = datas;
            return View();
        }

        public ActionResult Edit(int id)
        {
            member_edit member = null;
            if (id == 0)
            {   //添加
                member = new member_edit();
            }
            else
            {   //编辑
                member = memberDAL.GetInstance().GetEditInfo(id);
            }

            //宠物类型
            IList<pet> petTypes = petDAL.GetInstance().GetList<pet>("state=0");
            ViewBag.smallDogs = petTypes.Where(a => a.type == 0);
            ViewBag.midDogs = petTypes.Where(a => a.type == 1);
            ViewBag.largeDogs = petTypes.Where(a => a.type == 2);
            ViewBag.cats = petTypes.Where(a => a.type == 3);
            ViewBag.others = petTypes.Where(a => a.type == 4);

            return View(member);
        }

        [HttpPost]
        public ActionResult Edit(member_edit member)
        {
            bool status = false;
            string errMsg = string.Empty;
            try
            {
                member.name = member.name.Trim();
                member.phone = member.phone.Trim();
                member.email = member.email.Trim();
                member.remark = member.remark.Trim();
                if (member.id == 0)
                {
                    member.money = 0;
                    status = memberDAL.GetInstance().Add<member>(member) > 0;
                    //添加会员时，插入一条充值记录
                    //rechargeDAL.GetInstance().Add<recharge>(new recharge()
                    //{
                    //    sno = GetSno(),
                    //    memberid = member.id,
                    //    money = member.money,
                    //    remark = "添加会员时充值",
                    //    balance = member.money,
                    //});
                }
                else
                {
                    status = memberDAL.GetInstance().UpdateByKey<member>(member, member.id) > 0;
                }

                //添加喜爱的宠物
                if (member?.petList.Count > 0)
                {
                    memberPetDAL.GetInstance().Delete($"memberid={member.id}");
                    foreach (string pet in member.petList)
                    {
                        memberPetDAL.GetInstance().Add(new memberpet()
                        {
                            memberid = member.id,
                            petid = int.Parse(pet),
                        });
                    }
                }
            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }
            return Content(CommonBLL.GetReturnJson(status, errMsg));
        }

        public ActionResult Recharge(int memberid)
        {
            member member = null;
            if (memberid > 0)
            {   //充值
                member = memberDAL.GetInstance().GetEntityByKey<member>(memberid);
            }

            ViewBag.MemberId = member.id;
            ViewBag.Name = member.name;
            return View();
        }

        [HttpPost]
        public ActionResult Recharge(int memberid, decimal money, decimal payMoney, string remark)
        {
            bool status = false;
            string errMsg = string.Empty;
            if (memberid > 0 && money > 0)
            {   //充值
                try
                {
                    member member = memberDAL.GetInstance().GetEntityByKey<member>(memberid);
                    decimal balance = member.money += money;
                    //更新会员余额
                    Dictionary<string, object> keyvalue = new Dictionary<string, object>() { { "money", balance } };
                    memberDAL.GetInstance().UpdateByKey(keyvalue, memberid);
                    //添加充值记录
                    rechargeDAL.GetInstance().Add(new recharge()
                    {
                        sno = GetSno(),
                        memberid = memberid,
                        money = money,
                        paymoney = payMoney,
                        remark = remark,
                        balance = balance,
                    });
                    status = true;
                }
                catch (Exception e)
                {
                    errMsg = "服务器异常";
                    Util.Log.LogUtil.Write($"充值失败：memberid {memberid} money {money} remark {remark} \r\n" + e, Util.Log.LogType.Error);
                }
            }
            return Content(CommonBLL.GetReturnJson(status, errMsg));
        }

        //充值记录
        public ActionResult RechargeRecord(int memberid)
        {
            string where = memberid > 0 ? $"memberid={memberid}" : "1=1";
            IList<recharge> rechargeList = rechargeDAL.GetInstance().GetList<recharge>(where);
            ViewBag.rechargeList = rechargeList;
            return View();
        }



        /// <summary>
        /// 生成充值的流水号
        /// </summary>
        /// <returns></returns>
        private string GetSno()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssffff");
        }
    }
}
