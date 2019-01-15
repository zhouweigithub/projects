using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunny.DAL;
using Sunny.Model;

namespace Sunny.BLL.API
{
    public class CouponBLL
    {
        /// <summary>
        /// 发放优惠券
        /// </summary>
        /// <param name="studentId">学员id</param>
        /// <param name="couponId">优惠券id</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public bool AddCouponToStudent(int studentId, int couponId, int count)
        {
            //添加优惠券
            int count1 = DBData.GetInstance(DBTable.student_coupon).Add(new StudentCoupon()
            {
                student_id = studentId,
                coupon_id = couponId,
                count = count,
                state = 0,
            });

            //添加历史记录
            int count2 = DBData.GetInstance(DBTable.coupon_history).Add(new CouponHistory()
            {
                student_id = studentId,
                coupon_id = couponId,
                count = count,
            });

            return count1 > 0;
        }
    }
}
