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
SELECT b.id,a.count,b.name,b.money,b.start_time,b.end_time,IF(b.end_time<NOW(),'已过期','未使用')`status` FROM student_coupon a
INNER JOIN coupon b ON a.coupon_id=b.id
LEFT JOIN category c ON b.category_id=c.id OR b.category_id=0
WHERE b.state=0 AND c.type=0 AND a.student_id='{0}'
 ";

        /// <summary>
        /// 获取当前订单可用的默认优惠券
        /// </summary>
        private static readonly string getCouponDefaultOfStudentSql = @"
SELECT b.id,1 count,b.name,b.money,b.start_time,b.end_time FROM student_coupon a
INNER JOIN coupon b ON a.coupon_id=b.id
WHERE a.state=0 AND a.count>0 AND b.state=0 AND b.start_time<NOW() AND b.end_time >NOW() AND b.category_id='{0}' AND a.student_id='{1}'
ORDER BY b.money DESC
";

        private static readonly string addOrUpdateCouponCountSql = @"
INSERT IGNORE INTO student_coupon (student_id,coupon_id,`count`,state) VALUES('{1}','{2}',0,0);
UPDATE student_coupon SET `count`=`count`+{0} WHERE student_id='{1}' AND coupon_id='{2}'";

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
        /// <param name="productId"></param>
        /// <returns></returns>
        public static List<CouponListJson> GetCouponDefaultOfStudent(int studentId, int productId)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    DataTable dt = dbhelper.ExecuteDataTable(string.Format(getCouponDefaultOfStudentSql, productId, studentId));

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.ToList<CouponListJson>();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("GetCouponDefaultOfStudent 出错：" + ex, Util.Log.LogType.Error);
            }

            return new List<CouponListJson>();
        }

        /// <summary>
        /// 增加用户的优惠券数量
        /// </summary>
        /// <param name="studentId">用户id</param>
        /// <param name="couponId">优惠券id</param>
        /// <param name="count">需要增加的数量</param>
        /// <returns></returns>
        public static bool AddCouponCount(int studentId, int couponId, int count)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    int countTmp = dbhelper.ExecuteNonQuery(string.Format(addOrUpdateCouponCountSql, count, studentId, couponId));
                    return countTmp > 0;
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("UpdateCouponCount 出错：" + ex, Util.Log.LogType.Error);
            }

            return false;
        }

    }
}
