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
        public int Id;
        /// <summary>
        /// 优惠券名称
        /// </summary>
        public string Name;
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Money;
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime Start_Time;
        /// <summary>
        /// 到期时间
        /// </summary>
        public DateTime End_Time;
        /// <summary>
        /// 状态
        /// </summary>
        public string Status;
    }
}
