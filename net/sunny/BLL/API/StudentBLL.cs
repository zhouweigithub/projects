using Sunny.Common;
using Sunny.DAL;
using Sunny.Model;
using Sunny.Model.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.BLL.API
{
    public class StudentBLL
    {

        /// <summary>
        /// 发放注册返现金额
        /// </summary>
        /// <param name="studentid">学生id</param>
        /// <returns></returns>
        public static bool SendRegisterBackMomey(int studentid)
        {
            try
            {
                SiteInfo info = DBData.GetInstance(DBTable.site_info).GetEntityByKey<SiteInfo>(Const.RegisterBackMoney);
                decimal.TryParse(info == null ? "0" : info.pvalue, out decimal backMoney);

                if (backMoney <= 0)
                    return false;

                //账户返现
                StudentDAL.AddCash(studentid, backMoney);
                Student student = DBData.GetInstance(DBTable.student).GetEntityByKey<Student>(studentid);
                decimal balance = student.cash;

                //添加余额变动记录
                DBData.GetInstance(DBTable.pay_record).Add(new PayRecord()
                {
                    user_id = studentid,
                    money = backMoney,
                    type = 5,
                    order_id = string.Empty,
                    user_type = 0,
                    comment = "注册返现",
                    balance = balance,
                });

                return true;
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"SendRegisterBackMomey 发放注册返现金额操作出错：studentid={studentid} \r\n {e}", Util.Log.LogType.Error);
            }

            return false;
        }

        /// <summary>
        /// 发放邀请奖励
        /// </summary>
        /// <param name="studentid"></param>
        /// <returns></returns>
        public static bool SendInvitationBackMomey(int studentid)
        {
            try
            {
                CustInvationUserInfo invationInfo = InvitationDAL.GetInvitationUsers(studentid);
                if (invationInfo == null)
                    return false;

                IList<SiteInfo> infos = DBData.GetInstance(DBTable.site_info).GetList<SiteInfo>();
                SiteInfo backInfo = infos.FirstOrDefault(a => a.pkey == Const.BackCachMoney);
                SiteInfo secondBackInfo = infos.FirstOrDefault(a => a.pkey == Const.SecondBackCachPercent);

                decimal.TryParse(backInfo == null ? "0" : backInfo.pvalue, out decimal backMoney);
                decimal.TryParse(secondBackInfo == null ? "0" : secondBackInfo.pvalue, out decimal secondBackPercent);

                if (backMoney <= 0)
                    return false;

                decimal money1 = backMoney;
                decimal money2 = Math.Round(invationInfo.from2 == 0 ? 0 : backMoney * secondBackPercent, 2);
                if (money1 > 0)
                {
                    //账户返现
                    StudentDAL.AddCash(invationInfo.from1, money1);
                    Student student = DBData.GetInstance(DBTable.student).GetEntityByKey<Student>(invationInfo.from1);
                    decimal balance = student.cash;

                    //添加余额变动记录
                    DBData.GetInstance(DBTable.pay_record).Add(new PayRecord()
                    {
                        user_id = invationInfo.from1,
                        money = money1,
                        type = 1,
                        order_id = string.Empty,
                        user_type = 0,
                        comment = "邀请返现",
                        balance = balance,
                    });

                    //添加邀请返现记录
                    DBData.GetInstance(DBTable.cashback_history).Add(new CashbackHistory()
                    {
                        student_id = invationInfo.from1,
                        from_student_id = studentid,
                        money = money1,
                    });

                    //更新邀请奖励发放状态
                    Dictionary<string, object> fieldValue = new Dictionary<string, object>();
                    fieldValue.Add("state", 1);
                    DBData.GetInstance(DBTable.invitation).UpdateByKey(fieldValue, studentid);
                }
                if (money2 > 0)
                {
                    //账户返现
                    StudentDAL.AddCash(invationInfo.from2, money2);
                    Student student = DBData.GetInstance(DBTable.student).GetEntityByKey<Student>(invationInfo.from2);
                    decimal balance = student.cash;

                    //添加余额变动记录
                    DBData.GetInstance(DBTable.pay_record).Add(new PayRecord()
                    {
                        user_id = invationInfo.from2,
                        money = money2,
                        type = 1,
                        order_id = string.Empty,
                        user_type = 0,
                        comment = "间接邀请返现",
                        balance = balance,
                    });

                    //添加邀请返现记录
                    DBData.GetInstance(DBTable.cashback_history).Add(new CashbackHistory()
                    {
                        student_id = invationInfo.from2,
                        from_student_id = studentid,
                        money = money2,
                    });
                }


                return true;
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"SendInvitationBackMomey 发放邀请奖励操作出错：studentid={studentid} \r\n {e}", Util.Log.LogType.Error);
            }

            return false;
        }

    }
}
