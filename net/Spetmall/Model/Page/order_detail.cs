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
    public class order_detail : order
    {
        /// <summary>
        /// 会员姓名
        /// </summary>
        public string memberName { get; set; }
        /// <summary>
        /// 会员手机号
        /// </summary>
        public string memberPhone { get; set; }
    }
}
