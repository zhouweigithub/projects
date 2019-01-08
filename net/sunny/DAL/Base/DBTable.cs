using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.DAL
{

    /// <summary>
    /// 数据库中所有的表名
    /// </summary>
    public class DBTable
    {
        /// <summary>
        /// 预约请求
        /// </summary>
        public static readonly string appointment = "appointment";
        /// <summary>
        /// 具体每次上课信息
        /// </summary>
        public static readonly string class_ = "class";
        /// <summary>
        /// 课程结束后的反馈信息
        /// </summary>
        public static readonly string class_comment = "class_comment";
        /// <summary>
        /// 上课的学生
        /// </summary>
        public static readonly string class_student = "class_student";
        /// <summary>
        /// 注册教练信息表
        /// </summary>
        public static readonly string coach = "coach";
        /// <summary>
        /// 教练跟教练队长的关系
        /// </summary>
        public static readonly string coach_caption = "coach_caption";
        /// <summary>
        /// 教练队长与场馆的关系
        /// </summary>
        public static readonly string coachcaption_venue = "coachcaption_venue";
        /// <summary>
        /// 优惠券基本信息
        /// </summary>
        public static readonly string coupon = "coupon";
        /// <summary>
        /// 课程基础信息
        /// </summary>
        public static readonly string course = "course";
        /// <summary>
        /// 支付信息
        /// </summary>
        public static readonly string pay_detail = "pay_detail";
        /// <summary>
        /// 支付时使用的优惠券信息
        /// </summary>
        public static readonly string pay_detail_coupon = "pay_detail_coupon";
        /// <summary>
        /// 教练上课后资金结算
        /// </summary>
        public static readonly string settlement = "settlement";
        /// <summary>
        /// 注册用户信息表
        /// </summary>
        public static readonly string student = "student";
        /// <summary>
        /// 学员的优惠券信息
        /// </summary>
        public static readonly string student_coupon = "student_coupon";
        /// <summary>
        /// 学生预约课程情况
        /// </summary>
        public static readonly string student_course = "student_course";
        /// <summary>
        /// 场馆信息
        /// </summary>
        public static readonly string venue = "venue";
        /// <summary>
        /// 提现记录
        /// </summary>
        public static readonly string withdrawal = "withdrawal";

        public static readonly string campus = "campus";
        public static readonly string category = "category";
        public static readonly string class_comment_url = "class_comment_url";
        public static readonly string deliver = "deliver";
        public static readonly string discount = "discount";
        public static readonly string order = "order";
        public static readonly string order_coupon = "order_coupon";
        public static readonly string order_discount = "order_discount";
        public static readonly string order_product = "order_product";
        public static readonly string order_product_specification_detail = "order_product_specification_detail";
        public static readonly string product = "product";
        public static readonly string product_detail = "product_detail";
        public static readonly string product_discount = "product_discount";
        public static readonly string product_headimg = "product_headimg";
        public static readonly string product_specification_detail = "product_specification_detail";
        public static readonly string product_specification_detail_price = "product_specification_detail_price";
        public static readonly string receiver_info = "receiver_info";
        public static readonly string specification = "specification";
        public static readonly string specification_detail = "specification_detail";

    }
}
