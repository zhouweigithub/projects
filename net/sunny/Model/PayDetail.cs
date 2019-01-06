using Sunny.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model
{
    /// <summary>
    /// 支付信息
    /// </summary>
    public class PayDetail
    {
        public string id { get; set; }
        /// <summary>
        /// 课程id
        /// </summary>
        [TableField]
        public int student_id { get; set; }
        /// <summary>
        /// 教练id
        /// </summary>
        [TableField]
        public int course_id { get; set; }
        /// <summary>
        /// 场馆id
        /// </summary>
        [TableField]
        public int total_money { get; set; }
        /// <summary>
        /// 学员id
        /// </summary>
        [TableField]
        public int selfpay_money { get; set; }
        /// <summary>
        /// 学员id
        /// </summary>
        [TableField]
        public int coupon_money { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime crtime { get; set; }
    }
}
