using Spetmall.Common;
using Spetmall.DAL;
using Spetmall.Model;
using Spetmall.Model.Custom;
using Spetmall.Model.Page;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spetmall.BLL.Page
{
    public class ReceiptBLL
    {

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
                    cost = item.cost,
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
                        confirm_product.fullsendInfo.saleOrign = rule.sale;
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
                        confirm_product.fullsendInfo.saleOrign = rule.sale;
                    }
                }

                //分组满就减中各商品减去的金额，应当按照各自的商品总金额按等比分配
                if (rule != null)
                    ResetSaleMoneyOfFullsendCategoryRule(discountProducts);
            }

            //按店铺
            foreach (receipt_fullsend_detail item in allFullSends.Where(a => a.type == 0))
            {
                //购买的那些正在搞这个活动的商品
                List<receipt_confirm_products> discountProducts = datas.Where(a => a.fullsendInfo == null && !(a.discountInfo != null && a.discountInfo.fullsend == 0)).ToList();
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
                        confirm_product.fullsendInfo.saleOrign = rule.sale;
                    }
                }

                //分组满就减中各商品减去的金额，应当按照各自的商品总金额按等比分配
                if (rule != null)
                    ResetSaleMoneyOfFullsendCategoryRule(discountProducts);
            }
        }

        /// <summary>
        /// 取所有的限时折扣活动
        /// </summary>
        /// <returns></returns>
        public static List<receipt_discount_detail> GetAllDiscounts()
        {
            List<receipt_discount_detail> result = new List<receipt_discount_detail>();
            IList<discount> discounts = discountDAL.GetInstance().GetList<discount>("state=1 AND starttime<=CURDATE() AND endtime>=CURDATE()");
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
            IList<fullsend> fullsends = fullsendDAL.GetInstance().GetList<fullsend>("state=1 AND starttime<=CURDATE() AND endtime>=CURDATE()");
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

                order order = new order()
                {
                    id = orderid,
                    payType = postData.paytype,
                    payMoney = postData.totalPayMoney,
                    discountMoney = datas.Sum(a => a.total_sale_money),
                    memberid = postData.memberid,
                    productMoney = datas.Sum(a => a.money),
                    adjustMomey = postData.totalNeedMoney - postData.totalPayMoney,
                    state = state,
                    remark = postData.remark,
                    costMoney = datas.Sum(a => a.cost_money),
                    profitMoney = datas.Sum(a => a.profit_money),
                    crdate = DateTime.Today,
                    crtime = DateTime.Now,
                };
                isOk = orderDAL.GetInstance().Add(order) > 0;

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
                        cost = item.cost,
                        costMoney = item.cost_money,
                        profitMoney = item.profit_money,
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

                        //修改会员的余额
                        if (postData.memberid != 0 && postData.paytype == 4)
                        {
                            UpdateMemberBalance(postData);
                        }

                        //删除挂单的订单信息
                        if (!string.IsNullOrWhiteSpace(postData.orderid))
                        {
                            DeleteOrderById(postData.orderid);
                        }

                        //打印小票
                        PrintObject printObject = new PrintObject()
                        {
                            ShopName = WebConfigData.ReceiptPrinterShopName,
                            Phone = WebConfigData.ReceiptPrinterPhone,
                            QRCodeImg = Const.RootWebPath + "Images\\wx.jpg",
                            OrderNo = orderid,
                            Time = DateTime.Now,
                            ReceiptMoney = order.discountMoney + order.adjustMomey,
                            PayMoney = order.payMoney,
                        };
                        foreach (receipt_confirm_products item in datas)
                        {
                            PrintProducts pdts = new PrintProducts()
                            {
                                Name = item.productName,
                                Price = item.price,
                                Count = item.count,
                                Money = item.money,
                                BarCode = item.barcode,
                            };
                            printObject.Products.Add(pdts);
                        }
                        PrintReceiptBLL printer = new PrintReceiptBLL(printObject);
                        printer.Print();
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

        /// <summary>
        /// 更新会员余额
        /// </summary>
        /// <param name="postData"></param>
        private static void UpdateMemberBalance(orderPost postData)
        {
            decimal balance = 0;
            try
            {
                member currentMember = memberDAL.GetInstance().GetEntityByKey<member>(postData.memberid);
                if (currentMember != null)
                {
                    if (currentMember.money >= postData.totalPayMoney)
                    {
                        balance = currentMember.money - postData.totalPayMoney;
                    }

                    //更新会员余额
                    Dictionary<string, object> fields = new Dictionary<string, object>
                    {
                        { "money", balance }
                    };
                    memberDAL.GetInstance().UpdateByKey(fields, postData.memberid);
                }
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"UpdateMemberBalance 更新会员余额失败 会员id:{postData.memberid} 余额:{balance} \r\n{e}", Util.Log.LogType.Error);
            }
        }

        private static string CreateNewOrderId()
        {   //获取首位不为0的随机数
            //return Function.GetRangeNumber(1, "123456789") + Function.GetRangeNumber(14, Common.RangeType.Number);
            return Converter.ConvertToMySqlTimeStamp(DateTime.Now, 13);
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
        /// <param name="state">0正常订单 1临时挂单</param>
        /// <returns></returns>
        public static ReceiptOrderInfo GetOrderInfo(string orderid, short state)
        {
            try
            {
                List<order_detail> orderList = orderDAL.GetOrderList(orderid, string.Empty, string.Empty, string.Empty, state, 1, int.MaxValue);
                if (orderList.Count > 0)
                {
                    order_detail order = orderList[0];
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

        private static ReceiptOrderInfo ConvertOrderToReceiptOrderInfo(order_detail order)
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
                    adjust_price = order.adjustMomey,
                    memberid = order.memberid,
                    paytype = order.payTypeString,
                    memberName = order.memberName,
                    memberPhone = order.memberPhone,
                    remark = order.remark,
                    crtime = order.crtime,
                };
        }

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

        /// <summary>
        /// 对按分类进行满就减处理的商品，满就减金额按照各商品的支付金额比例分配
        /// </summary>
        /// <param name="discountProducts"></param>
        private static void ResetSaleMoneyOfFullsendCategoryRule(List<receipt_confirm_products> discountProducts)
        {
            if (discountProducts == null || discountProducts.Count() == 0)
                return;

            //平均分配各满就减金额到各个商品中
            decimal totalFullsendMoney = discountProducts.First().fullSend_money;   //总的立减金额
            decimal totalBeforeFullsendMoney = discountProducts.Sum(a => a.money - a.discount_money);  //满就减之前的总金额

            foreach (receipt_confirm_products item in discountProducts)
            {   // 根据该商品的总金额占整个分类总金额的比例来分配该商品的立减金额
                decimal beforeFullsendMoney = item.money - item.discount_money; //满就减之前的金额
                decimal singleFullsendMoney = totalFullsendMoney * (beforeFullsendMoney / totalBeforeFullsendMoney);
                item.fullsendInfo.sale = (double)Math.Round(singleFullsendMoney, 2);
            }

            //分配完后，如果总的分配额大于或少于应该分配的金额，则将差异部分分配到金额最大那个商品上去，最后差异应该在1元以内
            decimal totalFullsendMoneyNow = discountProducts.Sum(a => a.fullSend_money);   //分配后总的立减金额
            decimal diff = totalFullsendMoney - totalFullsendMoneyNow;
            if (diff != 0)
            {
                receipt_confirm_products tmpProduct = discountProducts.OrderByDescending(a => a.total_money).First();
                tmpProduct.fullsendInfo.sale += (double)diff;
            }
        }

    }
}
