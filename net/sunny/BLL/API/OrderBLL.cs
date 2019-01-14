using Sunny.Model;
using Sunny.Model.Custom;
using Sunny.Model.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.DAL
{
    public class OrderBLL
    {
        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="orderRequest"></param>
        /// <returns></returns>
        public static int CreateOrder(OrderRequest orderRequest)
        {
            if (CheckOrderRequest(orderRequest, out decimal dissMoney, out decimal couponMoney))
            {
                ReceiverInfo receiver = DBData.GetInstance(DBTable.receiver_info).GetEntityByKey<ReceiverInfo>(orderRequest.ceceiverid);
                Student user = DBData.GetInstance(DBTable.student).GetEntity<Student>($"username='{orderRequest.user_name}'");
                string orderId = CreateOrderId(user.id);
                int insertCount = DBData.GetInstance(DBTable.order).Add(new Order()
                {
                    address = receiver.address,
                    phone = receiver.phone,
                    receiver = receiver.name,
                    crdate = DateTime.Today,
                    crtime = DateTime.Now,
                    deliver_id = orderRequest.deliverid,
                    freight = 0,
                    message = orderRequest.message,
                    money = orderRequest.money,
                    state = 0,
                    userid = user.id,
                    discount_money = dissMoney,
                    coupon_money = couponMoney,
                    order_id = orderId,
                });

                if (insertCount > 0)
                {   //存储订单相关信息
                    SaveOrderExtraInfo(orderRequest, orderId, user.id);
                }

                return insertCount;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// 存储订单的相关信息
        /// </summary>
        /// <param name="orderRequest"></param>
        /// <param name="orderId"></param>
        private static void SaveOrderExtraInfo(OrderRequest orderRequest, string orderId, int userid)
        {
            try
            {
                CreateCourse(orderRequest.products, orderId, userid);
                CreateOrderCoupons(orderRequest, orderId);
                CreateOrderDiscount(orderRequest.products, orderId);
                CreateOrderProduct(orderRequest.products, orderId);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write("SaveOrderExtraInfo 存储订单的相关信息出错：" + e, Util.Log.LogType.Error);
            }
        }

        /// <summary>
        /// 生成订单号
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        private static string CreateOrderId(int userid)
        {
            return string.Format("{0}_{1}_{2}",
                DateTime.Now.ToString("yyyyMMddHHmmssffff") + Common.Function.GetRangeNumber(4, Common.RangeType.Number),
                userid,
                Common.Function.GetRangeNumber(4, Common.RangeType.Number));
        }

        /// <summary>
        /// 检测数据是否正确
        /// </summary>
        /// <param name="orderRequest"></param>
        /// <returns></returns>
        private static bool CheckOrderRequest(OrderRequest orderRequest, out decimal dissMoney, out decimal couponMoney)
        {
            dissMoney = 0;
            couponMoney = 0;
            return CheckProductPrice(orderRequest.products, out dissMoney) && CheckCoupons(orderRequest, out couponMoney);
        }

        /// <summary>
        /// 检测优惠券数据是否正确
        /// </summary>
        /// <param name="orderRequest"></param>
        /// <returns></returns>
        private static bool CheckCoupons(OrderRequest orderRequest, out decimal money)
        {
            money = 0;
            try
            {
                string productIds = string.Join(",", orderRequest.products.Select(a => a.prouductid));
                string couponIds = string.Join(",", orderRequest.coupons.Select(a => a.id));
                Student user = DBData.GetInstance(DBTable.student).GetEntity<Student>($"username='{orderRequest.user_name}'");
                List<Coupon> coupons = StudentCouponDAL.GetStudentCouponList(user.id, couponIds, productIds);
                if (coupons.Count != orderRequest.coupons.Length)
                    return false;   //若传入的优惠券数量和服务器上查出来的优惠券对不上，则无效

                if (orderRequest.coupon_money != coupons.Sum(a => a.money))
                    return false;   //若传入的优惠券的金额和服务器上查出来 的金额对不上，则无效

                money = coupons.Sum(a => a.money);
                return true;
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write("CheckCoupons 检测订单优惠券数据是否正确时出错：" + e, Util.Log.LogType.Error);
                return false;
            }
        }

        /// <summary>
        /// 创建订单相关的优惠券信息
        /// </summary>
        /// <param name="orderRequest"></param>
        /// <param name="orderId"></param>
        private static void CreateOrderCoupons(OrderRequest orderRequest, string orderId)
        {
            string productIds = string.Join(",", orderRequest.products.Select(a => a.prouductid));
            string couponIds = string.Join(",", orderRequest.coupons.Select(a => a.id));
            Student user = DBData.GetInstance(DBTable.student).GetEntity<Student>($"username='{orderRequest.user_name}'");
            List<Coupon> coupons = StudentCouponDAL.GetStudentCouponList(user.id, couponIds, productIds);

            foreach (Coupon item in coupons)
            {
                try
                {
                    int count = orderRequest.coupons.First(a => a.id == item.id).count;
                    DBData.GetInstance(DBTable.order_coupon).Add(new OrderCoupon()
                    {
                        order_id = orderId,
                        coupon_id = item.id,
                        price = item.money,
                        name = item.name,
                        count = count,
                        money = item.money * count,
                    });
                }
                catch (Exception e)
                {
                    Util.Log.LogUtil.Write("CreateOrderCoupons 创建订单订单优惠券数据时出错：" + e, Util.Log.LogType.Error);
                }
            }
        }

        /// <summary>
        /// 检测商品的价格是否正确
        /// </summary>
        /// <param name="products"></param>
        /// <returns></returns>
        private static bool CheckProductPrice(List<ProductRequest> products, out decimal money)
        {
            money = 0;
            if (products.Count == 0)
                return false;

            foreach (ProductRequest item in products)
            {
                try
                {
                    CoursePriceInfoJson info = CourseDAL.GetCoursePriceInfo(item.prouductid, item.plan_code);
                    if (info.price - info.discount_money != item.price)
                        return false;

                    money += info.discount_money;
                }
                catch (Exception e)
                {
                    Util.Log.LogUtil.Write("CheckProductPrice 检测订单商品的价格是否正确时出错：" + e, Util.Log.LogType.Error);
                }
            }
            return true;
        }

        /// <summary>
        /// 创建订单相关的折扣信息
        /// </summary>
        /// <param name="products"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        private static bool CreateOrderDiscount(List<ProductRequest> products, string orderId)
        {
            if (products.Count == 0)
                return false;

            foreach (ProductRequest item in products)
            {
                try
                {
                    CoursePriceInfoJson info = CourseDAL.GetCoursePriceInfo(item.prouductid, item.plan_code);
                    DBData.GetInstance(DBTable.order_coupon).Add(new OrderDiscount()
                    {
                        order_id = orderId,
                        discount_id = info.discount_id,
                        product_id = info.courseid,
                        name = info.discount_name,
                        money = info.discount_money,
                    });
                }
                catch (Exception e)
                {
                    Util.Log.LogUtil.Write("CreateOrderDiscount 创建订单相关折扣信息时出错：" + e, Util.Log.LogType.Error);
                }
            }
            return true;
        }

        /// <summary>
        /// 创建订单相关的商品信息
        /// </summary>
        /// <param name="products"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        private static bool CreateOrderProduct(List<ProductRequest> products, string orderId)
        {
            if (products.Count == 0)
                return false;

            foreach (ProductRequest item in products)
            {
                try
                {
                    Product product = DBData.GetInstance(DBTable.product).GetEntityByKey<Product>(item.prouductid);
                    CustDisscount discount = DiscountDAL.GetDisscountByProductId(item.prouductid);
                    //存储订单中的商品信息
                    DBData.GetInstance(DBTable.order_coupon).Add(new OrderProduct()
                    {
                        order_id = orderId,
                        product_id = item.prouductid,
                        product_name = product.name,
                        count = item.count,
                        price = item.price,
                        discount_amount = discount.money * item.count,
                        total_amount = item.price * item.count,
                    });
                    //存储订单中各商品的规格信息
                    DBData.GetInstance(DBTable.order_product_specification_detail).Add(new OrderProductSpecificationDetail()
                    {
                        order_id = orderId,
                        product_id = item.prouductid,
                        plan_code = item.plan_code,
                        price = item.price,
                    });
                }
                catch (Exception e)
                {
                    Util.Log.LogUtil.Write("CreateOrderProduct 创建订单相关商品信息时出错：" + e, Util.Log.LogType.Error);
                }
            }
            return true;
        }

        /// <summary>
        /// 创建已购买的课程信息
        /// </summary>
        /// <param name="orderRequest"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        private static bool CreateCourse(List<ProductRequest> products, string orderId, int userid)
        {
            if (products.Count == 0)
                return false;

            foreach (ProductRequest item in products)
            {
                try
                {
                    Category category = CategoryDAL.GetCategoryByProductId(item.prouductid);
                    if (category.type == 0)
                    {
                        Hours hour = DBData.GetInstance(DBTable.hours).GetEntityByKey<Hours>(item.prouductid);

                        //存储订单中各商品的规格信息
                        DBData.GetInstance(DBTable.course).Add(new Course()
                        {
                            product_id = item.prouductid,
                            student_id = userid,
                            venue_id = item.venueid,
                            order_id = orderId,
                            max_count = item.max_person,
                            hour = hour.hour,
                            over_hour = 0,
                        });
                    }
                }
                catch (Exception e)
                {
                    Util.Log.LogUtil.Write("CreateCourse 创建已购买的课程信息时出错：" + e, Util.Log.LogType.Error);
                }
            }
            return true;
        }

    }
}
