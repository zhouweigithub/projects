using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spetmall.Model.Page
{

    /// <summary>
    /// 创建订单时提交的信息
    /// </summary>
    public class orderPost
    {
        /// <summary>
        /// 订单号（挂单的已经存在订单号）
        /// </summary>
        public string orderid { get; set; }
        /// <summary>
        /// 会员id
        /// </summary>
        public int memberid { get; set; }
        /// <summary>
        /// 支付方式 1现金 2微信 3支付宝 4余额 5刷卡 6其他
        /// </summary>
        public short paytype { get; set; }
        /// <summary>
        /// 商品信息
        /// </summary>
        public string products { get; set; }
        /// <summary>
        /// 是否启用会员折扣
        /// </summary>
        public short isMemberDiscount { get; set; }
        /// <summary>
        /// 原始总金额
        /// </summary>
        public decimal totalMoney { get; set; }
        /// <summary>
        /// 应收总金额
        /// </summary>
        public decimal totalNeedMoney { get; set; }
        /// <summary>
        /// 实收总金额
        /// </summary>
        public decimal totalPayMoney { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 支付宝或微信的付款码
        /// </summary>
        public string auth_code { get; set; }
    }
}
