using Sunny.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model
{
    /// <summary>
    /// 课程基础信息
    /// </summary>
    public class Course
    {
        public int id { get; set; }
        /// <summary>
        /// 课程名称
        /// </summary>
        [TableField]
        public string name { get; set; }
        /// <summary>
        /// 课程简介
        /// </summary>
        [TableField]
        public string summary { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        [TableField]
        public int price { get; set; }
        /// <summary>
        /// 原价
        /// </summary>
        [TableField]
        public int original_price { get; set; }
        /// <summary>
        /// 学员最大人数
        /// </summary>
        [TableField]
        public short maxStudentCount { get; set; }
        /// <summary>
        /// 学时
        /// </summary>
        [TableField]
        public short class_hour { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        [TableField]
        public DateTime start_date { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime end_date { get; set; }
        /// <summary>
        /// 状态0正常 1不可用
        /// </summary>
        [TableField]
        public short state { get; set; }
        /// <summary>
        /// 教练资金分成比例
        /// </summary>
        [TableField]
        public float rate { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime crtime { get; set; }
    }
}
