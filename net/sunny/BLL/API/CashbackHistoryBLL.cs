using Sunny.DAL;
using Sunny.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.BLL.API
{

    /// <summary>
    /// 邀请返现
    /// </summary>
    public class CashbackHistoryBLL
    {
        /// <summary>
        /// 获取邀请汇总信息及每天的邀请收益信息
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public static CashbackHistoryJson GetCashbackData(int studentId)
        {
            List<CashbackHistoryItem> items = CashbackHistoryDAL.GetCashbackHistoryList(studentId);
            decimal totalMoney = CashbackHistoryDAL.GetTotalBackMoney(studentId);
            int totalCount = InvitationDAL.GetInvitatedCount(studentId);

            CashbackHistoryJson result = new CashbackHistoryJson()
            {
                total_money = totalMoney,
                total_count = totalCount,
                items = items,
            };

            return result;
        }
    }
}
