using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.DAL
{
    public class CoachDAL
    {
        private static readonly string addCashSql = "UPDATE coach SET cash=cash+{0} WHERE id={1}";

        private static readonly string isCaptionPhoneExistSql = "SELECT COUNT(1) FROM coachcaption_venue a INNER JOIN coach b ON a.coach_id=b.id WHERE b.phone= '{0}' AND b.state= 0";

        /// <summary>
        /// 添加余额
        /// </summary>
        /// <param name="coachId">教练id</param>
        /// <param name="cash">添加的金额，可为负</param>
        /// <returns></returns>
        public static bool AddCash(int coachId, decimal cash)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    int count = dbhelper.ExecuteNonQueryParams(string.Format(addCashSql, cash, coachId));
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("AddCash 出错：" + ex, Util.Log.LogType.Error);
            }

            return false;
        }

        /// <summary>
        /// 检测教练队长是否存在
        /// </summary>
        /// <param name="phone">教练队长的手机号</param>
        /// <returns></returns>
        public static bool IsCaptionPhoneExist(string phone)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    int count = dbhelper.ExecuteNonQueryParams(string.Format(isCaptionPhoneExistSql, phone));
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("UpdateCash 出错：" + ex, Util.Log.LogType.Error);
            }

            return false;
        }

    }
}
