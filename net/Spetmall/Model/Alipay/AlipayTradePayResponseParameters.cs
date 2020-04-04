using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spetmall.Model.Alipay
{
    /// <summary>
    /// 统一收单交易支付接口返回参数
    /// </summary>
    public class AlipayTradePayResponseParameters
    {
        /// <summary>
        /// 必填 网关返回码,详见文档 10000返回成功
        /// </summary>
        public String code;
        /// <summary>
        /// 必填 网关返回码描述,详见文档
        /// </summary>
        public String msg;
        /// <summary>
        /// 必填 签名,详见文档
        /// </summary>
        public String sign;
        /// <summary>
        /// 必填 交易金额
        /// </summary>
        public decimal total_amount;
        /// <summary>
        /// 必填 实收金额
        /// </summary>
        public String receipt_amount;
        /// <summary>
        /// 必填 交易支付时间
        /// </summary>
        public DateTime gmt_payment;
        //        /// <summary>
        //        /// 必填 交易支付使用的资金渠道
        //        /// </summary>
        //public TradeFundBill fund_bill_list;
        /// <summary>
        /// 必填 买家在支付宝的用户id
        /// </summary>
        public String buyer_user_id;
    }
}
