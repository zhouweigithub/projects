using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spetmall.Model.Page
{
    /// <summary>
    /// 确认订单的信息
    /// </summary>
    public class receipt_confirm_products
    {
        /// <summary>
        /// 商品id
        /// </summary>
        public int productId;
        /// <summary>
        /// 数量
        /// </summary>
        public int count;
        /// <summary>
        /// 单价
        /// </summary>
        public decimal price;
        /// <summary>
        /// 成本价
        /// </summary>
        public decimal cost;
        /// <summary>
        /// 分类
        /// </summary>
        public int category;
        /// <summary>
        /// 折扣
        /// </summary>
        public double discount;
        /// <summary>
        /// 条码
        /// </summary>
        public string barcode { get; set; }
        /// <summary>
        /// 缩略图
        /// </summary>
        public string thumbnail;
        /// <summary>
        /// 商品名称
        /// </summary>
        public string productName;
        /// <summary>
        /// 是否启用会员折扣
        /// </summary>
        public bool isDiscounted;
        /// <summary>
        /// 会员信息
        /// </summary>
        public member memberInfo;
        /// <summary>
        /// 折扣活动信息
        /// </summary>
        public receipt_discount discountInfo;
        /// <summary>
        /// 满就减活动信息
        /// </summary>
        public receipt_fullsend fullsendInfo;
        /// <summary>
        /// 总成本
        /// </summary>
        public decimal cost_money
        {
            get
            {
                return cost * count;
            }
        }
        /// <summary>
        /// 折扣前金额
        /// </summary>
        public decimal money
        {
            get
            {
                return price * count;
            }
        }
        /// <summary>
        /// 限时折扣掉的金额，基于原价
        /// </summary>
        public decimal discount_money
        {
            get
            {
                if (discountInfo != null)
                {
                    if (discountInfo.sale > 0 && discountInfo.sale < 10)
                    {
                        return Math.Round(money * (1 - (decimal)discountInfo.sale / 10), 2);
                    }
                }

                return 0;
            }
        }
        /// <summary>
        /// 满就送掉的金额，基于限时折扣后金额
        /// </summary>
        public decimal fullSend_money
        {
            get
            {
                if (fullsendInfo != null)
                {
                    if (fullsendInfo.aim > (decimal)fullsendInfo.sale)
                    {
                        return (decimal)fullsendInfo.sale;
                    }
                }

                return 0;
            }
        }
        /// <summary>
        /// 会员折扣金额，基于满就减后的金额
        /// </summary>
        public decimal member_money
        {
            get
            {
                if (isDiscounted && memberInfo != null && memberInfo.discount > 0 && memberInfo.discount < 10)
                {
                    return Math.Round((money - discount_money - fullSend_money) * (1 - (decimal)memberInfo.discount / 10), 2);
                }
                return 0;
            }
        }
        /// <summary>
        /// 优惠总金额
        /// </summary>
        public decimal total_sale_money
        {
            get
            {
                return discount_money + fullSend_money + member_money;
            }
        }
        /// <summary>
        /// 处理所有优惠后的总金额
        /// </summary>
        public decimal total_money
        {
            get
            {
                return money - total_sale_money;
            }
        }
        /// <summary>
        /// 总利润
        /// </summary>
        public decimal profit_money
        {
            get
            {
                return total_money - cost_money;
            }
        }
        public string discountMsg
        {
            get
            {
                if (discount_money > 0)
                {
                    if (discountInfo.ruleType == 0)
                    {
                        return $"满{(int)discountInfo.aim}件,打{discountInfo.sale}折";
                    }
                    else if (discountInfo.ruleType == 1)
                    {
                        return $"满{discountInfo.aim}元,打{discountInfo.sale}折";
                    }
                    else
                    {
                        return "未知折扣";
                    }
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string fullSendMsg
        {
            get
            {
                if (fullSend_money > 0)
                {
                    return $"满{fullsendInfo.aim}元,减{fullsendInfo.sale}元";
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string memberMsg
        {
            get
            {
                if (member_money > 0)
                {
                    return $"会员折扣,{memberInfo.discount}折";
                }
                else
                {
                    return string.Empty;
                }
            }
        }

    }
}
