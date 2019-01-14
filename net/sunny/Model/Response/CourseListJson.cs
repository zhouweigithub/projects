using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model.Custom
{
    /// <summary>
    /// 课程列表基本信息
    /// </summary>
    public class CourseListJson
    {
        /// <summary>
        /// 课程id
        /// </summary>
        public int courseid { get; set; }
        /// <summary>
        /// 课程名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 主图
        /// </summary>
        public string main_img { get; set; }
        /// <summary>
        /// 最低价
        /// </summary>
        public decimal min_price { get; set; }
        /// <summary>
        /// 最高价
        /// </summary>
        public decimal max_price { get; set; }
        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal discount_money { get; set; }
    }
}
