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
}
