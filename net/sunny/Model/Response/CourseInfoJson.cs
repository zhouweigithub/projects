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
        public int CourseId;
        /// <summary>
        /// 课程名称
        /// </summary>
        public string Name;
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
        /// <summary>
        /// 头部图片地址
        /// </summary>
        public string Heading_Urls;
        /// <summary>
        /// 商品详情
        /// </summary>
        public string Detail;
        /// <summary>
        /// 课程的评论
        /// </summary>
        public List<ClassCommentJson> CommentsList;

    }

    /// <summary>
    /// 课程的价格信息
    /// </summary>
    public class CoursePriceInfoJson
    {
        /// <summary>
        /// 课程id
        /// </summary>
        public int courseid;
        /// <summary>
        /// 课程名称
        /// </summary>
        public string course_name;
        /// <summary>
        /// 原价
        /// </summary>
        public decimal price;
        /// <summary>
        /// 折扣金额
        /// </summary>
        public decimal discount_money;
        /// <summary>
        /// 折扣id
        /// </summary>
        public int discount_id;
        /// <summary>
        /// 折扣名称
        /// </summary>
        public string discount_name;

    }

}
