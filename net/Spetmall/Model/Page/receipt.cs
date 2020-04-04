using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spetmall.Model.Page
{

    public class receipt_discount : discount
    {
        /// <summary>
        /// 0按件折扣、1按价格折扣
        /// </summary>
        public short ruleType { get; set; }
        /// <summary>
        /// 需要达到的最低值
        /// </summary>
        public decimal aim { get; set; }
        /// <summary>
        /// 折扣值
        /// </summary>
        public double sale { get; set; }
    }

    public class receipt_fullsend : fullsend
    {
        /// <summary>
        /// 需要达到的最低值
        /// </summary>
        public decimal aim { get; set; }
        /// <summary>
        /// 实际立减金额
        /// </summary>
        public double sale { get; set; }
        /// <summary>
        /// 活动中的立减金额
        /// </summary>
        public double saleOrign { get; set; }
    }

    /// <summary>
    /// 限时折扣详细信息
    /// </summary>
    public class receipt_discount_detail : discount
    {
        public List<salerule> rules;
        public List<saleproduct> products;
    }

    /// <summary>
    /// 满就送详细信息
    /// </summary>
    public class receipt_fullsend_detail : fullsend
    {
        public List<salerule> rules;
        public List<saleproduct> products;
    }
}
