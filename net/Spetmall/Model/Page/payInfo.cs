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
