using Sunny.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model
{
    /// <summary>
    /// 用户资金变动记录
    /// </summary>
    public class PayRecord
    {
        public int Id { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        [TableField]
        public int user_id { get; set; }
        /// <summary>
        /// 0学员1教练
        /// </summary>
        [TableField]
        public short user_type { get; set; }
        /// <summary>
        /// 购物订单号
        /// </summary>
        [TableField]
        public string order_id { get; set; }
        /// <summary>
        /// 金额(正值为收入，负值为支出)
        /// </summary>
        [TableField]
        public decimal money { get; set; }
        /// <summary>
        /// 类型0购买商品1邀请返现2提现3上课结算收入4充值5注册返现
        /// </summary>
        [TableField]
        public short type { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [TableField]
        public string comment { get; set; }
        /// <summary>
        /// 余额
        /// </summary>
        [TableField]
        public decimal balance { get; set; }
        public DateTime crtime { get; set; }
    }
}
