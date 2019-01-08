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
        public int CourseId;
        /// <summary>
        /// 课程名称
        /// </summary>
        public string Name;
        /// <summary>
        /// 主图
        /// </summary>
        public string Main_Img;
        /// <summary>
        /// 最低价
        /// </summary>
        public string MinPrice;
        /// <summary>
        /// 最高价
        /// </summary>
        public string MaxPrice;
        /// <summary>
        /// 优惠金额
        /// </summary>
        public string Discount_Money;
    }
}
