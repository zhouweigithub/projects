using Sunny.Common;
using Sunny.DAL;
using Sunny.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.BLL.API
{
    public class WithdrawBLL
    {
        /// <summary>
        /// 提现
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <param name="money">提现金额</param>
        /// <param name="type">0学员1教练</param>
        public static bool Withdraw(int userid, decimal money, UserType user_type)
        {
            try
            {
                if (money == 0)
                    return false;

                decimal balance = 0;
                if (user_type == UserType.Student)
                {
                    StudentDAL.AddCash(userid, -money);
                    Student student = DBData.GetInstance(DBTable.student).GetEntityByKey<Student>(userid);
                    balance = student.cash;
                }
                else
                {
                    CoachDAL.AddCash(userid, -money);
                    Coach coach = DBData.GetInstance(DBTable.coach).GetEntityByKey<Coach>(userid);
                    balance = coach.cash;
                }
                DBData.GetInstance(DBTable.pay_record).Add(new PayRecord()
                {
                    user_id = userid,
                    money = -money,
                    type = 2,
                    order_id = string.Empty,
                    user_type = (short)user_type,
                    comment = "提现",
                    balance = balance,
                });

                return true;
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"Withdraw 提现操作出错：userid={userid} money={money} user_type={user_type} \r\n {e}", Util.Log.LogType.Error);
            }

            return false;
        }
    }
}
