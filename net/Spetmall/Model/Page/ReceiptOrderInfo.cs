using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spetmall.Model.Page
{

    /// <summary>
    /// 订单详情
    /// </summary>
    public class ReceiptOrderInfo
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string orderid { get; set; } = string.Empty;
        /// <summary>
        /// 折扣前总金额
        /// </summary>
        public decimal totalprice { get; set; }
        /// <summary>
        /// 折扣金额
        /// </summary>
        public decimal discount_total_price { get; set; }
        /// <summary>
        /// 折扣后应付总金额
        /// </summary>
        public decimal activitytotalprice { get; set; }
        /// <summary>
        /// 货物数量
        /// </summary>
        public decimal buy_number_total { get; set; }
        /// <summary>
        /// 会员id
        /// </summary>
        public int memberid { get; set; }
        /// <summary>
        /// 会员姓名
        /// </summary>
        public string memberName { get; set; }
        /// <summary>
        /// 下单时间
        /// </summary>
        public DateTime crtime { get; set; }
        /// <summary>
        /// 支付方式名称
        /// </summary>
        public string paytype { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 商品数据
        /// </summary>
        public Dictionary<int, ReceiptOrderProductInfo> receiptgoodsdata { get; set; } = new Dictionary<int, ReceiptOrderProductInfo>();
    }

    public class ReceiptOrderProductInfo
    {
        /// <summary>
        /// 商品id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 商品数量
        /// </summary>
        public int number { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal price { get; set; }
        /// <summary>
        /// 库存警戒值
        /// </summary>
        public int alarm { get; set; }
        /// <summary>
        /// 库存量
        /// </summary>
        public int store { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 缩略图
        /// </summary>
        public string mapthumimg { get; set; }
        /// <summary>
        /// 优惠总金额
        /// </summary>
        public decimal discount_goods_total_price { get; set; }
        /// <summary>
        /// 优惠后总金额
        /// </summary>
        public decimal goods_activity_total_price { get; set; }
        /// <summary>
        /// 优惠前总金额
        /// </summary>
        public decimal goods_total_price { get; set; }

    }
}
