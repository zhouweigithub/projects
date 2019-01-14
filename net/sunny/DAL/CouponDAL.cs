using MySql.Data.MySqlClient;
using Sunny.Model.Custom;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.DAL
{
    /// <summary>
    /// 优惠券信息操作类
    /// </summary>
    public class CouponDAL
    {

        /// <summary>
        /// 获取优惠券信息
        /// </summary>
        private static readonly string getCouponInfoListSql = @"
SELECT a.id,b.name,b.money,b.start_time,b.end_time,IF(a.state=1,'已使用',IF(b.end_time<NOW(),'已过期','未使用'))`status` FROM student_coupon a
INNER JOIN coupon b ON a.coupon_id=b.id
LEFT JOIN category c ON b.category_id=c.id OR b.category_id=0
WHERE b.state=0 AND c.type=0 AND a.student_id='{0}'
 ";

        /// <summary>
        /// 获取当前订单可用的默认优惠券
        /// </summary>
        private static readonly string getCouponDefaultOfStudentSql = @"
SELECT b.id,b.name,b.money FROM student_coupon a
INNER JOIN coupon b ON a.coupon_id=b.id
WHERE b.category_id='{0}' AND a.student_id='{1}'
ORDER BY b.money DESC
LIMIT 1
";

        /// <summary>
        /// 获取优惠券信息
        /// </summary>
        /// <param name="studentId">学员id</param>
        /// <returns></returns>
        public static List<CouponListJson> GetCouponList(int studentId)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    DataTable dt = dbhelper.ExecuteDataTable(string.Format(getCouponInfoListSql, studentId));

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.ToList<CouponListJson>();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("GetCouponList 出错：" + ex, Util.Log.LogType.Error);
            }

            return new List<CouponListJson>();
        }

        /// <summary>
        /// 获取当前订单可用的默认优惠券
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public static CouponListJson GetCouponDefaultOfStudent(int studentId, int categoryId)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    DataTable dt = dbhelper.ExecuteDataTable(string.Format(getCouponDefaultOfStudentSql, categoryId, studentId));

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.ToList<CouponListJson>().First();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("GetCouponDefaultOfStudent 出错：" + ex, Util.Log.LogType.Error);
            }

            return null;
        }


    }
}
