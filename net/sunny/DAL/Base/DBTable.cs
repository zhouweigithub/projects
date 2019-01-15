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
        ///// <summary>
        ///// 预约请求
        ///// </summary>
        //public static readonly string appointment = "appointment";
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
        /// 购买成功的课程表
        /// </summary>
        public static readonly string course = "course";
        ///// <summary>
        ///// 支付信息
        ///// </summary>
        //public static readonly string pay_detail = "pay_detail";
        ///// <summary>
        ///// 支付时使用的优惠券信息
        ///// </summary>
        //public static readonly string pay_detail_coupon = "pay_detail_coupon";
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
        ///// <summary>
        ///// 学生预约课程情况
        ///// </summary>
        //public static readonly string student_course = "student_course";
        /// <summary>
        /// 场馆信息
        /// </summary>
        public static readonly string venue = "venue";
        /// <summary>
        /// 提现记录
        /// </summary>
        public static readonly string withdrawal = "withdrawal";

        /// <summary>
        /// 校区
        /// </summary>
        public static readonly string campus = "campus";
        /// <summary>
        /// 商品分类
        /// </summary>
        public static readonly string category = "category";
        /// <summary>
        /// 上课后教练上传的图片或视频
        /// </summary>
        public static readonly string class_comment_url = "class_comment_url";
        /// <summary>
        /// 送货方式
        /// </summary>
        public static readonly string deliver = "deliver";
        /// <summary>
        /// 折扣基本信息
        /// </summary>
        public static readonly string discount = "discount";
        /// <summary>
        /// 购物订单信息
        /// </summary>
        public static readonly string order = "order";
        /// <summary>
        /// 订单使用的优惠券
        /// </summary>
        public static readonly string order_coupon = "order_coupon";
        /// <summary>
        /// 订单涉及的折扣
        /// </summary>
        public static readonly string order_discount = "order_discount";
        /// <summary>
        /// 订单中的商品
        /// </summary>
        public static readonly string order_product = "order_product";
        /// <summary>
        /// 订单商品的规格信息
        /// </summary>
        public static readonly string order_product_specification_detail = "order_product_specification_detail";
        /// <summary>
        /// 商品表
        /// </summary>
        public static readonly string product = "product";
        /// <summary>
        /// 商品详情
        /// </summary>
        public static readonly string product_detail = "product_detail";
        /// <summary>
        /// 商品折扣信息
        /// </summary>
        public static readonly string product_discount = "product_discount";
        /// <summary>
        /// 商品的头部滚动图片
        /// </summary>
        public static readonly string product_headimg = "product_headimg";
        /// <summary>
        /// 商品的规格详情
        /// </summary>
        public static readonly string product_specification_detail = "product_specification_detail";
        /// <summary>
        /// 商品各规格组的价格
        /// </summary>
        public static readonly string product_specification_detail_price = "product_specification_detail_price";
        /// <summary>
        /// 收货人信息
        /// </summary>
        public static readonly string receiver_info = "receiver_info";
        /// <summary>
        /// 规格分类名
        /// </summary>
        public static readonly string specification = "specification";
        /// <summary>
        /// 规格详情
        /// </summary>
        public static readonly string specification_detail = "specification_detail";
        /// <summary>
        /// 课时
        /// </summary>
        public static readonly string hours = "hours";
        /// <summary>
        /// 上课人数分类
        /// </summary>
        public static readonly string course_type = "course_type";
        /// <summary>
        /// 站内各处的图片
        /// </summary>
        public static readonly string banner = "banner";
        /// <summary>
        /// 接单时教练限定
        /// </summary>
        public static readonly string booking_coach_queue = "booking_coach_queue";
        /// <summary>
        /// 用户下的预订信息
        /// </summary>
        public static readonly string booking_student = "booking_student";
        /// <summary>
        /// 优惠券获得历史记录
        /// </summary>
        public static readonly string coupon_history = "coupon_history";
    }
}
