using Sunny.Model;
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

        private static readonly string GetStudentCouponSql = @"
SELECT a.id,b.money,b.name FROM student_coupon a
INNER JOIN coupon b ON a.coupon_id=b.id
INNER JOIN category d ON b.category_id=d.id
INNER JOIN product e ON d.id=e.category_id
WHERE a.state=0 AND b.state=0 AND b.start_time<NOW() AND b.end_time >NOW() AND a.student_id={0} AND a.id IN({1}) AND e.id IN({2})
GROUP BY a.id";


        public static List<Coupon> GetStudentCouponList(int studentId, string studentCouponIds, string productIds)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    DataTable dt = dbhelper.ExecuteDataTable(string.Format(GetStudentCouponSql, studentId, studentCouponIds, productIds));

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.ToList<Coupon>();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("GetStudentCouponList 出错：" + ex, Util.Log.LogType.Error);
            }

            return new List<Coupon>();
        }

    }

}
