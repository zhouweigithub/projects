using Sunny.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model
{
    /// <summary>
    /// 学员支付记录
    /// </summary>
    public class PayRecord
    {
        public int Id { get; set; }
        /// <summary>
        /// 订单id
        /// </summary>
        [TableField]
        public int order_id { get; set; }
        /// <summary>
        /// 支付金额
        /// </summary>
        [TableField]
        public int money { get; set; }
        public int crtime { get; set; }
    }
}
