using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model.Custom
{
    /// <summary>
    /// 优惠券信息
    /// </summary>
    public class CouponListJson
    {
        /// <summary>
        /// 优惠券id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 优惠券名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal money { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int count { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime start_time { get; set; }
        /// <summary>
        /// 到期时间
        /// </summary>
        public DateTime end_time { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public short status { get; set; }
    }
}
