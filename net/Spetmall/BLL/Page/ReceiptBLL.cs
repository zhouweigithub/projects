using Spetmall.Common;
using Spetmall.DAL;
using Spetmall.Model;
using Spetmall.Model.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spetmall.BLL.Page
{
    public class ReceiptBLL
    {
        ///// <summary>
        ///// 查找商品参与的限时折扣活动信息
        ///// </summary>
        ///// <param name="product"></param>
        ///// <returns></returns>
        //public static receipt_discount GetProductDiscount(product product)
        //{
        //    try
        //    {
        //        //根据商品查找折扣活动
        //        receipt_discount discount = receiptDAL.GetDiscount(product.id, 2);
        //        if (discount == null)
        //        {   //根据商品分类查找折扣活动
        //            List<int> parents = new List<int>();    //所有父级分类
        //            categoryDAL.GetInstance().GetParentIds(product.category, parents);
        //            foreach (int categoryId in parents)
        //            {
        //                receipt_discount tmp = receiptDAL.GetDiscount(categoryId, 1);
        //                if (tmp != null)
        //                    break;
        //            }
        //        }
        //        if (discount == null)
        //        {   //查找店铺级的折扣活动
        //            discount = receiptDAL.GetDiscount(0, 0);
        //        }

        //        return discount;
        //    }
        //    catch (Exception e)
        //    {
        //        Util.Log.LogUtil.Write("查找商品参与的限时折扣活动信息时出错：" + e, Util.Log.LogType.Error);
        //    }
        //    return null;
        //}

        ///// <summary>
        ///// 查找商品参与的满就送活动信息
        ///// </summary>
        ///// <param name="product"></param>
        ///// <returns></returns>
        //public static receipt_fullsend GetProductFullSend(product product)
        //{
        //    try
        //    {
        //        //根据商品查找满就送活动
        //        receipt_fullsend fullsend = receiptDAL.GetFullSend(product.id, 2);
        //        if (fullsend == null)
        //        {   //根据商品分类查找满就送活动
        //            List<int> parents = new List<int>();    //所有父级分类
        //            categoryDAL.GetInstance().GetParentIds(product.category, parents);
        //            foreach (int categoryId in parents)
        //            {
        //                receipt_fullsend tmp = receiptDAL.GetFullSend(categoryId, 1);
        //                if (tmp != null)
        //                    break;
        //            }
        //        }
        //        if (fullsend == null)
        //        {   //查找店铺级的满就送活动
        //            fullsend = receiptDAL.GetFullSend(0, 0);
        //        }

        //        return fullsend;
        //    }
        //    catch (Exception e)
        //    {
        //        Util.Log.LogUtil.Write("查找商品参与的满就送活动信息时出错：" + e, Util.Log.LogType.Error);
        //    }
        //    return null;
        //}


        /// <summary>
        /// 根据收银时提交的数据获取相关详细数据
        /// </summary>
        /// <param name="memberid">会员id</param>
        /// <param name="products">商品集</param>
        /// <param name="isMemberDiscount">是否启用会员折扣</param>
        /// <returns></returns>
        public static List<receipt_confirm_products> GetDatas(int memberid, string products, short isMemberDiscount)
        {
            Dictionary<int, int> productsList = Util.Json.JsonUtil.Deserialize<Dictionary<int, int>>(products);

            //会员信息
            member member = null;
            if (memberid > 0)
                member = memberDAL.GetInstance().GetEntityByKey<member>(memberid);

            string productIds = string.Empty;
            foreach (int item in productsList.Keys)
            {
                productIds += item + ",";
            }
            productIds = productIds.TrimEnd(',');
            IList<product> productInfoList = productDAL.GetInstance().GetList<product>($"id in({productIds})");

            List<receipt_confirm_products> datas = new List<receipt_confirm_products>();
            foreach (product item in productInfoList)
            {
                receipt_confirm_products tmp = new receipt_confirm_products()
                {
                    productId = item.id,
                    productName = item.name,
                    category = item.category,
                    thumbnail = item.thumbnail,
                    price = item.price,
                    count = productsList[item.id],
                    isDiscounted = item.ismemberdiscount == 1 && isMemberDiscount == 1,
                    memberInfo = member,
                    barcode = item.barcode,
                };

                datas.Add(tmp);
            }

            //匹配各商品的限时折扣活动
            MatchDiscount(datas);

            //匹配各商品的满就减活动
            MatchFullSend(datas);

            return datas;
        }

        /// <summary>
        /// 匹配各商品的限时折扣活动
        /// </summary>
        /// <param name="datas"></param>
        private static void MatchDiscount(List<receipt_confirm_products> datas)
        {
            //找到所有的活动信息，然后匹配各商品信息确认各活动是否生效
            List<receipt_discount_detail> allDiscounts = GetAllDiscounts();

            //按商品
            foreach (receipt_discount_detail item in allDiscounts.Where(a => a.type == 2))
            {
                //购买的那些正在搞这个活动的商品
                var discountProducts = datas.Where(a => item.products.Select(b => b.productid).Contains(a.productId));
                salerule rule = null;
                if (item.way == 0)
                {   //按件折扣
                    rule = item.rules.OrderByDescending(a => a.aim).FirstOrDefault(a => a.aim <= discountProducts.Sum(b => b.count));
                }
                else if (item.way == 1)
                {   //按总金额折扣
                    rule = item.rules.OrderByDescending(a => a.aim).FirstOrDefault(a => a.aim <= discountProducts.Sum(b => b.money));
                }

                if (rule != null)
                {   //给各个做活动的商品添加限时活动信息
                    foreach (receipt_confirm_products confirm_product in discountProducts)
                    {
                        confirm_product.discountInfo = ConvertToDetail(item);
                        confirm_product.discountInfo.ruleType = item.way;
                        confirm_product.discountInfo.aim = rule.aim;
                        confirm_product.discountInfo.sale = rule.sale;
                    }
                }
            }

            //按分类
            foreach (receipt_discount_detail item in allDiscounts.Where(a => a.type == 1))
            {
                List<receipt_confirm_products> discountProducts = new List<receipt_confirm_products>();
                //购买的那些正在搞这个活动的商品，排除掉已经匹配到活动的商品
                foreach (receipt_confirm_products confirm_product in datas.Where(a => a.discountInfo == null))
                {
                    List<int> parents = new List<int>();
                    //当前商品的所有父级分类
                    categoryDAL.GetInstance().GetParentIds(confirm_product.category, parents);
                    foreach (int parent in parents)
                    {   //从近到远遍历父级分类
                        if (item.products.Count(a => a.productid == parent) > 0)
                        {   //如果某个父级分类在做活动
                            discountProducts.Add(confirm_product);
                            break;
                        }
                    }
                }

                salerule rule = null;
                if (item.way == 0)
                {   //按件折扣
                    rule = item.rules.OrderByDescending(a => a.aim).FirstOrDefault(a => a.aim <= discountProducts.Sum(b => b.count));
                }
                else if (item.way == 1)
                {   //按总金额折扣
                    rule = item.rules.OrderByDescending(a => a.aim).FirstOrDefault(a => a.aim <= discountProducts.Sum(b => b.money));
                }

                if (rule != null)
                {   //给各个做活动的商品添加限时活动信息
                    foreach (receipt_confirm_products confirm_product in discountProducts)
                    {
                        confirm_product.discountInfo = ConvertToDetail(item);
                        confirm_product.discountInfo.ruleType = item.way;
                        confirm_product.discountInfo.aim = rule.aim;
                        confirm_product.discountInfo.sale = rule.sale;
                    }
                }
            }

            //按店铺
            foreach (receipt_discount_detail item in allDiscounts.Where(a => a.type == 0))
            {
                //购买的那些正在搞这个活动的商品
                var discountProducts = datas.Where(a => a.discountInfo == null);
                salerule rule = null;
                if (item.way == 0)
                {   //按件折扣
                    rule = item.rules.OrderByDescending(a => a.aim).FirstOrDefault(a => a.aim <= discountProducts.Sum(b => b.count));
                }
                else if (item.way == 1)
                {   //按总金额折扣
                    rule = item.rules.OrderByDescending(a => a.aim).FirstOrDefault(a => a.aim <= discountProducts.Sum(b => b.money));
                }

                if (rule != null)
                {   //给各个做活动的商品添加限时活动信息
                    foreach (receipt_confirm_products confirm_product in discountProducts)
                    {
                        confirm_product.discountInfo = ConvertToDetail(item);
                        confirm_product.discountInfo.ruleType = item.way;
                        confirm_product.discountInfo.aim = rule.aim;
                        confirm_product.discountInfo.sale = rule.sale;
                    }
                }
            }
        }

        /// <summary>
        /// 匹配各商品的满就减活动
        /// </summary>
        /// <param name="datas"></param>
        private static void MatchFullSend(List<receipt_confirm_products> datas)
        {
            //找到所有的活动信息，然后匹配各商品信息确认各活动是否生效
            List<receipt_fullsend_detail> allFullSends = GetAllFullSends();

            //按商品
            foreach (receipt_fullsend_detail item in allFullSends.Where(a => a.type == 2))
            {
                //购买的那些正在搞这个活动的商品，排除掉已经打了限时折扣，同时，限时折扣与满就减不能同时使用的活动
                var discountProducts = datas.Where(a => item.products.Select(b => b.productid).Contains(a.productId) && !(a.discountInfo != null && a.discountInfo.fullsend == 0));
                salerule rule = null;
                //按限时折扣后总金额折扣
                rule = item.rules.OrderByDescending(a => a.aim).FirstOrDefault(a => a.aim <= discountProducts.Sum(b => b.money - b.discount_money));

                if (rule != null)
                {   //给各个做活动的商品添加限时活动信息
                    foreach (receipt_confirm_products confirm_product in discountProducts)
                    {
                        confirm_product.fullsendInfo = ConvertToDetail(item);
                        confirm_product.fullsendInfo.aim = rule.aim;
                        confirm_product.fullsendInfo.sale = rule.sale;
                    }
                }
            }

            //按分类
            foreach (receipt_fullsend_detail item in allFullSends.Where(a => a.type == 1))
            {
                List<receipt_confirm_products> discountProducts = new List<receipt_confirm_products>();
                //购买的那些正在搞这个活动的商品，排除掉已经匹配到活动的商品
                foreach (receipt_confirm_products confirm_product in datas.Where(a => a.fullsendInfo == null && !(a.discountInfo != null && a.discountInfo.fullsend == 0)))
                {
                    List<int> parents = new List<int>();
                    //当前商品的所有父级分类
                    categoryDAL.GetInstance().GetParentIds(confirm_product.category, parents);
                    foreach (int parent in parents)
                    {   //从近到远遍历父级分类
                        if (item.products.Count(a => a.productid == parent) > 0)
                        {   //如果某个父级分类在做活动
                            discountProducts.Add(confirm_product);
                            break;
                        }
                    }
                }

                salerule rule = null;
                //按限时折扣后总金额折扣
                rule = item.rules.OrderByDescending(a => a.aim).FirstOrDefault(a => a.aim <= discountProducts.Sum(b => b.money - b.discount_money));

                if (rule != null)
                {   //给各个做活动的商品添加限时活动信息
                    foreach (receipt_confirm_products confirm_product in discountProducts)
                    {
                        confirm_product.fullsendInfo = ConvertToDetail(item);
                        confirm_product.fullsendInfo.aim = rule.aim;
                        confirm_product.fullsendInfo.sale = rule.sale;
                    }
                }
            }

            //按店铺
            foreach (receipt_fullsend_detail item in allFullSends.Where(a => a.type == 0))
            {
                //购买的那些正在搞这个活动的商品
                var discountProducts = datas.Where(a => a.fullsendInfo == null && !(a.discountInfo != null && a.discountInfo.fullsend == 0));
                salerule rule = null;
                //按限时折扣后总金额折扣
                rule = item.rules.OrderByDescending(a => a.aim).FirstOrDefault(a => a.aim <= discountProducts.Sum(b => b.money - b.discount_money));

                if (rule != null)
                {   //给各个做活动的商品添加限时活动信息
                    foreach (receipt_confirm_products confirm_product in discountProducts)
                    {
                        confirm_product.fullsendInfo = ConvertToDetail(item);
                        confirm_product.fullsendInfo.aim = rule.aim;
                        confirm_product.fullsendInfo.sale = rule.sale;
                    }
                }
            }
        }

        /// <summary>
        /// 取所有的限时折扣活动
        /// </summary>
        /// <returns></returns>
        public static List<receipt_discount_detail> GetAllDiscounts()
        {
            List<receipt_discount_detail> result = new List<receipt_discount_detail>();
            IList<discount> discounts = discountDAL.GetInstance().GetList<discount>("state=1 AND starttime<NOW() AND endtime>NOW()");
            foreach (discount item in discounts)
            {
                receipt_discount_detail detail = ConvertToDetail(item);
                detail.products = saleProductDAL.GetInstance().GetList<saleproduct>($"saleid={item.id}").ToList();
                detail.rules = saleRuleDAL.GetInstance().GetList<salerule>($"saleid={item.id}").ToList();
                result.Add(detail);
            }
            return result;
        }

        /// <summary>
        /// 取所有的满就减活动
        /// </summary>
        /// <returns></returns>
        public static List<receipt_fullsend_detail> GetAllFullSends()
        {
            List<receipt_fullsend_detail> result = new List<receipt_fullsend_detail>();
            IList<fullsend> fullsends = fullsendDAL.GetInstance().GetList<fullsend>("state=1 AND starttime<NOW() AND endtime>NOW()");
            foreach (fullsend item in fullsends)
            {
                receipt_fullsend_detail detail = ConvertToDetail(item);
                detail.products = saleProductDAL.GetInstance().GetList<saleproduct>($"saleid={item.id}").ToList();
                detail.rules = saleRuleDAL.GetInstance().GetList<salerule>($"saleid={item.id}").ToList();
                result.Add(detail);
            }
            return result;
        }

        private static receipt_discount ConvertToDetail(receipt_discount_detail discount)
        {
            return new receipt_discount()
            {
                id = discount.id,
                name = discount.name,
                state = discount.state,
                way = discount.way,
                type = discount.type,
                coupon = discount.coupon,
                fullsend = discount.fullsend,
                starttime = discount.starttime,
                endtime = discount.endtime,
                crtime = discount.crtime,
            };
        }

        private static receipt_fullsend ConvertToDetail(receipt_fullsend_detail fullsend)
        {
            return new receipt_fullsend()
            {
                id = fullsend.id,
                name = fullsend.name,
                state = fullsend.state,
                type = fullsend.type,
                starttime = fullsend.starttime,
                endtime = fullsend.endtime,
                crtime = fullsend.crtime,
            };
        }

        private static receipt_discount_detail ConvertToDetail(discount discount)
        {
            return new receipt_discount_detail()
            {
                id = discount.id,
                name = discount.name,
                state = discount.state,
                way = discount.way,
                type = discount.type,
                coupon = discount.coupon,
                fullsend = discount.fullsend,
                starttime = discount.starttime,
                endtime = discount.endtime,
                crtime = discount.crtime,
            };
        }

        private static receipt_fullsend_detail ConvertToDetail(fullsend fullsend)
        {
            return new receipt_fullsend_detail()
            {
                id = fullsend.id,
                name = fullsend.name,
                state = fullsend.state,
                type = fullsend.type,
                starttime = fullsend.starttime,
                endtime = fullsend.endtime,
                crtime = fullsend.crtime,
            };
        }

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="postData">前端发回的数据</param>
        /// <param name="state">0正常订单 1临时挂单</param>
        /// <returns></returns>
        public static (bool result, string msg) CreateOrder(orderPost postData, short state)
        {
            bool isOk = true;
            string msg = state == 0 ? "收银成功" : "挂单成功";
            string orderid = CreateNewOrderId();
            try
            {
                List<receipt_confirm_products> datas = GetDatas(postData.memberid, postData.products, postData.isMemberDiscount);

                //正常订单需要检测提交的数据是否与服务器一致，挂单先不用检测
                if (state == 0 && !IsOrderValid(postData, datas))
                {
                    return (false, "商品或活动信息已经变化，请重新下单");
                }

                isOk = orderDAL.GetInstance().Add(new order()
                {
                    id = orderid,
                    payType = postData.paytype,
                    payMoney = datas.Sum(a => a.total_money),
                    discountMoney = datas.Sum(a => a.total_sale_money),
                    memberid = postData.memberid,
                    productMoney = datas.Sum(a => a.money),
                    adjustMomey = postData.totalNeedMoney - postData.totalPayMoney,
                    state = state,
                    remark = postData.remark,
                }) > 0;

                foreach (receipt_confirm_products item in datas)
                {
                    isOk = isOk && orderProductDAL.GetInstance().Add(new orderproduct()
                    {
                        orderid = orderid,
                        productid = item.productId,
                        count = item.count,
                        money = item.money,
                        price = item.price,
                        discountMoney = item.total_sale_money,
                        payMoney = item.total_money,
                        barcode = item.barcode,
                        category = item.category,
                        ismemberdiscount = item.isDiscounted ? (short)1 : (short)0,
                        name = item.productName,
                        thumbnail = item.thumbnail,
                    }) > 0;
                }

                if (isOk)
                {
                    if (state == 0)
                    {   //正常下单完成后，需要修改商品的库存和销量
                        foreach (receipt_confirm_products item in datas)
                        {
                            productDAL.GetInstance().ReduceStoreAndSales(item.productId, item.count);
                        }
                    }
                    else if (state == 1)
                    {   //删除挂单的订单信息
                        DeleteOrderById(postData.orderid);
                    }
                    else
                    {
                    }
                }
            }
            catch (Exception e)
            {
                msg = "服务异常";
                Util.Log.LogUtil.Write($"创建订单出错：orderid: {orderid} \r\n{e}", Util.Log.LogType.Error);

                //删除已创建的订单
                DeleteOrderById(orderid);

                isOk = false;
            }

            return (isOk, msg);
        }

        private static string CreateNewOrderId()
        {   //获取首位不为0的随机数
            return Function.GetRangeNumber(1, "123456789") + Function.GetRangeNumber(14, Common.RangeType.Number);
        }

        /// <summary>
        /// 删除订单及商品信息
        /// </summary>
        /// <param name="orderid"></param>
        public static bool DeleteOrderById(string orderid)
        {
            if (string.IsNullOrWhiteSpace(orderid))
                return false;

            bool isOk = true;
            try
            {
                isOk = orderDAL.GetInstance().DeleteByKey(orderid) > 0;
                isOk = isOk && orderProductDAL.GetInstance().Delete($"orderid='{orderid}'") > 0;
            }
            catch (Exception e)
            {
                isOk = false;
                Util.Log.LogUtil.Write($"删除订单出错：orderid: {orderid} \r\n{e}", Util.Log.LogType.Error);
            }

            return isOk;
        }

        private static bool IsOrderValid(orderPost postData, List<receipt_confirm_products> serverData)
        {
            decimal money = serverData.Sum(a => a.money);   //原始总金额
            decimal total_sale_money = serverData.Sum(a => a.total_sale_money); //优惠总金额
            decimal need_pay_money = money - total_sale_money;  //应该支付的总金额

            return postData.totalMoney == money && postData.totalNeedMoney == need_pay_money;
        }

        /// <summary>
        /// 获取订单及其中的商品信息
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public static ReceiptOrderInfo GetOrderInfo(string orderid)
        {
            try
            {
                order order = orderDAL.GetInstance().GetEntityByKey<order>(orderid);
                if (order != null)
                {
                    ReceiptOrderInfo orderInfo = ConvertOrderToReceiptOrderInfo(order);
                    List<ReceiptOrderProductInfo> products = orderProductDAL.GetProductByOrderId(orderid);
                    orderInfo.receiptgoodsdata = ConvertProductDic(products);
                    orderInfo.buy_number_total = products.Sum(a => a.number);
                    return orderInfo;
                }
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write("GetOrderInfo 获取订单信息出错：" + e, Util.Log.LogType.Error);
            }

            return null;
        }

        private static ReceiptOrderInfo ConvertOrderToReceiptOrderInfo(order order)
        {
            if (order == null)
                return null;
            else
                return new ReceiptOrderInfo()
                {
                    orderid = order.id,
                    totalprice = order.productMoney,
                    activitytotalprice = order.payMoney,
                    discount_total_price = order.discountMoney,
                    memberid = order.memberid,
                };
        }

        /// <summary>
        /// 将商品列表转换为字典格式
        /// </summary>
        /// <param name="products"></param>
        /// <returns></returns>
        private static Dictionary<int, ReceiptOrderProductInfo> ConvertProductDic(List<ReceiptOrderProductInfo> products)
        {
            Dictionary<int, ReceiptOrderProductInfo> result = new Dictionary<int, ReceiptOrderProductInfo>();
            foreach (ReceiptOrderProductInfo item in products)
            {
                if (!result.ContainsKey(item.id))
                {
                    result.Add(item.id, item);
                }
            }

            return result;
        }

    }
}
