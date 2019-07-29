using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spetmall.Model.Custom
{
    public class PrintObject
    {
        public string ShopName;
        public string OrderNo;
        public string Phone;
        /// <summary>
        /// 微信二维码图片地址
        /// </summary>
        public string QRCodeImg;
        public DateTime Time;
        /// <summary>
        /// 总优惠金额
        /// </summary>
        public decimal ReceiptMoney;
        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal PayMoney;
        /// <summary>
        /// 订单中的商品集
        /// </summary>
        public List<PrintProducts> Products = new List<PrintProducts>();
    }

    public class PrintProducts
    {
        public string Name;
        public int Count;
        public decimal Price;
        public decimal Money;
        /// <summary>
        /// 条形码
        /// </summary>
        public string BarCode;
    }
}
