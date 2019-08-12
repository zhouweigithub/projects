using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spetmall.Model.Page
{
    /// <summary>
    /// 交易流水
    /// </summary>
    public class payInfo
    {
        public DateTime crdate { get; set; }
        /// <summary>
        /// 支付总额
        /// </summary>
        public decimal payMoney { get; set; }
        /// <summary>
        /// 优惠总额
        /// </summary>
        public decimal discountMoney { get; set; }
        /// <summary>
        /// 调价总额
        /// </summary>
        public decimal adjustMomey { get; set; }
        /// <summary>
        /// 利润总额
        /// </summary>
        public decimal profitMoney { get; set; }
        /// <summary>
        /// 成本总额
        /// </summary>
        public decimal costMoney { get; set; }
        /// <summary>
        /// 交易次数
        /// </summary>
        public int payCount { get; set; }
        /// <summary>
        /// 商品金额
        /// </summary>
        public decimal productMoney { get; set; }
        /// <summary>
        /// 会员充值金额
        /// </summary>
        public decimal rechargeMoney { get; set; }
        /// <summary>
        /// 新增洗澡卡金额
        /// </summary>
        public decimal railCardMoney { get; set; }
        /// <summary>
        /// 现金支付金额
        /// </summary>
        public decimal xjMoney { get; set; }
        /// <summary>
        /// 微信支付金额
        /// </summary>
        public decimal wxMoney { get; set; }
        /// <summary>
        /// 支付宝支付金额
        /// </summary>
        public decimal zfbMoney { get; set; }
        /// <summary>
        /// 余额支付金额
        /// </summary>
        public decimal yueMoney { get; set; }
        /// <summary>
        /// 其他方式支付金额
        /// </summary>
        public decimal qitaMoney { get; set; }

    }

    public class countPayInfo
    {
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime crdate { get; set; }
        /// <summary>
        /// 销量
        /// </summary>
        public int count { get; set; }
    }

    public class moneyPayInfo
    {
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime crdate { get; set; }
        /// <summary>
        /// 销量
        /// </summary>
        public decimal money { get; set; }
    }
}
