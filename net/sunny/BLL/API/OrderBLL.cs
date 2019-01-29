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
                ReceiverInfo receiver = DBData.GetInstance(DBTable.receiver_info).GetEntityByKey<ReceiverInfo>(orderRequest.receiverid);
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
                UpdateBalance(orderRequest, orderId, userid);
                CreateCourse(orderRequest.products, orderId, userid);
                CreateOrderCoupons(orderRequest, orderId, userid);
                CreateOrderDiscount(orderRequest.products, orderId);
                CreateOrderProduct(orderRequest.products, orderId);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write("SaveOrderExtraInfo 存储订单的相关信息出错：" + e, Util.Log.LogType.Error);
            }
        }

        /// <summary>
        /// 更新余额
        /// </summary>
        /// <param name="orderRequest"></param>
        /// <param name="orderId"></param>
        /// <param name="userid"></param>
        private static void UpdateBalance(OrderRequest orderRequest, string orderId, int userid)
        {
            if (orderRequest.money == 0)
                return;

            StudentDAL.AddCash(userid, -orderRequest.money);
            Student student = DBData.GetInstance(DBTable.student).GetEntityByKey<Student>(userid);
            DBData.GetInstance(DBTable.pay_record).Add(new PayRecord()
            {
                user_id = userid,
                money = -orderRequest.money,
                type = 0,
                order_id = orderId,
                user_type = 0,
                comment = "购买商品",
                balance = student.cash,
            });
        }

        /// <summary>
        /// 生成订单号
        /// </summary>
        /// <param name="userid">用户唯一标识</param>
        /// <returns></returns>
        private static string CreateOrderId(int userid)
        {
            return string.Format("{0}_{1}", "1" + userid.ToString().PadLeft(6, '0'),
                DateTime.Now.ToString("yyyyMMddHHmmssffff") + Common.Function.GetRangeNumber(4, Common.RangeType.Number)
            );
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
                string productIds = string.Join(",", orderRequest.products.Select(a => a.prouduct_id));
                string couponIds = string.Join(",", orderRequest.coupons.Select(a => a.id));
                Student user = DBData.GetInstance(DBTable.student).GetEntity<Student>($"username='{orderRequest.user_name}'");
                List<CustCoupon> serverCoupons = StudentCouponDAL.GetStudentCouponList(user.id, couponIds, productIds);

                foreach (TmpCoupon item in orderRequest.coupons)
                {
                    var tmpServer = serverCoupons.FirstOrDefault(a => a.id == item.id);
                    if (tmpServer == null || tmpServer.count < item.count || tmpServer.count <= 0)
                        return false;
                }

                money = serverCoupons.Sum(a => a.money * a.count);
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
        private static void CreateOrderCoupons(OrderRequest orderRequest, string orderId, int userid)
        {
            string productIds = string.Join(",", orderRequest.products.Select(a => a.prouduct_id));
            string couponIds = string.Join(",", orderRequest.coupons.Select(a => a.id));
            Student user = DBData.GetInstance(DBTable.student).GetEntity<Student>($"username='{orderRequest.user_name}'");
            List<CustCoupon> coupons = StudentCouponDAL.GetStudentCouponList(user.id, couponIds, productIds);

            foreach (CustCoupon item in coupons)
            {
                try
                {
                    int count = orderRequest.coupons.First(a => a.id == item.id).count;
                    DBData.GetInstance(DBTable.order_coupon).Add(new OrderCoupon()
                    {
                        order_id = orderId,
                        coupon_id = item.id,
                        name = item.name,
                        count = count,
                        money = item.money,
                    });

                    //更新用户优惠券数量
                    CouponDAL.AddCouponCount(userid, item.id, -item.count);
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
                    CustProductPriceInfoJson info = GetPriceInfo(item);
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
                    CustProductPriceInfoJson info = GetPriceInfo(item);
                    DBData.GetInstance(DBTable.order_coupon).Add(new OrderDiscount()
                    {
                        order_id = orderId,
                        discount_id = info.discount_id,
                        product_id = info.product_id,
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
                    Product product = DBData.GetInstance(DBTable.product).GetEntityByKey<Product>(item.prouduct_id);
                    CustDisscount discount = DiscountDAL.GetDisscountByProductId(item.prouduct_id);
                    //存储订单中的商品信息
                    DBData.GetInstance(DBTable.order_coupon).Add(new OrderProduct()
                    {
                        order_id = orderId,
                        product_id = item.prouduct_id,
                        product_name = product.name,
                        count = item.count,
                        price = item.price,
                        orig_price = item.plan_price,
                        discount_amount = discount.money * item.count,
                        total_amount = item.price * item.count,
                        venueid = item.venue_id,
                    });
                    //存储订单中各商品的规格信息
                    DBData.GetInstance(DBTable.order_product_specification_detail).Add(new OrderProductSpecificationDetail()
                    {
                        order_id = orderId,
                        product_id = item.prouduct_id,
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
                    Category category = CategoryDAL.GetCategoryByProductId(item.prouduct_id);
                    if (category.type == 0)
                    {
                        Hours hour = DBData.GetInstance(DBTable.hours).GetEntityByKey<Hours>(item.prouduct_id);

                        //存储订单中各商品的规格信息
                        DBData.GetInstance(DBTable.course).Add(new Course()
                        {
                            product_id = item.prouduct_id,
                            student_id = userid,
                            venue_id = item.venue_id,
                            order_id = orderId,
                            max_count = item.type_id,
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

        public static List<OrderJson> GetOrderInfo(int userid, int state)
        {
            List<OrderJson> result = new List<OrderJson>();
            List<CustOrderProduct> orderProductList = OrderDAL.GetOrderProductList(userid, state);
            string[] orderIds = orderProductList.Select(a => a.order_id).Distinct().ToArray();
            List<CustOrderProductSpecification> specificationList = OrderDAL.GetOrderProductSpecificationList(orderIds);
            List<CustOrderCoupon> couponList = OrderDAL.GetOrderCouponList(orderIds);

            foreach (string orderId in orderIds)
            {
                CustOrderProduct orderInfo = orderProductList.First(a => a.order_id == orderId);
                OrderJson orderObj = new OrderJson()
                {
                    order_id = orderId,
                    money = orderInfo.money,
                    discount_money = orderInfo.discount_money,
                    coupon_money = orderInfo.coupon_money,
                    state = orderInfo.state,
                    crtime = orderInfo.crtime,
                    coupons = new List<OrderCouponJson>(),
                    products = new List<OrderProductJson>(),
                };

                var products = orderProductList.Where(a => a.order_id == orderId);
                foreach (CustOrderProduct product in products)
                {
                    var specification = specificationList.FirstOrDefault(a => a.order_id == orderId && a.product_id == product.product_id);
                    orderObj.products.Add(new OrderProductJson()
                    {
                        order_id = orderId,
                        product_id = product.product_id,
                        product_name = product.product_name,
                        price = product.price,
                        orig_price = product.orig_price,
                        count = product.count,
                        total_amount = product.total_amount,
                        campus = product.campus_name,
                        venue_name = product.venue_name,
                        specifications = specification != null ? specification.specifications : "",
                    });
                }

                var coupons = couponList.Where(a => a.order_id == orderId);
                foreach (CustOrderCoupon coupon in coupons)
                {
                    orderObj.coupons.Add(new OrderCouponJson()
                    {
                        order_id = orderId,
                        name = coupon.name,
                        money = coupon.money,
                        count = coupon.count,
                    });
                }

                result.Add(orderObj);
            }

            return result;
        }

        /// <summary>
        /// 获取商品的价格和折扣信息
        /// </summary>
        /// <param name="productRequest"></param>
        /// <returns></returns>
        private static CustProductPriceInfoJson GetPriceInfo(ProductRequest productRequest)
        {
            CustProductPriceInfoJson result = null;
            Category category = CategoryDAL.GetCategoryByProductId(productRequest.prouduct_id);
            if (category.type == 0)
            {
                result = CourseDAL.GetCoursePriceInfo(productRequest.prouduct_id, productRequest.venue_id, productRequest.type_id);
            }
            else
            {
                result = ProductDAL.GetProductPriceInfo(productRequest.prouduct_id, productRequest.plan_code);
            }

            return result;
        }

    }
}
