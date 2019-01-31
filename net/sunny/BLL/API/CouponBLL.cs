using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunny.DAL;
using Sunny.Model;
using Sunny.Model.Custom;
using Sunny.Model.Request;

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
        public static bool AddCouponToStudent(int studentId, int couponId, int count)
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

        /// <summary>
        /// 获取默认的优惠券
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="productId"></param>
        /// <param name="totalMoney"></param>
        /// <returns></returns>
        public static CouponListJson GetDefaultCoupon(int studentId, int productId, decimal totalMoney)
        {
            var coupons = CouponDAL.GetCouponDefaultOfStudent(studentId, productId).OrderByDescending(a => a.money);
            var tmp = coupons.FirstOrDefault(a => a.money <= totalMoney);
            //if (tmp == null)
            //{
            //    coupons = coupons.OrderBy(a => a.money);
            //    tmp = coupons.FirstOrDefault(a => a.money >= totalMoney);
            //    tmp = coupons.FirstOrDefault(a => a.money >= totalMoney);
            //}

            return tmp;
        }


        public static List<CouponListJson> GetAvailableCoupon(int studentId, List<TmpProduct> products)
        {
            List<CouponListJson> result = new List<CouponListJson>();
            string productIds = string.Join(",", products.Select(a => a.product_id));
            IList<Product> productList = DBData.GetInstance(DBTable.product).GetList<Product>($"id in({productIds})");
            string categoryIds = string.Join(",", productList.Select(a => a.category_id));
            List<CustCoupon> coupons = StudentCouponDAL.GetStudentAvailableCouponList(studentId, categoryIds);

            foreach (CustCoupon serverCoupon in coupons)
            {   //该优惠券相关的商品集
                var tmpProductIds = productList.Where(a => a.category_id == serverCoupon.category_id).Select(b => b.id);
                decimal money = products.Where(a => tmpProductIds.Contains(a.product_id)).Sum(b => b.total_money);
                if (money >= serverCoupon.money)
                {
                    result.Add(new CouponListJson()
                    {
                        id = serverCoupon.id,
                        money = serverCoupon.money,
                        count = serverCoupon.count,
                        name = serverCoupon.name,
                        start_time = serverCoupon.start_time,
                        end_time = serverCoupon.end_time,
                        status = "未使用",
                    });
                }
            }

            return result;
        }
    }
}
