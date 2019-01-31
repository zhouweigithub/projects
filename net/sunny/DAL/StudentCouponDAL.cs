using Sunny.Model;
using Sunny.Model.Custom;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.DAL
{
    public class StudentCouponDAL
    {

        private static readonly string getStudentCouponSql = @"
SELECT b.id,b.money,b.name,a.count,b.category_id,b.multiple,b.start_time,b.end_time FROM student_coupon a
INNER JOIN coupon b ON a.coupon_id=b.id
INNER JOIN category d ON b.category_id=d.id
INNER JOIN product e ON d.id=e.category_id
WHERE a.state=0 AND b.state=0 AND b.start_time<NOW() AND b.end_time >NOW() AND a.student_id={0} AND b.id IN({1}) AND e.id IN({2})
GROUP BY b.id
";

        private static readonly string getStudentAvailableCouponListSql = @"
SELECT b.id,b.money,b.name,a.count,b.category_id,b.multiple,b.start_time,b.end_time FROM student_coupon a
INNER JOIN coupon b ON a.coupon_id=b.id
INNER JOIN category d ON b.category_id=d.id
WHERE a.state=0 AND b.state=0 AND b.start_time<NOW() AND b.end_time>NOW() AND a.student_id={0} AND d.id IN({1})
";

        /// <summary>
        /// 取相关的优惠券
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="studentCouponIds"></param>
        /// <param name="productIds"></param>
        /// <returns></returns>
        public static List<CustCoupon> GetStudentCouponList(int studentId, string studentCouponIds, string productIds)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    DataTable dt = dbhelper.ExecuteDataTable(string.Format(getStudentCouponSql, studentId, studentCouponIds, productIds));

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.ToList<CustCoupon>();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("GetStudentCouponList 出错：" + ex, Util.Log.LogType.Error);
            }

            return new List<CustCoupon>();
        }

        /// <summary>
        /// 取相关的优惠券
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="categoryIds"></param>
        /// <returns></returns>
        public static List<CustCoupon> GetStudentAvailableCouponList(int studentId, string categoryIds)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    DataTable dt = dbhelper.ExecuteDataTable(string.Format(getStudentAvailableCouponListSql, studentId, categoryIds));

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.ToList<CustCoupon>();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("GetStudentAvailableCouponList 出错：" + ex, Util.Log.LogType.Error);
            }

            return new List<CustCoupon>();
        }


    }

}
