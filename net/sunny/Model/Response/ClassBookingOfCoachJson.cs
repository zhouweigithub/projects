using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model.Custom
{
    /// <summary>
    /// 教练可接单的预约信息
    /// </summary>
    public class ClassBookingOfCoachJson
    {
        /// <summary>
        /// 预约id
        /// </summary>
        public int booking_id { get; set; }
        /// <summary>
        /// 起始时间
        /// </summary>
        public DateTime start_time { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime end_time { get; set; }
        /// <summary>
        /// 第几课时
        /// </summary>
        public int hour { get; set; }
        /// <summary>
        /// 最大学生人数
        /// </summary>
        public int max_count { get; set; }
        /// <summary>
        /// 已学完的课时
        /// </summary>
        public int over_hour { get; set; }
        /// <summary>
        /// 商品id
        /// </summary>
        public int product_id { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string product_name { get; set; }
        /// <summary>
        /// 商品主图
        /// </summary>
        public string main_img { get; set; }
        /// <summary>
        /// 场馆名称
        /// </summary>
        public string venue_name { get; set; }
        /// <summary>
        /// 教练id
        /// </summary>
        public int coach_id { get; set; }
        /// <summary>
        /// 场馆id
        /// </summary>
        public int venue_id { get; set; }
        /// <summary>
        /// 学生姓名
        /// </summary>
        public string student_name { get; set; }
        /// <summary>
        /// 学生电话
        /// </summary>
        public string student_phone { get; set; }

    }
}
