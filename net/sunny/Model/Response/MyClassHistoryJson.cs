using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model.Custom
{
    /// <summary>
    /// 上课记录
    /// </summary>
    public class MyClassHistoryJson
    {
        /// <summary>
        /// 上课id
        /// </summary>
        public int class_id { get; set; }
        /// <summary>
        /// 教练id
        /// </summary>
        public int coach_id { get; set; }
        /// <summary>
        /// 商品id
        /// </summary>
        public int product_id { get; set; }
        /// <summary>
        /// 第几课时
        /// </summary>
        public int hour { get; set; }
        /// <summary>
        /// 上课开始时间
        /// </summary>
        public DateTime start_time { get; set; }
        /// <summary>
        /// 上课结束时间
        /// </summary>
        public DateTime end_time { get; set; }
        /// <summary>
        /// 上课状态
        /// </summary>
        public short class_state { get; set; }
        /// <summary>
        /// 学生上课状态
        /// </summary>
        public short student_state { get; set; }
        /// <summary>
        /// 该课总课时
        /// </summary>
        public int total_hour { get; set; }
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
        /// 校区名称
        /// </summary>
        public string campus_name { get; set; }
        /// <summary>
        /// 教练名称
        /// </summary>
        public string coach_name { get; set; }
        /// <summary>
        /// 教练电话
        /// </summary>
        public string coach_phone { get; set; }
    }
}
