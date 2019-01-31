using Sunny.Model;
using Sunny.Model.Custom;
using Sunny.Model.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Sunny.DAL
{
    public class OrderBLL
    {
        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="orderRequest"></param>
        /// <returns></returns>
        public static Order CreateOrder(OrderRequest orderRequest, out string msg)
        {
            string json = Util.Json.JsonUtil.Serialize(orderRequest);
            Util.Log.LogUtil.Write(json, Util.Log.LogType.Debug);

            if (CheckOrderRequest(orderRequest, out decimal dissMoney, out decimal couponMoney, out msg))
            {
                ReceiverInfo receiver = DBData.GetInstance(DBTable.receiver_info).GetEntityByKey<ReceiverInfo>(orderRequest.receiverid);
                Student user = DBData.GetInstance(DBTable.student).GetEntity<Student>($"username='{orderRequest.user_name}'");
                string orderId = CreateOrderId(user.id);
                Order order = new Order()
                {
                    address = receiver?.address,
                    phone = receiver?.phone,
                    receiver = receiver?.name,
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
                };
                int insertCount = DBData.GetInstance(DBTable.order).Add(order);

                if (insertCount > 0)
                {   //存储订单相关信息
                    SaveOrderExtraInfo(orderRequest, orderId, user.id);
                    return order;
                }
                else
                {
                    msg = "保存订单失败";
                    return null;
                }

            }
            else
            {
                return null;
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
                DateTime.Now.ToString("yyyyMMddHHmmssffff") + Common.Function.GetRangeCharaters(4, Common.RangeType.Number)
            );
        }

        /// <summary>
        /// 检测数据是否正确
        /// </summary>
        /// <param name="orderRequest"></param>
        /// <param name="dissMoney"></param>
        /// <param name="couponMoney"></param>
        /// <param name="msg">返回的错误信息</param>
        /// <returns></returns>
        private static bool CheckOrderRequest(OrderRequest orderRequest, out decimal dissMoney, out decimal couponMoney, out string msg)
        {
            msg = string.Empty;
            dissMoney = 0;
            couponMoney = 0;
            if (!CheckProductInfo(orderRequest.products) || !CheckProductPrice(orderRequest.products, out dissMoney))
            {
                msg = "商品信息已发生变化，请重新下单";
                return false;
            }

            bool isOk = CheckCoupons(orderRequest, out couponMoney);
            if (!isOk)
                msg = "优惠券异常";

            return isOk;
        }

        /// <summary>
        /// 检测订单商品分类与服务器商品分类是否一致
        /// </summary>
        /// <param name="products"></param>
        /// <returns></returns>
        private static bool CheckProductInfo(List<ProductRequest> products)
        {
            try
            {
                string productIds = string.Join(",", products.Select(a => a.product_id));
                IList<Product> serverList = DBData.GetInstance(DBTable.product).GetList<Product>($"id in({productIds})");
                foreach (ProductRequest item in products)
                {   //检测前端传入的商品分类与服务端的商品分类是否一致
                    if (serverList.First(a => a.id == item.product_id).category_id != item.category_id)
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"CheckProductInfo 检测订单商品信息时出错：{e}", Util.Log.LogType.Error);
                return false;
            }
        }

        /// <summary>
        /// 检测优惠券数据是否合法
        /// </summary>
        /// <param name="orderRequest"></param>
        /// <param name="money"></param>
        /// <returns></returns>
        private static bool CheckCoupons(OrderRequest orderRequest, out decimal money)
        {
            money = 0;
            try
            {
                Student user = DBData.GetInstance(DBTable.student).GetEntity<Student>($"username='{orderRequest.user_name}'");
                if (orderRequest.coupons == null || orderRequest.coupons.Length == 0)
                {
                    return true;
                }
                else if (orderRequest.coupons.Length == 1)
                {   //仅有一张优惠券
                    var couponId = orderRequest.coupons.First().id.ToString();
                    if (orderRequest.products.Count == 1)
                    {   //一个商品
                        var product = orderRequest.products.First();
                        List<CustCoupon> serverCoupons = StudentCouponDAL.GetStudentCouponList(user.id, couponId, product.product_id.ToString());
                        if (serverCoupons.Count == 1)
                        {
                            //money = Math.Min(serverCoupons[0].money, product.price);
                            money = product.price * product.count >= serverCoupons[0].money ? serverCoupons[0].money : 0;
                            return true;
                        }
                        return false;
                    }
                    else
                    {   //多个商品
                        string productIds = string.Join(",", orderRequest.products.Select(a => a.product_id));
                        List<CustCoupon> serverCoupons = StudentCouponDAL.GetStudentCouponList(user.id, couponId, productIds);
                        //serverCoupons = serverCoupons.Where(a => a.multiple == 0).ToList(); //过滤可一次使用多张的会更券，暂不考虑此情况，默认一次只能使用一张
                        if (serverCoupons.Count == 1)
                        {
                            var products = orderRequest.products.Where(a => a.category_id == serverCoupons[0].category_id);
                            //money = Math.Min(serverCoupons[0].money, products.Sum(a => a.price * a.count));
                            decimal totlalMoney = products.Sum(a => a.price * a.count);     //商品总金额
                            money = totlalMoney >= serverCoupons[0].money ? serverCoupons[0].money : 0;
                            return true;
                        }
                        return false;
                    }
                }
                else
                {   //有多张优惠券
                    string coupons = string.Join(",", orderRequest.coupons.Select(a => a.id));
                    if (orderRequest.products.Count == 1)
                    {   //一个商品
                        var product = orderRequest.products.First();
                        List<CustCoupon> serverCoupons = StudentCouponDAL.GetStudentCouponList(user.id, coupons, product.product_id.ToString());
                        if (serverCoupons.Count == orderRequest.coupons.Length)
                        {
                            //money = Math.Min(serverCoupons.Sum(a => a.money),);
                            decimal totalCouponsMoney = serverCoupons.Sum(a => a.money);    //多张优惠券的金额
                            money = product.price * product.count >= totalCouponsMoney ? totalCouponsMoney : 0;
                            return true;
                        }
                        return false;
                    }
                    else
                    {   //多个商品todo:
                        return false;
                    }
                }

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
            if (orderRequest.coupons == null || orderRequest.coupons.Length == 0)
                return;

            string productIds = string.Join(",", orderRequest.products.Select(a => a.product_id));
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
                    return false;
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
                    if (info != null && info.discount_id > 0)
                    {
                        DBData.GetInstance(DBTable.order_discount).Add(new OrderDiscount()
                        {
                            order_id = orderId,
                            discount_id = info.discount_id,
                            product_id = info.product_id,
                            name = info.discount_name,
                            money = info.discount_money,
                        });
                    }
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
                    Product product = DBData.GetInstance(DBTable.product).GetEntityByKey<Product>(item.product_id);
                    CustDisscount discount = DiscountDAL.GetDisscountByProductId(item.product_id);
                    //存储订单中的商品信息
                    DBData.GetInstance(DBTable.order_product).Add(new OrderProduct()
                    {
                        order_id = orderId,
                        product_id = item.product_id,
                        product_name = product.name,
                        count = item.count,
                        price = item.price,
                        orig_price = item.plan_price,
                        discount_amount = discount == null ? 0 : discount.money * item.count,
                        total_amount = item.price * item.count,
                        venueid = item.venue_id,
                    });
                    //存储订单中各商品的规格信息
                    DBData.GetInstance(DBTable.order_product_specification_detail).Add(new OrderProductSpecificationDetail()
                    {
                        order_id = orderId,
                        product_id = item.product_id,
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
                    Category category = CategoryDAL.GetCategoryByProductId(item.product_id);
                    if (category.type == 0)
                    {
                        Hours hour = DBData.GetInstance(DBTable.hours).GetEntityByKey<Hours>(item.product_id);
                        IList<CourseType> courseTypes = DBData.GetInstance(DBTable.course_type).GetList<CourseType>();
                        int maxCount = courseTypes.First(a => a.id == item.type_id).max_people;
                        //存储订单中各商品的规格信息
                        DBData.GetInstance(DBTable.course).Add(new Course()
                        {
                            product_id = item.product_id,
                            student_id = userid,
                            venue_id = item.venue_id,
                            order_id = orderId,
                            max_count = maxCount,
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
            List<CustOrderProduct> orderProductList = OrderDAL.GetOrderProductList(userid, state, string.Empty);
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
                        main_img = product.main_img,
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
            Category category = CategoryDAL.GetCategoryByProductId(productRequest.product_id);
            if (category.type == 0)
            {
                result = CourseDAL.GetCoursePriceInfo(productRequest.product_id, productRequest.venue_id, productRequest.type_id);
            }
            else
            {
                result = ProductDAL.GetProductPriceInfo(productRequest.product_id, productRequest.plan_code);
            }

            return result;
        }

    }
}
