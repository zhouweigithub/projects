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
                Util.Log.LogUtil.Write("UpdateCash 出错：" + ex, Util.Log.LogType.Error);
            }

            return false;
        }

    }
}
