using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spetmall.Model.Common;

namespace Spetmall.Model
{
    /// <summary>
    /// 订单(BASE TABLE)
    /// </summary>
    public class order
    {

        /// <summary>
        /// 订单号
        /// </summary>
        [TableField]
        public string id { get; set; }
        /// <summary>
        /// 商品总金额
        /// </summary>
        [TableField]
        public decimal productMoney { get; set; }
        /// <summary>
        /// 实际支付总金额
        /// </summary>
        [TableField]
        public decimal payMoney { get; set; }
        /// <summary>
        /// 优惠金额
        /// </summary>
        [TableField]
        public decimal discountMoney { get; set; }
        /// <summary>
        /// 调价金额
        /// </summary>
        [TableField]
        public decimal adjustMomey { get; set; }
        /// <summary>
        /// 总成本
        /// </summary>
        [TableField]
        public decimal costMoney { get; set; }
        /// <summary>
        /// 总利润
        /// </summary>
        [TableField]
        public decimal profitMoney { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        [TableField]
        public short payType { get; set; }
        /// <summary>
        /// 会员id
        /// </summary>
        [TableField]
        public int memberid { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [TableField]
        public string remark { get; set; }
        /// <summary>
        /// 0正常订单 1临时挂单
        /// </summary>
        [TableField]
        public short state { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        [TableField]
        public DateTime crdate { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [TableField]
        public DateTime crtime { get; set; }
        public string payTypeString
        {
            get
            {
                string result = string.Empty;
                switch (payType)
                {
                    case 1:
                        result = "现金支付";
                        break;
                    case 2:
                        result = "微信支付";
                        break;
                    case 3:
                        result = "支付宝支付";
                        break;
                    case 4:
                        result = "余额支付";
                        break;
                    case 5:
                        result = "刷卡支付";
                        break;
                    case 6:
                        result = "其他";
                        break;
                    default:
                        result = "未知";
                        break;
                }
                return result;
            }
        }
    }
}
