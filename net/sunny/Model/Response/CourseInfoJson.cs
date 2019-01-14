using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model.Custom
{
    /// <summary>
    /// 课程详情信息
    /// </summary>
    public class CourseInfoJson
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
        /// <summary>
        /// 头部图片地址
        /// </summary>
        public string heading_urls { get; set; }
        /// <summary>
        /// 商品详情
        /// </summary>
        public string detail { get; set; }
        /// <summary>
        /// 课程的评论
        /// </summary>
        public List<ClassCommentJson> commentslist { get; set; } = new List<ClassCommentJson>();

    }

    /// <summary>
    /// 课程的价格信息
    /// </summary>
    public class CoursePriceInfoJson
    {
        /// <summary>
        /// 课程id
        /// </summary>
        public int courseid { get; set; }
        /// <summary>
        /// 课程名称
        /// </summary>
        public string course_name { get; set; }
        /// <summary>
        /// 原价
        /// </summary>
        public decimal price { get; set; }
        /// <summary>
        /// 折扣金额
        /// </summary>
        public decimal discount_money { get; set; }
        /// <summary>
        /// 折扣id
        /// </summary>
        public int discount_id { get; set; }
        /// <summary>
        /// 折扣名称
        /// </summary>
        public string discount_name { get; set; }

    }

}
